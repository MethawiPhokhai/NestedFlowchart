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

            #region Color Set
            ColorSetModel colorSetProduct1 = new ColorSetModel()
            {
                Id = IdManagements.GetlastestColorSetId(),
                Name = "loopi",
                Type = new List<string>()
                {
                    "INT",
                    "INTs"
                },
                Text = "colset loopi = product INT*INTs;"
            };

            var col1 = approach.CreateColorSet(allTemplates[(int)TemplateEnum.ColorSetTemplate], colorSetProduct1);

            #endregion

            #region Var
            VarModel var1Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INTs",
                Name = "arr",
                Layout = "var arr: INTs;"
            };

            VarModel var2Model = new VarModel()
            {
                Id = IdManagements.GetlastestVarId(),
                Type = "INT",
                Name = "i,i2,j,j2",
                Layout = "var i,i2,j,j2: INT;"
            };

            var var1 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var1Model);
            var var2 = approach.CreateVar(allTemplates[(int)TemplateEnum.VarTemplate], var2Model);

            #endregion

            #region Page
            int countSubPage = 0;
            PageDeclare pages = new PageDeclare();
            PreviousNode previousNode = new PreviousNode();

            //Declaration
            string ruleString = string.Empty;
            PlaceModel rulePlace = new PlaceModel();

            TransitionModel definejTransition = new TransitionModel();


            for (int i = 0; i < sortedFlowcharts.Count; i++)
            {
                //Rule1 : Start
                if (sortedFlowcharts[i].NodeType.ToLower() == "start")
                {
                    Rule1 rule1 = new Rule1();
                    var rule1Result = rule1.ApplyRule(allTemplates[(int)TemplateEnum.PlaceTemplate]);

                    rulePlace = rule1Result.Item1;
                    ruleString = rule1Result.Item2.ToString();

                    previousNode.previousPlaceModel = rulePlace;
                    previousNode.Type = "place";
                }
                //Rule2 : Initialize Process
                else if (sortedFlowcharts[i].NodeType.ToLower() == "process"
                    && sortedFlowcharts[i - 2].NodeType.ToLower() == "start")
                {
                    //Initial Marking
                    //TODO : กรณี ตัดตัวอีกษร Array
                    if (sortedFlowcharts[i].ValueText.Contains('[') || sortedFlowcharts[i].ValueText.ToLower().Contains("arr"))
                    {
                        var arrayValue = sortedFlowcharts[i].ValueText.Substring(sortedFlowcharts[i].ValueText.IndexOf('=') + 1).Trim();

                        rulePlace.Type = "INTs";
                        rulePlace.InitialMarking = arrayValue;
                    }

                    Rule2 rule2 = new Rule2();
                    var rule2Result = rule2.ApplyRule(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], rulePlace);

                    PlaceModel rule2Place = rule2Result.Item1;
                    TransitionModel rule2Transition = rule2Result.Item2;
                    string rule2String = rule2Result.Item4;

                    previousNode.previousPlaceModel = rule2Place;
                    previousNode.previousTransitionModel = rule2Transition;
                    previousNode.Type = "place";

                    //From previous rule
                    ruleString = rule2String.Contains("Start") ? string.Empty : ruleString;

                    //Combine All String and add to page node
                    StringBuilder allString = new StringBuilder();
                    allString.Append(pages.mainPageModel.Node);
                    allString.Append(ruleString);
                    allString.Append(rule2String);
                    pages.mainPageModel.Node = allString.ToString();
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

                        //VarModel var3Model = new VarModel()
                        //{
                        //    Id = IdManagements.GetlastestVarId(),
                        //    Type = "INT",
                        //    Name = "i,i2,j,j2",
                        //    Layout = "var i,i2,j,j2: INT;"
                        //};

                        //var var2 = approach.CreateVar(allTemplates[(int)TemplateEnum.varTemplate], var2Model);

                        Rule3 rule3 = new Rule3();
                        var rule3Result = rule3.ApplyRuleWithoutHierarchy(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate]
                            , previousNode, sortedFlowcharts[i].ValueText);

                        previousNode.previousPlaceModel = rule3Result.Item1;
                        previousNode.Type = "place";


                        pages.mainPageModel.Node += rule3Result.Item4;
                    }
                    //Case Nested => Create Hierachy Tool
                    else if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("j =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("k =")
                        || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("l =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("m ="))
                    {

                        Rule3 rule3 = new Rule3();
                        var definej = rule3.ApplyRuleWithHierarchy(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], allTemplates[(int)TemplateEnum.SubStrTemplate], allTemplates[(int)TemplateEnum.PortTemplate],
                            previousNode, pages.subPageModel1.Id, sortedFlowcharts[i].ValueText);

                        definejTransition = definej.Item2;

                        string definejOldPage = definej.Item4;
                        string defindjNewPage = definej.Item5;


                        previousNode.previousPlaceModel = definej.Item1;
                        previousNode.Type = "place";

                        //Add to old page
                        pages.mainPageModel.Node += definejOldPage;

                        //Add to sub page
                        //countSubPage indicate current page you are in
                        countSubPage++;
                        CreatePageNodeByCountSubPage(countSubPage, pages, defindjNewPage);
                    }
                    else
                    {
                        //countSubPage = 0,1 to test create on current page

                        if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("i ++"))
                        {
                            //TODO: Send real process to code segment inscription
                            //TODO: Find solution to create arc 
                            Rule4 rule4 = new Rule4();
                            var rule4Result = rule4.ApplyRule(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode);

                            PlaceModel rule4Place = rule4Result.Item1;
                            TransitionModel rule4Transition = rule4Result.Item2;
                            string rule4String = rule4Result.Item4;

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
                    var rule5Result = rule5.ApplyRule(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode.previousPlaceModel);
                    
                    PlaceModel rule5Place = rule5Result.Item1;
                    TransitionModel rule5Transition = rule5Result.Item2;
                    string rule5String = rule5Result.Item4;

                    previousNode.previousPlaceModel = rule5Place;
                    previousNode.previousTransitionModel = rule5Transition;
                    previousNode.Type = "place";

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule5String);
                }
                //Rule 6 Decision
                else if (sortedFlowcharts[i].NodeType.ToLower() == "condition")
                {
                    var trueCondition = "[" + sortedFlowcharts[i].ValueText.Replace("N", " length arr") + "]";
                    var falseCondition = ConvertDecision(trueCondition);

                    //TODO: Replace Array with List.nth(arr,j)

                    //TODO: Separate between true and false case by arrow[i+1]
                    Rule6 rule6 = new Rule6();
                    var rule6Result = rule6.ApplyRule(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode,
                        trueCondition, falseCondition);

                    PlaceModel rule6Place = rule6Result.Item1;
                    TransitionModel rule6Transition = rule6Result.Item3;
                    string rule6String = rule6Result.Item5;

                    previousNode.previousPlaceModel = rule6Place;
                    previousNode.previousTransitionModel = rule6Transition; //True Condition
                    previousNode.Type = "transition";

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule6String);

                }
                //Rule 7 End
                else if (sortedFlowcharts[i].NodeType.ToLower() == "end")
                {
                    //TODO: Previous transition need to check (in subpage)
                    Rule7 rule7 = new Rule7();
                    var rule7Result = rule7.ApplyRule(allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode);

                    PlaceModel rule7Place = rule7Result.Item1;
                    string rule7String = rule7Result.Item2;
                    
                    previousNode.previousPlaceModel = rule7Place;

                    //Reset because it's need to end at main page
                    countSubPage = 0;
                    CreatePageNodeByCountSubPage(countSubPage, pages, rule7String);
                }

            }


            var page1 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate], 
                pages.mainPageModel);

            string page2 = (pages.subPageModel1.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                pages.subPageModel1) :
                string.Empty;

            string page3 = (pages.subPageModel2.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel2) :
                string.Empty;
            
            string page4 = (pages.subPageModel3.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel3) :
                string.Empty;
           
            string page5 = (pages.subPageModel4.Node != string.Empty) ?
                approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate],
                    pages.subPageModel4) :
                string.Empty;
            
            var allPage = page1 + page2 + page3 + page4 + page5;
            #endregion

            #region Instance
            //TODO: Define instance if have more than 2
            HierarchyInstanceModel inst = new HierarchyInstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "page=\"ID6\">",
                Closer = "{0}"
            };

            HierarchyInstanceModel inst2 = new HierarchyInstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "trans=\"" + definejTransition.Id1 + "\"",
                Closer = "/></instance>"
            };

            var instances = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst);
            var instances2 = approach.CreateHierarchyInstance(allTemplates[(int)TemplateEnum.InstanceTemplate], inst2);

            var allInstances = string.Format(instances, instances2);

            #endregion


            //Combine to put on Empty Color Set
            var allColorSet = col1;
            var allVar = var1 + var2;

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
        private static void CreatePageNodeByCountSubPage(int countSubPage, PageDeclare pages, string rule)
        {
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
        private string ConvertDecision(string condition)
        {
            if (condition.Contains("&gt;"))
            {
                return condition.Replace("&gt;", "&lt;=");
            }
            else if (condition.Contains("&lt;"))
            {
                return condition.Replace("&lt;", "&gt;=");
            }
            else if (condition.Contains("="))
            {
                return condition.Replace("=", "!=");
            }
            else if (condition.Contains("!="))
            {
                return condition.Replace("!=", "=");
            }
            else
            {
                return condition;
            }
        }
    }
}