using NestedFlowchart.Declaration;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using NestedFlowchart.Rules;
using NestedFlowchart.Templates;
using System.Text;

namespace NestedFlowchart.Functions
{
    public class ExportToCPN
    {
        private readonly Rule1 _rule1;
        private readonly Rule2 _rule2;
        private readonly Rule3 _rule3;
        private readonly Rule4 _rule4;
        private readonly Rule5 _rule5;
        private readonly Rule6 _rule6;
        private readonly Rule7 _rule7;
        private readonly TransformationApproach _approach;

        public ExportToCPN(Rule1 rule1, Rule2 rule2, Rule3 rule3, Rule4 rule4
            , Rule5 rule5, Rule6 rule6, Rule7 rule7, TransformationApproach approach)
        {
            _rule1 = rule1;
            _rule2 = rule2;
            _rule3 = rule3;
            _rule4 = rule4;
            _rule5 = rule5;
            _rule6 = rule6;
            _rule7 = rule7;
            _approach = approach;
        }

        public void ExportFile(string? TemplatePath, string? ResultPath, List<XMLCellNode> sortedFlowcharts)
        {
            string[] allTemplates = ReadAllTemplate(TemplatePath);

            string allColorSet = _approach.CreateAllColorSets(_approach, allTemplates);

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
            PlaceModel rule1Place = new PlaceModel();
            #endregion

            //Declare page position
            PositionManagements page1Position = new PositionManagements();
            PositionManagements page2Position = new PositionManagements();

            TransitionModel definejTransition = new TransitionModel();

            for (int i = 0; i < sortedFlowcharts.Count; i++)
            {
                //Rule1 : Start
                if (sortedFlowcharts[i].NodeType.ToLower() == "start")
                {
                    rule1Place = Rule1(page1Position);
                }
                //Rule2 : Initialize Process
                else if (sortedFlowcharts[i].NodeType.ToLower() == "process"
                    && sortedFlowcharts[i - 2].NodeType.ToLower() == "start")
                {
                    arrayName = Rule2(sortedFlowcharts, allTemplates, countSubPage, pages, previousNode, arrayName, rule1Place, page1Position, i);
                }
                //Rule3 : I=0, J=1 , Rule4
                else if (sortedFlowcharts[i].NodeType.ToLower() == "process")
                {
                    //TODO: Check in case define more than i

                    //Case Not Nested => Define i
                    if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("i ="))
                    {
                        Rule3_1(sortedFlowcharts, allTemplates, countSubPage, pages, previousNode, arrayName, page1Position, i);
                    }
                    //Case Nested => Create Hierachy Tool
                    else if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("j =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("k =")
                        || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("l =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("m ="))
                    {
                        definejTransition = Rule3_2(sortedFlowcharts, allTemplates, ref countSubPage, pages, previousNode, arrayName, page1Position, page2Position, i);
                    }
                    else
                    {
                        countSubPage = Rule4(sortedFlowcharts, allTemplates, pages, previousNode, arrayName, page1Position, i);
                    }
                }
                //Rule 5 Connector
                else if (sortedFlowcharts[i].NodeType.ToLower() == "connector")
                {
                    Rule5(allTemplates, countSubPage, pages, previousNode, arrayName, page1Position);
                }
                //Rule 6 Decision
                else if (sortedFlowcharts[i].NodeType.ToLower() == "condition")
                {
                    Rule6(sortedFlowcharts, allTemplates, countSubPage, pages, previousNode, arrayName, page1Position, i);
                }
                //Rule 7 End
                else if (sortedFlowcharts[i].NodeType.ToLower() == "end")
                {
                    Rule7(allTemplates, out countSubPage, pages, out previousNode, arrayName, page1Position);
                }
            }

            #endregion AppleRules

            string allVar = _approach.CreateAllVariables(_approach, allTemplates, arrayName);

            string allPage = _approach.CreateAllPages(_approach, allTemplates, pages);

            string allInstances = _approach.CreateAllInstances(_approach, allTemplates, definejTransition);

            string firstCPN = string.Format(allTemplates[(int)TemplateEnum.EmptyCPNTemplate],
                allColorSet + allVar, allPage, allInstances);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
        }




        private PlaceModel Rule1(PositionManagements page1Position)
        {
            return _rule1.ApplyRule(page1Position);
        }
        private string Rule2(List<XMLCellNode> sortedFlowcharts, string[] allTemplates, int countSubPage, PageDeclare pages, PreviousNode previousNode, string arrayName, PlaceModel rule1Place, PositionManagements page1Position, int i)
        {
            arrayName = _rule2.AssignInitialMarking(
                                    sortedFlowcharts,
                                    arrayName,
                                    rule1Place,
                                    i);

            var (rule2Place, rule2Transition, rule2Arc1, rule2Arc2) = _rule2.ApplyRule(
                rule1Place,
                arrayName,
                page1Position);

            //Set previous node for create arc next rule
            previousNode.previousPlaceModel = rule2Place;
            previousNode.previousTransitionModel = rule2Transition;
            previousNode.Type = "place";


            //Rule2 need to create Rule1 here because initial marking
            var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule1Place);

            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule2Arc1);
            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule2Transition);
            var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule2Arc2);
            var place2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule2Place);
            var rule2String = place1 + place2 + transition + arc1 + arc2;

            CreatePageNodeByCountSubPage(countSubPage, pages, rule2String);
            return arrayName;
        }
        private void Rule3_1(List<XMLCellNode> sortedFlowcharts, string[] allTemplates, int countSubPage, PageDeclare pages, PreviousNode previousNode, string arrayName, PositionManagements page1Position, int i)
        {
            //TODO: Find solution to declare var
            //In case declare more than 1 line
            sortedFlowcharts[i].ValueText = sortedFlowcharts[i].ValueText.Replace("<br>", "\n");
            sortedFlowcharts[i].ValueText = sortedFlowcharts[i].ValueText.ToLower().Replace("int", "");
            sortedFlowcharts[i].ValueText = sortedFlowcharts[i].ValueText.Replace(";", "");

            var (rule3Place, rule3Transition, rule3Arc1, rule3Arc2) = _rule3.ApplyRuleWithoutHierarchy(
                sortedFlowcharts[i].ValueText,
                arrayName,
                previousNode,
                page1Position
                );

            previousNode.previousPlaceModel = rule3Place;
            previousNode.Type = "place";

            var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3Place);
            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc1);
            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition);
            var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc2);

            var rule3OldString = place1 + transition + arc1 + arc2;

            CreatePageNodeByCountSubPage(countSubPage, pages, rule3OldString);
        }
        private TransitionModel Rule3_2(List<XMLCellNode> sortedFlowcharts, string[] allTemplates, ref int countSubPage, PageDeclare pages, PreviousNode previousNode, string arrayName, PositionManagements page1Position, PositionManagements page2Position, int i)
        {
            TransitionModel definejTransition;
            var (rule3InputPlace, rule3OutputPlace, rule3InputPlace2, rule3OutputPlace2, rule3PS2,
                                        rule3Transition, rule3Transition2,
                                        rule3Arc1, rule3Arc2, rule3Arc3, rule3Arc4, rule3Arc5) = _rule3.ApplyRuleWithHierarchy(
                                        allTemplates[(int)TemplateEnum.SubStrTemplate],
                                        allTemplates[(int)TemplateEnum.PortTemplate],
                                        pages.subPageModel1.Id,
                                        sortedFlowcharts[i].ValueText,
                                        arrayName,
                                        previousNode,
                                        page1Position,
                                        page2Position
                                        );

            //Set previous in subpage first
            previousNode.previousPlaceModel = rule3PS2;
            definejTransition = rule3Transition;
            previousNode.Type = "place";

            //Main Page
            var inputPlace = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3InputPlace);
            var subPageTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition);
            var outputPlace = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3OutputPlace);
            var arc0 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc1);
            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc2);
            var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc3);

            var rule3OldString = inputPlace + subPageTransition + outputPlace + arc0 + arc1 + arc2;

            //Sub Page
            var inputPlace2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3InputPlace2);
            var outputPlace2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3OutputPlace2);
            var afterInputTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule3Transition2);
            var arc3 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc4);
            var ps2 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule3PS2);
            var arc4 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule3Arc5);

            var rule3NewString = inputPlace2 + outputPlace2 + afterInputTransition + arc3 + ps2 + arc4;

            //Add to main page 
            CreatePageNodeByCountSubPage(countSubPage, pages, rule3OldString);

            //Add to sub page
            countSubPage++;
            CreatePageNodeByCountSubPage(countSubPage, pages, rule3NewString);
            return definejTransition;
        }
        private int Rule4(List<XMLCellNode> sortedFlowcharts, string[] allTemplates, PageDeclare pages, PreviousNode previousNode, string arrayName, PositionManagements page1Position, int i)
        {
            int countSubPage;
            //countSubPage = 0,1 to test create on current page

            if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("i ++"))
            {
                //TODO: Send real process to code segment inscription
                //TODO: Find solution to create arc
                var (rule4Place, rule4Transition, _, rule4String) = _rule4.ApplyRule(
                    allTemplates[(int)TemplateEnum.TransitionTemplate],
                    allTemplates[(int)TemplateEnum.PlaceTemplate],
                    allTemplates[(int)TemplateEnum.ArcTemplate],
                    arrayName,
                    previousNode,
                    page1Position);

                previousNode.previousPlaceModel = rule4Place;
                previousNode.previousTransitionModel = rule4Transition;
                previousNode.Type = "transition";

                countSubPage = 0;
                CreatePageNodeByCountSubPage(countSubPage, pages, rule4String);
            }

            countSubPage = 1;
            return countSubPage;
        }
        private void Rule5(string[] allTemplates, int countSubPage, PageDeclare pages, PreviousNode previousNode, string arrayName, PositionManagements page1Position)
        {
            var (rule5Place, rule5Transition, rule5Arc1, rule5Arc2) = _rule5.ApplyRule(
                                    arrayName,
                                    previousNode.previousPlaceModel,
                                    page1Position);

            previousNode.previousPlaceModel = rule5Place;
            previousNode.previousTransitionModel = rule5Transition;
            previousNode.Type = "place";


            var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule5Place);
            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule5Arc1);
            var transition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule5Transition);
            var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule5Arc2);
            var rule5String = place1 + transition + arc1 + arc2;

            CreatePageNodeByCountSubPage(countSubPage, pages, rule5String);
        }
        private void Rule6(List<XMLCellNode> sortedFlowcharts, string[] allTemplates, int countSubPage, PageDeclare pages, PreviousNode previousNode, string arrayName, PositionManagements page1Position, int i)
        {
            string trueCondition = _rule6.CreateTrueCondition(sortedFlowcharts[i].ValueText, arrayName);
            string falseCondition = _rule6.CreateFalseDecision(trueCondition);

            //TODO: Replace Array with List.nth(arr,j)

            //TODO: Separate between true and false case by arrow[i+1]

            var (rule6Place, rule6FalseTransition, rule6TrueTransition, rule6Arc1, rule6Arc2) = _rule6.ApplyRule(
                previousNode.previousPlaceModel,
                trueCondition,
                falseCondition,
                arrayName,
                page1Position);

            previousNode.previousPlaceModel = rule6Place;
            previousNode.previousTransitionModel = rule6TrueTransition;
            previousNode.Type = "transition";


            var trueTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule6TrueTransition);
            var falseTransition = _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule6FalseTransition);
            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule6Arc1);
            var arc2 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule6Arc2);
            var rule6String = trueTransition + falseTransition + arc1 + arc2;

            CreatePageNodeByCountSubPage(countSubPage, pages, rule6String);
        }
        private void Rule7(string[] allTemplates, out int countSubPage, PageDeclare pages, out PreviousNode previousNode, string arrayName, PositionManagements page1Position)
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





            var (rule7Place, rule7Transition, rule7Arc1) = _rule7.ApplyRule(
                arrayName,
                previousNode,
                page1Position);

            previousNode.previousPlaceModel = rule7Place;

            var place1 = _approach.CreatePlace(allTemplates[(int)TemplateEnum.PlaceTemplate], rule7Place);

            var transition = rule7Transition != null ?
                _approach.CreateTransition(allTemplates[(int)TemplateEnum.TransitionTemplate], rule7Transition) :
                string.Empty;

            var arc1 = _approach.CreateArc(allTemplates[(int)TemplateEnum.ArcTemplate], rule7Arc1);
            var rule7String = place1 + transition + arc1;

            //Reset because it's need to end at main page
            countSubPage = 0;
            CreatePageNodeByCountSubPage(countSubPage, pages, rule7String);
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