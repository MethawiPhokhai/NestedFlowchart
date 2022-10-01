using NestedFlowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Functions
{
    public class TransformationApproach
    {
        #region Create CPN Element

        public string CreateColorSet(string colorTemplate, ColorSetModel model)
        {
            string newName = string.Empty;
            foreach(var item in model.Type)
            {
                newName += "<id>" + item + "</id>\n";
            }

            return string.Format(colorTemplate, model.Id, model.Name, newName, model.Text);
        }

        public string CreateVar(string varTemplate, VarModel model)
        {
            string newName = string.Empty;
            var name = model.Name.Split(',');
            foreach(var item in name)
            {
                newName += "<id>" + item + "</id>\n";
            }
            return string.Format(varTemplate, model.Id, model.Type, newName, model.Layout);
        }

        public string CreatePlace(string placeTemplate, PlaceModel model)
        {
            placeTemplate = string.Format(placeTemplate, model.Id1, model.Id2, model.Id3, 
                model.Name, model.Type, model.InitialMarking, 
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3);

            return placeTemplate;
        }

        public string CreateTransition(string transitionTemplate, TransitionModel model)
        {
            string transition = string.Format(transitionTemplate,model.Id1, model.Id2, model.Id3, model.Id4, model.Id5,
                model.Name, model.Condition,
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3,
                model.xPos4, model.yPos4,
                model.xPos5, model.yPos5,
                model.CodeSegment,
                model.SubsitutetionTransition);

            return transition;
        }

        public string CreateArc(string arcTemplate, ArcModel model)
        {
            string arc = string.Format(arcTemplate,model.Id1, model.Id2, 
                model.TransEnd, model.PlaceEnd, 
                model.Orientation, model.Type,
                model.xPos, model.yPos);

            return arc;
        }

        public string CreateHierarchy_Instance(string instanceTemplate, InstanceModel model)
        {
            return string.Format(instanceTemplate, model.Id, model.Text, model.Closer);
        }

        public string CreatePage(string pageTemplate, PageModel model)
        {
            return string.Format(pageTemplate, model.Id, model.Name, model.Node);
        }

        #endregion

        private string ConvertDecision(string sign)
        {
            return sign switch
            {
                "&gt;" => "&lt;=",
                "&lt;" => "&gt;=",
                "=" => "!=",
                "!=" => "=",
                _ => string.Empty
            };
        }



        //Rule 1 : Transform start to place start
        public PlaceModel Rule1()
        {
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
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
        public (PlaceModel, TransitionModel, ArcModel, string) Rule2(string transitionTemplate, string placeTemplate, string arcTemplate, PlaceModel placeRule1)
        {
            TransitionModel tr = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

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
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "P1",

                xPos1 = placeRule1.xPos1 - 4,
                yPos1 = placeRule1.yPos1 - 168,

                xPos2 = placeRule1.xPos2 - 4,
                yPos2 = placeRule1.yPos2 - 167,

                xPos3 = placeRule1.xPos3 - 4,
                yPos3 = placeRule1.yPos3 - 167,

                Type = "INTs"
            };

            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = placeRule1.Id1,

                xPos = 3,
                yPos = 84,

                Orientation = "PtoT", //Place to Transition
                Type = "arr"
            };

            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = a1.xPos - 4,
                yPos = a1.yPos - 82,

                Orientation = "TtoP", //Transition to Place
                Type = "arr"
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
            return (pl, tr, a2, allNode);

        }

        //Rule 3 : Transform Nested Structure into Hierarchical
        public (PlaceModel, TransitionModel, ArcModel, string) Rule3(string transitionTemplate, string placeTemplate, string arcTemplate,
            PlaceModel placeRule2, TransitionModel tranRule2, ArcModel arcRule2, bool isHierarchy)
        {
            if (!isHierarchy)
            {
                //T2 Code Segment Inscription
                //Define i=1 in Code Segment Inscription
                var codeSeg = "input (); \n " +
                    "output(i); \n " +
                    "action \n " +
                    "let \n" +
                    "val i = 1 \n" +
                    "in \n" +
                    "(i) \n" +
                    "end";

                //T2 Transition
                TransitionModel tr = new TransitionModel()
                {
                    Id1 = IdManagements.GetlastestTransitionId(),
                    Id2 = IdManagements.GetlastestTransitionId(),
                    Id3 = IdManagements.GetlastestTransitionId(),
                    Id4 = IdManagements.GetlastestTransitionId(),
                    Id5 = IdManagements.GetlastestTransitionId(),

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

                //P2 Place
                PlaceModel pl = new PlaceModel()
                {
                    Id1 = IdManagements.GetlastestPlaceId(),
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),
                    Name = "P2",

                    xPos1 = placeRule2.xPos1 - 4,
                    yPos1 = placeRule2.yPos1 - 168,

                    xPos2 = placeRule2.xPos2 - 4,
                    yPos2 = placeRule2.yPos2 - 167,

                    xPos3 = placeRule2.xPos3 - 4,
                    yPos3 = placeRule2.yPos3 - 167,

                    Type = "loopi"
                };

                //Arc from P1 to T2
                ArcModel a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr.Id1,
                    PlaceEnd = placeRule2.Id1,

                    xPos = arcRule2.xPos - 4,
                    yPos = arcRule2.yPos - 82,

                    Orientation = "PtoT", //Place to Transition
                    Type = "arr"
                };

                //Arc from T2 to P2
                ArcModel a2 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr.Id1,
                    PlaceEnd = pl.Id1,

                    xPos = a1.xPos - 4,
                    yPos = a1.yPos - 82,

                    Orientation = "TtoP", //Transition to Place
                    Type = "(i,arr)"
                };

                var place1 = CreatePlace(placeTemplate, pl);
                var arc1 = CreateArc(arcTemplate, a1);
                var transition = CreateTransition(transitionTemplate, tr);
                var arc2 = CreateArc(arcTemplate, a2);

                return (pl, tr, a2, (place1 + transition + arc1 + arc2));
            }
            else
            {
                //P3 Place (Input place)
                PlaceModel p3 = new PlaceModel()
                {
                    Id1 = IdManagements.GetlastestPlaceId(),
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),
                    Name = "P3",

                    xPos1 = placeRule2.xPos1 - 4,
                    yPos1 = placeRule2.yPos1 - 168,

                    xPos2 = placeRule2.xPos2 - 4,
                    yPos2 = placeRule2.yPos2 - 167,

                    xPos3 = placeRule2.xPos3 - 4,
                    yPos3 = placeRule2.yPos3 - 167,

                    Type = "loopi"
                };

                //New Subpage Transition
                TransitionModel tr_subpage = new TransitionModel()
                {
                    Id1 = IdManagements.GetlastestTransitionId(),
                    Id2 = IdManagements.GetlastestTransitionId(),
                    Id3 = IdManagements.GetlastestTransitionId(),
                    Id4 = IdManagements.GetlastestTransitionId(),
                    Id5 = IdManagements.GetlastestTransitionId(),

                    Name = "New Subpage",

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

                };



                var place3 = CreatePlace(placeTemplate, p3);
                var tr_subpage1 = CreateTransition(transitionTemplate, tr_subpage);

                return (null, null, null, place3 + tr_subpage1);
            }
            

        }

        //Rule 4 Simple Process


        //Rule 5 : Connector
        public (PlaceModel, TransitionModel, ArcModel, string) Rule5(string transitionTemplate, string placeTemplate, string arcTemplate,
            PlaceModel placeRule3, TransitionModel tranRule3, ArcModel arcRule3)
        {
            //T3 Transition
            TransitionModel tr = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = "T3",

                xPos1 = tranRule3.xPos1 - 9,
                yPos1 = tranRule3.yPos1 - 168,

                xPos2 = tranRule3.xPos2 - 9,
                yPos2 = tranRule3.yPos2 - 168,

                xPos3 = tranRule3.xPos3 - 9,
                yPos3 = tranRule3.yPos3 - 168,

                xPos4 = tranRule3.xPos4 - 9,
                yPos4 = tranRule3.yPos4 - 168,

                xPos5 = tranRule3.xPos5 - 9,
                yPos5 = tranRule3.yPos5 - 168,

            };

            //CN1 Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "CN1",

                xPos1 = placeRule3.xPos1 - 4,
                yPos1 = placeRule3.yPos1 - 168,

                xPos2 = placeRule3.xPos2 - 4,
                yPos2 = placeRule3.yPos2 - 167,

                xPos3 = placeRule3.xPos3 - 4,
                yPos3 = placeRule3.yPos3 - 167,

                Type = "loopi"
            };

            //Arc from P2 to T3
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = placeRule3.Id1,

                xPos = arcRule3.xPos - 4,
                yPos = arcRule3.yPos - 82,

                Orientation = "PtoT", //Place to Transition
                Type = "(i,arr)"
            };

            //Arc from T3 to CN1
            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = a1.xPos - 4,
                yPos = a1.yPos - 82,

                Orientation = "TtoP", //Transition to Place
                Type = "(i,arr)"
            };


            var place1 = CreatePlace(placeTemplate, pl);
            var arc1 = CreateArc(arcTemplate, a1);
            var transition = CreateTransition(transitionTemplate, tr);
            var arc2 = CreateArc(arcTemplate, a2);

            return (pl, tr, a2, (place1 + transition + arc1 + arc2));
        }

        //Rule 6 : Decision
        public (PlaceModel, TransitionModel, TransitionModel, ArcModel, string) Rule6(string transitionTemplate, string placeTemplate, string arcTemplate,
            PlaceModel placeRule4, TransitionModel tranRule4, ArcModel arcRule4)
        {

            //GF1 Transition
            TransitionModel tr1 = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = "GF1",

                xPos1 = tranRule4.xPos1 - 39,
                yPos1 = tranRule4.yPos1 - 168,

                xPos2 = tranRule4.xPos2 - 59,
                yPos2 = tranRule4.yPos2 - 168,

                xPos3 = tranRule4.xPos3 - 39,
                yPos3 = tranRule4.yPos3 - 168,

                xPos4 = tranRule4.xPos4 - 39,
                yPos4 = tranRule4.yPos4 - 168,

                xPos5 = tranRule4.xPos5 - 39,
                yPos5 = tranRule4.yPos5 - 168,

                Condition = "[i " + ConvertDecision("&lt;")  + " length arr]"

            };

            //GT1 Transition
            TransitionModel tr2 = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = "GT1",

                xPos1 = tranRule4.xPos1 + 39,
                yPos1 = tranRule4.yPos1 - 168,

                xPos2 = tranRule4.xPos2 + 139,
                yPos2 = tranRule4.yPos2 - 168,

                xPos3 = tranRule4.xPos3 + 39,
                yPos3 = tranRule4.yPos3 - 168,

                xPos4 = tranRule4.xPos4 + 39,
                yPos4 = tranRule4.yPos4 - 168,

                xPos5 = tranRule4.xPos5 + 39,
                yPos5 = tranRule4.yPos5 - 168,

                Condition = "[i &lt; length arr]"

            };

            //Arc from CN1 to GF1
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr1.Id1,
                PlaceEnd = placeRule4.Id1,

                xPos = arcRule4.xPos - 34,
                yPos = arcRule4.yPos - 82,

                Orientation = "PtoT", //Place to Transition
                Type = "(i,arr)"
            };

            //Arc from CN1 to GT1
            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr2.Id1,
                PlaceEnd = placeRule4.Id1,

                xPos = arcRule4.xPos + 34,
                yPos = arcRule4.yPos - 82,

                Orientation = "PtoT", //Place to Transition
                Type = "(i,arr)"
            };

            var transition1 = CreateTransition(transitionTemplate, tr1);
            var transition2 = CreateTransition(transitionTemplate, tr2);

            var arc1 = CreateArc(arcTemplate, a1);
            var arc2 = CreateArc(arcTemplate, a2);

            //Return transition an arc of GT
            return (placeRule4, tr1, tr2, a2, transition1 + transition2 + arc1 + arc2);
        }

        //Rule 7 : End
        public (PlaceModel, string) Rule7(string placeTemplate, string arcTemplate, 
            PlaceModel lastestPlace, TransitionModel lastestTran, ArcModel lastestArc)
        {
            //End Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "End",
                Type = "INTs",
                InitialMarking = string.Empty,

                xPos1 = lastestPlace.xPos1 - 4,
                yPos1 = lastestPlace.yPos1 - 168,

                xPos2 = lastestPlace.xPos2 - 4,
                yPos2 = lastestPlace.yPos2 - 167,

                xPos3 = lastestPlace.xPos3 - 4,
                yPos3 = lastestPlace.yPos3 - 167,

            };

            //Arc from GF1 to End
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = lastestTran.Id1,
                PlaceEnd = pl.Id1,

                xPos = lastestArc.xPos - 4,
                yPos = lastestArc.yPos - 82,

                Orientation = "TtoP", //Transition to Place
                Type = "arr"
            };

            var place1 = CreatePlace(placeTemplate, pl);
            var arc1 = CreateArc(arcTemplate, a1);
            return (pl, place1 + arc1);
        }

    }
}
