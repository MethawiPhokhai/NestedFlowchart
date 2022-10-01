using NestedFlowchart.Models;

namespace NestedFlowchart.Functions
{
    public class ExportToCPN
    {
        public void ExportFile(string? TemplatePath, string? ResultPath)
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


            var rule1 = approach.Rule1();
            var rule2 = approach.Rule2(transitionTemplate, placeTemplate, arcTemplate, rule1);
            var rule3 = approach.Rule3(transitionTemplate, placeTemplate, arcTemplate, string.Empty, rule2.Item1, rule2.Item2, rule2.Item3, false, page2Id);
            var rule5 = approach.Rule5(transitionTemplate, placeTemplate, arcTemplate, rule3.Item1, rule3.Item2, rule3.Item3);
            var rule6 = approach.Rule6(transitionTemplate, placeTemplate, arcTemplate, rule5.Item1, rule5.Item2, rule5.Item3);
            var rule7 = approach.Rule7(placeTemplate, arcTemplate, rule6.Item1, rule6.Item2, rule6.Item4);
            var definej = approach.Rule3(transitionTemplate, placeTemplate, arcTemplate, subStrTemplate, rule2.Item1, rule2.Item2, rule2.Item3, true, page2Id);

            var allNode = rule2.Item4 + rule3.Item4 + rule5.Item4 + rule6.Item5 + rule7.Item2 + definej.Item4;

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
            InstanceModel inst = new InstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "page=\"ID6\">",
                Closer = "{0}"
            };

            InstanceModel inst2 = new InstanceModel()
            {
                Id = IdManagements.GetlastestInstanceId(),
                Text = "page=\"" + p2.Id + "\"",
                Closer = "/></instance>"
            };

            var instances = approach.CreateHierarchy_Instance(instanceTemplate, inst);
            var instances2 = approach.CreateHierarchy_Instance(instanceTemplate, inst2);

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