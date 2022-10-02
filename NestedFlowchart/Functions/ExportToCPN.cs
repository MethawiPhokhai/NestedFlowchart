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

            var page2Id = IdManagements.GetlastestPageId();

            //Declaration
            string rule1string = string.Empty;
            string rule2string = string.Empty;
            string rule3string = string.Empty;
            string rule5string = string.Empty;

            PlaceModel rule1place = new PlaceModel();

            PlaceModel rule2place = new PlaceModel();
            TransitionModel rule2transition = new TransitionModel();
            ArcModel rule2ArcModel = new ArcModel();

            PlaceModel rule3place = new PlaceModel();
            TransitionModel rule3transition = new TransitionModel();
            ArcModel rule3ArcModel = new ArcModel();

            PlaceModel rule5place = new PlaceModel();
            TransitionModel rule5transition = new TransitionModel();
            ArcModel rule5ArcModel = new ArcModel();



            for (int i = 0; i < sortedFlowcharts.Count; i++)
            {
                //Rule1 : Start
                if (sortedFlowcharts[i].NodeType.ToLower() == "start")
                {
                    var rule1 = approach.Rule1(placeTemplate);
                    rule1place = rule1.Item1;
                    rule1string = rule1.Item2.ToString();
                }
                //Rule2 : Initialize Process
                else if(sortedFlowcharts[i].NodeType.ToLower() == "process" 
                    && sortedFlowcharts[i-2].NodeType.ToLower() == "start")
                {
                    //TODO: substring and assign Initial Marking
                    //rule1place.InitialMarking = sortedFlowcharts[i].ValueText

                    //Define type and Initial marking value
                    rule1place.Type = "INTs";
                    rule1place.InitialMarking = "[7,8,0,3,5]";

                    var rule2 = approach.Rule2(transitionTemplate, placeTemplate, arcTemplate, rule1place);
                    rule2place = rule2.Item1;
                    rule2transition = rule2.Item2;
                    rule2ArcModel = rule2.Item3;
                    rule2string = rule2.Item4;

                    if (rule2string.Contains("Start"))
                    {
                        rule1string = string.Empty;
                    }
                }
                //Rule3 : I= 0
                else if(sortedFlowcharts[i].NodeType.ToLower() == "process" 
                    && sortedFlowcharts[i-2].NodeType.ToLower() == "process"
                    && sortedFlowcharts[i-4].NodeType.ToLower() == "start")
                {
                    var rule3 = approach.Rule3(transitionTemplate, placeTemplate, arcTemplate, string.Empty, string.Empty, rule2place, rule2transition, rule2ArcModel, false, page2Id);
                    rule3place = rule3.Item1;
                    rule3transition = rule3.Item2;
                    rule3ArcModel = rule3.Item3;
                    rule3string = rule3.Item4;
                }
                //Rule 5 : Connector
                else if (sortedFlowcharts[i].NodeType.ToLower() == "connector")
                {
                    var rule5 = approach.Rule5(transitionTemplate, placeTemplate, arcTemplate, rule3place, rule3transition, rule3ArcModel);
                    rule5place = rule5.Item1;
                    rule5transition = rule5.Item2;
                    rule5ArcModel = rule5.Item3;
                    rule5string = rule5.Item4;
                }
            }
            
            
            
            
            var rule6 = approach.Rule6(transitionTemplate, placeTemplate, arcTemplate, rule5place, rule5transition, rule5ArcModel);
            var rule7 = approach.Rule7(placeTemplate, arcTemplate, rule6.Item1, rule6.Item2, rule6.Item4);
            var definej = approach.Rule3(transitionTemplate, placeTemplate, arcTemplate, subStrTemplate, portTemplate, rule2place, rule2transition, rule2ArcModel, true, page2Id);

            var allNode = $"{rule1string}{rule2string}{rule3string}{rule5string}{rule6.Item5}{rule7.Item2}{definej.Item4}";

            //Page1
            var p1 = new PageModel()
            {
                Id = "ID6",
                Name = "New Page",
                Node = allNode
            };

            //New Subpage Page
            var p2 = new PageModel()
            {
                Id = page2Id,
                Name = "New Subpage",
                Node = definej.Item5
            };

            var page1 = approach.CreatePage(pageTemplate, p1);
            var page2 = approach.CreatePage(pageTemplate, p2);

            var allPage = page1 + page2;
            #endregion

            #region Instance
            HierarchyInstanceModel inst = new HierarchyInstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "page=\"ID6\">",
                Closer = "{0}"
            };

            HierarchyInstanceModel inst2 = new HierarchyInstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "trans=\"" + definej.Item2.Id1 + "\"",
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
    }
}