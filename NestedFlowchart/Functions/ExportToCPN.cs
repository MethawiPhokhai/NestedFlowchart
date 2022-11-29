using NestedFlowchart.Declaration;
using NestedFlowchart.Models;
using NestedFlowchart.Rules;
using NestedFlowchart.Templates;

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
                    if (sortedFlowcharts[i].ValueText.Contains('[') || sortedFlowcharts[i].ValueText.ToLower().Contains("arr"))
                    {
                        var arrayValue = sortedFlowcharts[i].ValueText.Substring(sortedFlowcharts[i].ValueText.IndexOf('=') + 1).Trim();

                        rulePlace.Type = "INTs";
                        rulePlace.InitialMarking = arrayValue;
                    }

                    var rule2 = approach.Rule2(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], rulePlace);
                    string rule2string = rule2.Item4;

                    previousNode.previousPlaceModel = rule2.Item1;
                    previousNode.previousTransitionModel = rule2.Item2;
                    previousNode.Type = "place";


                    if (rule2string.Contains("Start"))
                    {
                        ruleString = string.Empty;
                    }

                    pages.mainPageModel.Node += ruleString + rule2string;
                }
                //Rule3 : I=0, J=1
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

                        var rule3 = approach.Rule3(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], string.Empty, string.Empty
                            , previousNode, false, string.Empty, sortedFlowcharts[i].ValueText);

                        previousNode.previousPlaceModel = rule3.Item1;
                        previousNode.Type = "place";


                        pages.mainPageModel.Node += rule3.Item4;
                    }
                    //Case Nested => Create Hierachy Tool
                    else if (sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("j =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("k =")
                        || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("l =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("m ="))
                    {


                        var definej = approach.Rule3(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], allTemplates[(int)TemplateEnum.SubStrTemplate], allTemplates[(int)TemplateEnum.PortTemplate],
                            previousNode, true, pages.subPageModel1.Id, sortedFlowcharts[i].ValueText);

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
                            var rule4 = approach.Rule4(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode);

                            previousNode.previousPlaceModel = rule4.Item1;
                            previousNode.previousTransitionModel = rule4.Item2;
                            previousNode.Type = "transition";

                            countSubPage = 0;
                            CreatePageNodeByCountSubPage(countSubPage, pages, rule4.Item4);
                        }

                        countSubPage = 1;
                    }
                }
                else if (sortedFlowcharts[i].NodeType.ToLower() == "connector")
                {
                    var rule5 = approach.Rule5(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode.previousPlaceModel);
                    previousNode.previousPlaceModel = rule5.Item1;
                    previousNode.previousTransitionModel = rule5.Item2;
                    previousNode.Type = "place";


                    CreatePageNodeByCountSubPage(countSubPage, pages, rule5.Item4);
                }
                else if (sortedFlowcharts[i].NodeType.ToLower() == "condition")
                {
                    var trueCondition = "[" + sortedFlowcharts[i].ValueText.Replace("N", " length arr") + "]";
                    var falseCondition = ConvertDecision(trueCondition);

                    //TODO: Replace Array with List.nth(arr,j)

                    //TODO: Separate between true and false case by arrow[i+1]
                    var rule6 = approach.Rule6(allTemplates[(int)TemplateEnum.TransitionTemplate], allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode,
                        trueCondition, falseCondition);

                    previousNode.previousPlaceModel = rule6.Item1;
                    previousNode.previousTransitionModel = rule6.Item3; //True Condition
                    previousNode.Type = "transition";

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule6.Item5);

                }
                else if (sortedFlowcharts[i].NodeType.ToLower() == "end")
                {
                    //TODO: Previous transition need to check (in subpage)
                    var rule7 = approach.Rule7(allTemplates[(int)TemplateEnum.PlaceTemplate], allTemplates[(int)TemplateEnum.ArcTemplate], previousNode);
                    previousNode.previousPlaceModel = rule7.Item1;

                    //Reset because it's need to end at main page
                    countSubPage = 0;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule7.Item2);
                }

            }


            var page1 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate], pages.mainPageModel);

            string page2 = string.Empty;
            if (pages.subPageModel1.Node != string.Empty)
            {
                page2 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate], pages.subPageModel1);
            }

            string page3 = string.Empty;
            if (pages.subPageModel2.Node != string.Empty)
            {
                page3 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate], pages.subPageModel2);
            }

            string page4 = string.Empty;
            if (pages.subPageModel3.Node != string.Empty)
            {
                page4 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate], pages.subPageModel3);
            }

            string page5 = string.Empty;
            if (pages.subPageModel4.Node != string.Empty)
            {
                page5 = approach.CreatePage(allTemplates[(int)TemplateEnum.PageTemplate], pages.subPageModel4);
            }

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

            string firstCPN = string.Format(allTemplates[(int)TemplateEnum.EmptyCPNTemplate], allColorSet + allVar, allPage, allInstances);

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