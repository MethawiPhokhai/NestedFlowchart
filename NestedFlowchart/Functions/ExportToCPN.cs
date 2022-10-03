using NestedFlowchart.Declaration;
using NestedFlowchart.Models;

namespace NestedFlowchart.Functions
{
    public class ExportToCPN
    {
        public void ExportFile(string? TemplatePath, string? ResultPath, List<XMLCellNode> sortedFlowcharts)
        {
            //Create CPN File
            string emptyCPNTemplate = File.ReadAllText(TemplatePath + "EmptyNet.txt");

            //Read place template
            var placeTemplate = File.ReadAllText(TemplatePath + "Place.txt");

            //Read transition template
            var transitionTemplate = File.ReadAllText(TemplatePath + "Transition.txt");

            //Read arc template
            var arcTemplate = File.ReadAllText(TemplatePath + "Arc.txt");

            //Read var declaration Type
            var varTemplate = File.ReadAllText(TemplatePath + "VarType.txt");
            
            //Read colorSet declaration Type
            var colorSetTemplate = File.ReadAllText(TemplatePath + "ColorSet.txt");
            
            //Read instance template
            var instanceTemplate = File.ReadAllText(TemplatePath + "Hierarchy_Instance.txt");
            
            //Read page template
            var pageTemplate = File.ReadAllText(TemplatePath + "Page.txt");

            //Read substr template
            var subStrTemplate = File.ReadAllText(TemplatePath + "Hierarchy_SubPageTransition.txt");

            //Read port template
            var portTemplate = File.ReadAllText(TemplatePath + "Hierarchy_Port.txt");

            TransformationApproach approach = new TransformationApproach();

            #region Color Set
            ColorSetModel colorSetProduct1 = new ColorSetModel()
            {
                Id =  IdManagements.GetlastestColorSetId(),
                Name = "loopi",
                Type = new List<string>()
                {
                    "INT",
                    "INTs"
                },
                Text = "colset loopi = product INT*INTs;"
            };

            var col1 = approach.CreateColorSet(colorSetTemplate, colorSetProduct1);

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

            var var1 = approach.CreateVar(varTemplate, var1Model);
            var var2 = approach.CreateVar(varTemplate, var2Model);

            #endregion

            #region Page
            int countSubPage = 0;
            PageDeclare pages = new PageDeclare();

            //Declaration
            string rule1string = string.Empty;
            string rule2string = string.Empty;

            PlaceModel previousPlaceModel = new PlaceModel();
            PlaceModel rule1place = new PlaceModel();

            PlaceModel rule2place = new PlaceModel();
            TransitionModel rule2transition = new TransitionModel();
            ArcModel rule2ArcModel = new ArcModel();

            PlaceModel rule3place = new PlaceModel();

            PlaceModel rule5place = new PlaceModel();
            TransitionModel rule5transition = new TransitionModel();
            ArcModel rule5ArcModel = new ArcModel();

            PlaceModel rule6place = new PlaceModel();
            TransitionModel rule6transition = new TransitionModel();
            ArcModel rule6ArcModel = new ArcModel();

            PlaceModel rule7Place = new PlaceModel();


            TransitionModel definejTransition = new TransitionModel();
            string definejOldPage = string.Empty;
            string defindjNewPage = string.Empty;

            for (int i = 0; i < sortedFlowcharts.Count; i++)
            {
                //Rule1 : Start
                if (sortedFlowcharts[i].NodeType.ToLower() == "start")
                {
                    var rule1 = approach.Rule1(placeTemplate);
                    rule1place = rule1.Item1;
                    previousPlaceModel = rule1place;
                    rule1string = rule1.Item2.ToString();
                }
                //Rule2 : Initialize Process
                else if(sortedFlowcharts[i].NodeType.ToLower() == "process" 
                    && sortedFlowcharts[i-2].NodeType.ToLower() == "start")
                {
                    //Initial Marking
                    if(sortedFlowcharts[i].ValueText.Contains('[') || sortedFlowcharts[i].ValueText.ToLower().Contains("arr"))
                    {
                        var arrayValue = sortedFlowcharts[i].ValueText.Substring(sortedFlowcharts[i].ValueText.IndexOf('=') + 1).Trim();
                            
                        rule1place.Type = "INTs";
                        rule1place.InitialMarking = arrayValue;
                    }

                    var rule2 = approach.Rule2(transitionTemplate, placeTemplate, arcTemplate, rule1place);
                    rule2place = rule2.Item1;
                    rule2transition = rule2.Item2;
                    rule2ArcModel = rule2.Item3;
                    rule2string = rule2.Item4;
                    previousPlaceModel = rule2place;

                    if (rule2string.Contains("Start"))
                    {
                        rule1string = string.Empty;
                    }

                    pages.mainPageModel.Node += rule1string + rule2string;
                }
                //Rule3 : I=0, J=1
                else if(sortedFlowcharts[i].NodeType.ToLower() == "process")
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

                        //var var2 = approach.CreateVar(varTemplate, var2Model);

                        var rule3 = approach.Rule3(transitionTemplate, placeTemplate, arcTemplate, string.Empty, string.Empty
                            , rule2place, rule2transition, rule2ArcModel, false, string.Empty, sortedFlowcharts[i].ValueText);

                        rule3place = rule3.Item1;
                        previousPlaceModel = rule3place;
                        pages.mainPageModel.Node += rule3.Item4;
                    }
                    //Case Nested => Create Hierachy Tool
                    else if(sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("j =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("k =")
                        || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("l =") || sortedFlowcharts[i].ValueText.ToLower().Trim().Contains("m ="))
                    {

                        //TODO: if node before is place create transition



                        var definej = approach.Rule3(transitionTemplate, placeTemplate, arcTemplate, subStrTemplate, portTemplate, rule2place, rule2transition, rule2ArcModel, true, pages.subPageModel1.Id, sortedFlowcharts[i].ValueText);
                        definejTransition = definej.Item2;
                        definejOldPage = definej.Item4;
                        defindjNewPage = definej.Item5;
                        previousPlaceModel = definej.Item1;

                        //Add to old page
                        pages.mainPageModel.Node += definejOldPage;

                        //Add to sub page
                        //countSubPage indicate current page you are in
                        countSubPage++;
                        CreatePageNodeByCountSubPage(countSubPage, pages, defindjNewPage);
                    }
                    else
                    {
                        //TODO: Create Rule4
                        //Rule 4 here
                    }

                }
                else if (sortedFlowcharts[i].NodeType.ToLower() == "connector")
                {
                    var rule5 = approach.Rule5(transitionTemplate, placeTemplate, arcTemplate, previousPlaceModel);
                    rule5place = rule5.Item1;
                    rule5transition = rule5.Item2;
                    rule5ArcModel = rule5.Item3;
                    previousPlaceModel = rule5place;
                    CreatePageNodeByCountSubPage(countSubPage, pages, rule5.Item4);
                }
                else if(sortedFlowcharts[i].NodeType.ToLower() == "condition")
                {
                    var trueCondition = "[" + sortedFlowcharts[i].ValueText.Replace("N", " length arr") + "]";
                    var falseCondition = ConvertDecision(trueCondition);

                    //TODO: Replace Array with List.nth(arr,j)

                    var rule6 = approach.Rule6(transitionTemplate, placeTemplate, arcTemplate, previousPlaceModel, rule5transition, rule5ArcModel,
                        trueCondition, falseCondition);
                    rule6place = rule6.Item1;
                    rule6transition = rule6.Item2;
                    rule6ArcModel = rule6.Item4;
                    previousPlaceModel = rule6place;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule6.Item5);

                }
                else if (sortedFlowcharts[i].NodeType.ToLower() == "end")
                {
                    //TODO: find solution to create Arc
                    var rule7 = approach.Rule7(placeTemplate, arcTemplate, rule6place, rule6transition, rule6ArcModel);
                    rule7Place = rule7.Item1;

                    //Reset because it's need to end at main page
                    countSubPage = 0;

                    CreatePageNodeByCountSubPage(countSubPage, pages, rule7.Item2);
                }

            }


            var page1 = approach.CreatePage(pageTemplate, pages.mainPageModel);

            string page2 = string.Empty;
            if(pages.subPageModel1.Node != string.Empty)
            {
                page2 = approach.CreatePage(pageTemplate, pages.subPageModel1);
            }

            string page3 = string.Empty;
            if (pages.subPageModel2.Node != string.Empty)
            {
                page3 = approach.CreatePage(pageTemplate, pages.subPageModel2);
            }

            string page4 = string.Empty;
            if (pages.subPageModel3.Node != string.Empty)
            {
                page4 = approach.CreatePage(pageTemplate, pages.subPageModel3);
            }

            string page5 = string.Empty;
            if (pages.subPageModel4.Node != string.Empty)
            {
                page5 = approach.CreatePage(pageTemplate, pages.subPageModel4);
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

            var instances = approach.CreateHierarchyInstance(instanceTemplate, inst);
            var instances2 = approach.CreateHierarchyInstance(instanceTemplate, inst2);

            var allInstances = string.Format(instances, instances2);
            
            #endregion


            //Combine to put on Empty Color Set
            var allColorSet = col1;
            var allVar = var1 + var2;

            string firstCPN = string.Format(emptyCPNTemplate, allColorSet + allVar, allPage, allInstances);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
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