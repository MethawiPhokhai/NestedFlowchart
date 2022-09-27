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

            //TODO: 
            //1. Create funtion for Place, Transition, Arc with Parameter
            //2. Foreach one by one from Flowchart Node to apply rule
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

            var var1 = CreateVar(varTemplate, var1Model);
            var var2 = CreateVar(varTemplate, var2Model);

            var rule1 = Rule1();
            var rule2 = Rule2(transitionTemplate, placeTemplate, arcTemplate, rule1);
            var rule3 = Rule3(transitionTemplate, placeTemplate, rule2.Item1, rule2.Item2);


            var allVar = var1 + var2; 
            var allNode = rule2.Item3 + rule3;
            string firstCPN = string.Format(emptyCPNTemplate, allVar, allNode);

            //Write to CPN File
            File.WriteAllText(ResultPath + "Result.cpn", firstCPN);
        }

        #region Create CPN Element

        private string CreateVar(string varTemplate, VarModel model)
        {
            string newName = string.Empty;
            var name = model.Name.Split(',');
            foreach(var item in name)
            {
                newName += "<id>" + item + "</id>\n";
            }
            return string.Format(varTemplate, model.Id, model.Type, newName, model.Layout);
        }

        private string CreatePlace(string placeTemplate, PlaceModel model)
        {
            placeTemplate = string.Format(placeTemplate, model.Id1, model.Id2, model.Id3, 
                model.Name, model.Type, model.InitialMarking, 
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3);

            return placeTemplate;
        }

        private string CreateTransition(string transitionTemplate, TransitionModel model)
        {
            string transition = string.Format(transitionTemplate,model.Id1, model.Id2, model.Id3, model.Id4, model.Id5,
                model.Name, model.Condition,
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3,
                model.xPos4, model.yPos4,
                model.xPos5, model.yPos5,
                model.CodeSegment);

            return transition;
        }

        private string CreateArc(string arcTemplate, ArcModel model)
        {
            string arc = string.Format(arcTemplate,model.Id1, model.Id2, 
                model.TransEnd, model.PlaceEnd, 
                model.Orientation, model.Type);

            return arc;
        }

        #endregion

        //Rule 1 : Transform start to place start
        private PlaceModel Rule1()
        {
            PlaceModel pl = new PlaceModel()
            {
                Id1 = "ID1412948772",
                Id2 = "ID1412948773",
                Id3 = "ID1412948774",
                Name = "Start",
                Type = "UNIT",
                InitialMarking = string.Empty,

                xPos1 = -15.000000,
                yPos1 = 126.000000,

                xPos2 = 24.000000,
                yPos2 = 102.000000,

                xPos3 = 41.000000,
                yPos3 = 149.000000

            };

            return pl;
        }

        //Rule 2 : Transform initialize process to transition and place, and assign initial marking
        private (PlaceModel, TransitionModel, string) Rule2(string transitionTemplate, string placeTemplate,string arcTemplate, PlaceModel placeRule1)
        {
            TransitionModel tr = new TransitionModel()
            {
                Id1 = "ID1412948792",
                Id2 = "ID1412948793",
                Id3 = "ID1412948794",
                Id4 = "ID1412948795",
                Id5 = "ID1412948796",

                Name = "T1",

                xPos1 = -15.000000,
                yPos1 = 42.000000,

                xPos2 = -54.000000,
                yPos2 = 73.000000,

                xPos3 = 29.000000,
                yPos3 = 73.000000,

                xPos4 = 49.000000,
                yPos4 = -10.000000,

                xPos5 = -83.000000,
                yPos5 = 11.000000
            };

            PlaceModel pl = new PlaceModel()
            {
                Id1 = "ID1412948775",
                Id2 = "ID1412948776",
                Id3 = "ID1412948777",
                Name = "P1",

                xPos1 = placeRule1.xPos1 - 4,
                yPos1 = placeRule1.yPos1 - 168,

                xPos2 = placeRule1.xPos2 - 4,
                yPos2 = placeRule1.yPos2 - 167,

                xPos3 = placeRule1.xPos3 - 4,
                yPos3 = placeRule1.yPos3 - 167
            };

            ArcModel a1 = new ArcModel()
            {
                Id1 = "ID1412948812",
                Id2 = "ID1412948813",

                TransEnd = tr.Id1,
                PlaceEnd = placeRule1.Id1,

                Orientation = "PtoT", //Place to Transition
                Type = "arr"
            };

            ArcModel a2 = new ArcModel()
            {
                Id1 = "ID1412948814",
                Id2 = "ID1412948815",

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                Orientation = "TtoP", //Transition to Place
                Type = string.Empty
            };


            //Define type and Initial marking value
            placeRule1.Type = "INTs";
            placeRule1.InitialMarking = "[7,8,0,3,5]";


            var place1 = CreatePlace(placeTemplate, placeRule1);
            var arc1 = CreateArc(arcTemplate, a1);
            var transition = CreateTransition(transitionTemplate, tr);
            var arc2 = CreateArc(arcTemplate, a2);
            var place2 = CreatePlace(placeTemplate, pl);

            var allNode = place1 + place2 + transition + arc1 + arc2;
            return (pl, tr, allNode);

        }

        //Rule 3 : Transform Nested Structure into Hierarchical
        private string Rule3(string transitionTemplate, string placeTemplate, PlaceModel placeRule2, TransitionModel tranRule2)
        {
            //Define i=1 in Code Segment Inscription
            var codeSeg = "input (); \n " +
                "output(i); \n " +
                "action \n " +
                "let \n" +
                "val i = 1 \n" +
                "in \n" +
                "(i) \n" +
                "end";


            TransitionModel tr = new TransitionModel()
            {
                Id1 = "ID1412948797",
                Id2 = "ID1412948798",
                Id3 = "ID1412948799",
                Id4 = "ID1412948800",
                Id5 = "ID1412948801",

                Name = "T2",

                xPos1 = tranRule2.xPos1 - 9,
                yPos1 = tranRule2.yPos1 - 168,

                xPos2 = tranRule2.xPos2 - 9,
                yPos2 = tranRule2.yPos2 - 168,

                xPos3 = tranRule2.xPos3 - 9,
                yPos3 = tranRule2.yPos3 - 168,

                xPos4 = tranRule2.xPos4 - 9,
                yPos4 = tranRule2.yPos4 - 168,

                xPos5 = tranRule2.xPos5 - 9,
                yPos5 = tranRule2.yPos5 - 168,

                CodeSegment = codeSeg
            };

            PlaceModel pl = new PlaceModel()
            {
                Id1 = "ID1412948778",
                Id2 = "ID1412948779",
                Id3 = "ID1412948780",
                Name = "P2",

                xPos1 = placeRule2.xPos1 - 4,
                yPos1 = placeRule2.yPos1 - 168,

                xPos2 = placeRule2.xPos2 - 4,
                yPos2 = placeRule2.yPos2 - 167,

                xPos3 = placeRule2.xPos3 - 4,
                yPos3 = placeRule2.yPos3 - 167
            };

            var place1 = CreatePlace(placeTemplate, pl);
            var transition = CreateTransition(transitionTemplate, tr);

            return place1 + transition;

        }
    }
}