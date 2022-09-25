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


            //TODO: 
            //1. Create funtion for Place, Transition, Arc with Parameter
            //2. Foreach one by one from Flowchart Node to apply rule


            var rule1 = Rule1(placeTemplate);
            var rule2 = Rule2(transitionTemplate, placeTemplate, rule1);



            //var Arc = File.ReadAllText(TemplatePath + "Arc.txt");
            //Arc = string.Format(Arc, "ID1412948792", "ID1412948772");

            //var PlaceAndTran = place + Transition + Arc;
            string firstCPN = string.Format(emptyCPNTemplate, rule2);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
        }

        #region Create CPN Element
        private string CreatePlace(string placeTemplate, PlaceModel model)
        {
            //Replace place parameter with string.format
            placeTemplate = string.Format(placeTemplate, model.Id1, model.Id2, model.Id3, 
                model.Name, model.Type, model.InitialMarking);

            return placeTemplate;
        }

        private string CreateTransition(string transitionTemplate, TransitionModel model)
        {
            //Replace transition parameter with string.format
            string transition = string.Format(transitionTemplate, model.Name, model.Condition);

            return transition;
        }

        #endregion

        //Rule 1 : Transform start to place start
        private PlaceModel Rule1(string placeTemplate)
        {
            PlaceModel pl = new PlaceModel();
            pl.Id1 = "ID1412948772";
            pl.Id2 = "ID1412948773";
            pl.Id3 = "ID1412948774";
            pl.Name = "Start";
            pl.Type = "UNIT";
            pl.InitialMarking = string.Empty;

            return pl;
        }

        //Rule 2 : Transform initialize process to transition and place, and assign initial marking
        private string Rule2(string transitionTemplate, string placeTemplate, PlaceModel rule1)
        {
            TransitionModel tr = new TransitionModel()
            {
                Name = "T1"
            };

            PlaceModel pl = new PlaceModel()
            {
                Id1 = "ID1412948775",
                Id2 = "ID1412948776",
                Id3 = "ID1412948777",
                Name = "P1"
            };

            //Define Type INTs
            //rule1.InitialMarking = "[7,8,0,3,5]";


            var place1 = CreatePlace(placeTemplate, rule1);
            var transition = CreateTransition(transitionTemplate, tr);
            var place2 = CreatePlace(placeTemplate, pl);

            return place1 + transition + place2;


        }
    }
}