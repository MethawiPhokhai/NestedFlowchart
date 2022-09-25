namespace NestedFlowchart.Functions
{
    public class ExportToCPN
    {
        public void ExportFile(string? TemplatePath, string? ResultPath)
        {
            //Create CPN File
            string emptyCPNTemplate = File.ReadAllText(TemplatePath + "EmptyNet.txt");

            //TODO: 
            //1. Create funtion for Place, Transition, Arc with Parameter
            //2. Foreach one by one from Flowchart Node to apply rule


            //Insert each CPN Node into empty template
            //Start

            var Place = File.ReadAllText(TemplatePath + "Place.txt");
            var Transition = File.ReadAllText(TemplatePath + "Transition.txt");
            var Arc = File.ReadAllText(TemplatePath + "Arc.txt");
            Arc = string.Format(Arc, "ID1412948792", "ID1412948772");

            var PlaceAndTran = Place + Transition + Arc;
            string firstCPN = string.Format(emptyCPNTemplate, PlaceAndTran);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
        }
    }
}