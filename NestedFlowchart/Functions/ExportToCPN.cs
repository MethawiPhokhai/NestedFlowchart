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

            //TODO: 
            //1. Create funtion for Place, Transition, Arc with Parameter
            //2. Foreach one by one from Flowchart Node to apply rule

            ColorSetModel colorSetProduct1 = new ColorSetModel()
            {
                Id = "ID1412426768",
                Name = "loopi",
                Type = new List<string>()
                {
                    "INT",
                    "INTs"
                },
                Text = "colset loopi = product INT*INTs;"
            };


            VarModel var1Model = new VarModel()
            {
                Id = "ID1412422143",
                Type = "INTs",
                Name = "arr",
                Layout = "var arr: INTs;"
            };

            VarModel var2Model = new VarModel()
            {
                Id = "ID1412422144",
                Type = "INT",
                Name = "i,i2,j,j2",
                Layout = "var i,i2,j,j2: INT;"
            };

            TransformationApproach approach = new TransformationApproach();

            var col1 = approach.CreateColorSet(colorSetTemplate, colorSetProduct1);

            var var1 = approach.CreateVar(varTemplate, var1Model);
            var var2 = approach.CreateVar(varTemplate, var2Model);

            var rule1 = approach.Rule1();
            var rule2 = approach.Rule2(transitionTemplate, placeTemplate, arcTemplate, rule1);
            var rule3 = approach.Rule3(transitionTemplate, placeTemplate, arcTemplate, rule2.Item1, rule2.Item2, rule2.Item3);
            var rule4 = approach.Rule5(transitionTemplate, placeTemplate, arcTemplate, rule3.Item1, rule3.Item2, rule3.Item3);
            var rule5 = approach.Rule6(transitionTemplate, placeTemplate, arcTemplate, rule4.Item1, rule4.Item2, rule4.Item3);

            var allColorSet = col1;
            var allVar = var1 + var2;
            var allNode = rule2.Item4 + rule3.Item4 + rule4.Item4 + rule5.Item4;
            string firstCPN = string.Format(emptyCPNTemplate, allColorSet + allVar, allNode);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
        }
    }
}