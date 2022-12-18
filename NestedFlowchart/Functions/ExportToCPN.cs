using NestedFlowchart.Declaration;
using NestedFlowchart.Models;
using NestedFlowchart.Rules;
using NestedFlowchart.Templates;
using System.Text;

namespace NestedFlowchart.Functions
{
    public class ExportToCPN
    {
        public void ExportFile(string? TemplatePath, string? ResultPath, List<XMLCellNode> sortedFlowcharts)
        {
            TransformationApproach approach = new TransformationApproach();
            string[] allTemplates = ReadAllTemplate(TemplatePath);

            string allColorSet = approach.CreateAllColorSets(approach, allTemplates);

            #region AppleRules

            int countSubPage = 0;
            PageDeclare pages = new PageDeclare();
            PreviousNode previousNode = new PreviousNode();

            #region Array Name
            /*
             * Declare Array name for arc
             */
            string arrayName = string.Empty;
            #endregion

            #region Rule1 Variable
            /*
             * Need to declare these variable to temp the Rule1 because
             * It use on initialize marking on Rule2
             */
            string rule1String = string.Empty;
            PlaceModel rule1Place = new PlaceModel();
            #endregion


            TransitionModel definejTransition = new TransitionModel();

            for (int i = 0; i < sortedFlowcharts.Count; i++)
            {
                //Rule1 : Start
                if (sortedFlowcharts[i].NodeType.ToLower() == "start")
                {
                    Rule1 rule1 = new Rule1();
                    rule1Place = rule1.ApplyRule();

                    previousNode.previousPlaceModel = rule1Place;
                    previousNode.Type = "place";
                }
                //Rule2 : Initialize Process
                else if (sortedFlowcharts[i].NodeType.ToLower() == "process"
                    && sortedFlowcharts[i - 2].NodeType.ToLower() == "start")
                {
                    
                    Rule2 rule2 = new Rule2();
                    arrayName = rule2.AssignInitialMarking(
                        sortedFlowcharts, 
                        arrayName, 
                        rule1Place, 
                        i);

                    var (rule2Place, rule2Transition, rule2Arc1, rule2Arc2) = rule2.ApplyRule(
                        rule1Place,
                        arrayName);

                    previousNode.previousPlaceModel = rule2Place;
                    previousNode.previousTransitionModel = rule2Transition;
                    previousNode.Type = "place";


                    //Rule2 need to create Rule1 here because initial marking
                    var place1 = approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule1Place);

                    var arc1 = approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule2Arc1);
                    var transition = approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule2Transition);
                    var arc2 = approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule2Arc2);
                    var place2 = approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule2Place);
                    var rule2String = place1 + place2 + transition + arc1 + arc2;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule2String);
                }
                //Rule3 : I=0, J=1 , Rule4
                else if (sortedFlowcharts[i].NodeType.ToLower() == "process")
                {
                    //TODO: Check in case define more than i

                    //Case Not Nested => Define i
                    if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("i ="))
                    {
                        //TODO: Find solution to declare var
                        //In case declare more than 1 line
                        sortedFlowcharts[i].ValueText = sortedFlowcharts[i].ValueText.Replace("<br>", "\n");
                        sortedFlowcharts[i].ValueText = sortedFlowcharts[i].ValueText.ToLower().Replace("int", "");
                        sortedFlowcharts[i].ValueText = sortedFlowcharts[i].ValueText.Replace(";", "");

                        Rule3 rule3 = new Rule3();
                        var (rule3Place, _, _, rule3OldString, _) = rule3.ApplyRuleWithoutHierarchy(
                            allTemplates[(int)TemplateEnum.TransitionTemplate], 
                            allTemplates[(int)TemplateEnum.PlaceTemplate], 
                            allTemplates[(int)TemplateEnum.ArcTemplate],
                            sortedFlowcharts[i].ValueText,
                            arrayName,
                            previousNode
                            );

                        previousNode.previousPlaceModel = rule3Place;
                        previousNode.Type = "place";

                        pages.mainPageModel.Node += rule3OldString;
                    }
                    //Case Nested => Create Hierachy Tool
                    else if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("j =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("k =")
                        || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("l =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("m ="))
                    {
                        Rule3 rule3 = new Rule3();
                        var (rule3Place, rule3Transition, _, rule3OldString, rule3NewString) = rule3.ApplyRuleWithHierarchy(
                            allTemplates[(int)TemplateEnum.TransitionTemplate], 
                            allTemplates[(int)TemplateEnum.PlaceTemplate], 
                            allTemplates[(int)TemplateEnum.ArcTemplate], 
                            allTemplates[(int)TemplateEnum.SubStrTemplate],
                            allTemplates[(int)TemplateEnum.PortTemplate],
                            pages.subPageModel1.Id,
                            sortedFlowcharts[i].ValueText,
                            arrayName,
                            previousNode
                            );

                        previousNode.previousPlaceModel = rule3Place;
                        definejTransition = rule3Transition;
                        previousNode.Type = "place";

                        //Add to old page
                        pages.mainPageModel.Node += rule3OldString;

                        //Add to sub page
                        //countSubPage indicate current page you are in
                        countSubPage++;
                        CreatePageNodeByCountSubPage(countSubPage, pages, rule3NewString);
                    }
                    else
                    {
                        //countSubPage = 0,1 to test create on current page

                        if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("i ++"))
                        {
                            //TODO: Send real process to code segment inscription
                            //TODO: Find solution to create arc
                            Rule4 rule4 = new Rule4();
                            var (rule4Place, rule4Transition, _, rule4String) = rule4.ApplyRule(
                                allTemplates[(int)TemplateEnum.TransitionTemplate], 
                                allTemplates[(int)TemplateEnum.PlaceTemplate], 
                                allTemplates[(int)TemplateEnum.ArcTemplate], 
                                arrayName,
                                previousNode);

                            previousNode.previousPlaceModel = rule4Place;
                            previousNode.previousTransitionModel = rule4Transition;
                            previousNode.Type = "transition";

                            countSubPage = 0;
                            CreatePageNodeByCountSubPage(countSubPage, pages, rule4String);
                        }

                        countSubPage = 1;
                    }
                }
                //Rule 5 Connector
                else if (sortedFlowcharts[i].NodeType.ToLower() == "connector")
                {
                    Rule5 rule5 = new Rule5();
                    var (rule5Place, rule5Transition, rule5Arc1, rule5Arc2) = rule5.ApplyRule(
                        arrayName,
                        previousNode.previousPlaceModel);

                    previousNode.previousPlaceModel = rule5Place;
                    previousNode.previousTransitionModel = rule5Transition;
                    previousNode.Type = "place";

                    
                    var place1 = approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule5Place);
                    var arc1 = approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule5Arc1);
                    var transition = approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule5Transition);
                    var arc2 = approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule5Arc2);
                    var rule5String = place1 + transition + arc1 + arc2;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule5String);
                }
                //Rule 6 Decision
                else if (sortedFlowcharts[i].NodeType.ToLower() == "condition")
                {
                    Rule6 rule6 = new Rule6();
                    string trueCondition = rule6.CreateTrueCondition(sortedFlowcharts[i].ValueText, arrayName);
                    string falseCondition = rule6.CreateFalseDecision(trueCondition);

                    //TODO: Replace Array with List.nth(arr,j)

                    //TODO: Separate between true and false case by arrow[i+1]

                    var (rule6Place, rule6FalseTransition, rule6TrueTransition, rule6Arc1, rule6Arc2) = rule6.ApplyRule(
                        previousNode.previousPlaceModel,
                        trueCondition,
                        falseCondition,
                        arrayName);

                    previousNode.previousPlaceModel = rule6Place;
                    previousNode.previousTransitionModel = rule6TrueTransition;
                    previousNode.Type = "transition";


                    var trueTransition = approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule6TrueTransition);
                    var falseTransition = approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule6FalseTransition);
                    var arc1 = approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule6Arc1);
                    var arc2 = approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule6Arc2);
                    var rule6String = trueTransition + falseTransition + arc1 + arc2;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule6String);
                }
                //Rule 7 End
                else if (sortedFlowcharts[i].NodeType.ToLower() == "end")
                {
                    //TODO: Previous transition need to check (in subpage)

                    /*
                     * For Test (force set GF to end)
                     */
                    previousNode = new PreviousNode
                    {
                        previousTransitionModel = new TransitionModel
                        {
                            Id1 = "ID1412848787"
                        },
                        Type = "transition",
                    };





                    Rule7 rule7 = new Rule7();
                    var (rule7Place, rule7Transition, rule7Arc1) = rule7.ApplyRule(
                        arrayName,
                        previousNode);

                    previousNode.previousPlaceModel = rule7Place;

                    var place1 = approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule7Place);

                    var transition = rule7Transition != null ? 
                        approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule7Transition) : 
                        string .Empty;

                    var arc1 = approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule7Arc1);
                    var rule7String = place1 + transition + arc1;

                    //Reset because it's need to end at main page
                    countSubPage = 0;
                    CreatePageNodeByCountSubPage(countSubPage, pages, rule7String);
                }
            }

            #endregion AppleRules

            string allVar = approach.CreateAllVariables(approach, allTemplates, arrayName);

            string allPage = approach.CreateAllPages(approach, allTemplates, pages);

            string allInstances = approach.CreateAllInstances(approach, allTemplates, definejTransition);

            string firstCPN = string.Format(allTemplates[(int)TemplateEnum.EmptyCPNTemplate],
                allColorSet + allVar, allPage, allInstances);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
        }

        private string[] ReadAllTemplate(string? TemplatePath)
        {
            string[] templateNames = {
                "EmptyNet.txt", "Place.txt", "Transition.txt", "Arc.txt",
                "VarType.txt", "ColorSet.txt", "Hierarchy_Instance.txt",
                "Page.txt", "Hierarchy_SubPageTransition.txt", "Hierarchy_Port.txt"
            };

            string[] allTemplates = new string[10];

            for (int i = 0; i < 10; i++)
            {
                allTemplates[i] = File.ReadAllText(TemplatePath + templateNames[i]);
            }

            return allTemplates;
        }

        private void CreatePageNodeByCountSubPage(int countSubPage, PageDeclare pages, string rule)
        {
            if (countSubPage < 0 || countSubPage > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(countSubPage), "โปรแกรม Support ไม่เกิน 5 nested loops, ปัจจุบันมี : " + countSubPage);
            }

            switch (countSubPage)
            {
                case 0:
                    pages.mainPageModel.Node += rule;
                    break;
                case 1:
                    pages.subPageModel1.Node += rule;
                    break;
                case 2:
                    pages.subPageModel2.Node += rule;
                    break;
                case 3:
                    pages.subPageModel3.Node += rule;
                    break;
                case 4:
                    pages.subPageModel4.Node += rule;
                    break;
            }
        }


    }
}