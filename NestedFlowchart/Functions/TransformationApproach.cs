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
        public string CreatePage(string pageTemplate, PageModel model)
        {
            return string.Format("\n" + pageTemplate, model.Id, model.Name, model.Node);
        }


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
            placeTemplate = string.Format("\n" + placeTemplate, model.Id1, model.Id2, model.Id3, 
                model.Name, model.Type, model.InitialMarking, 
                model.xPos1, model.yPos1,
                model.xPos2, model.yPos2,
                model.xPos3, model.yPos3,
                model.Port);

            return placeTemplate;
        }

        public string CreateTransition(string transitionTemplate, TransitionModel model)
        {
            string transition = string.Format("\n" + transitionTemplate,model.Id1, model.Id2, model.Id3, model.Id4, model.Id5,
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
            string arc = string.Format("\n" + arcTemplate,model.Id1, model.Id2, 
                model.TransEnd, model.PlaceEnd, 
                model.Orientation, model.Type,
                model.xPos, model.yPos);

            return arc;
        }


        public string CreateHierarchyInstance(string instanceTemplate, HierarchyInstanceModel model)
        {
            return string.Format(instanceTemplate, model.Id, model.Text, model.Closer);
        }

        public string CreateHierarchySubSt(string subStrTemplate, HierarchySubStModel model)
        {
            return string.Format("\n" + subStrTemplate, model.SubPageId,
                model.NewInputPlaceId, model.OldInputPlaceId,
                model.NewOutputPlaceId, model.OldOutputPlaceId,
                model.Id, model.Name,
                model.xPos, model.yPos);
        }

        public string CreateHierarchyPort(string portTemplate, HierarchyPortModel model)
        {
            return string.Format("\n" + portTemplate, model.Id,
                model.Type, model.xPos, model.yPos);
        }

        #endregion


        //TODO: Rule 3-6 can specify to subpage
        //Rule 1 : Transform start to place start
        public (PlaceModel,string) Rule1(string placeTemplate)
        {
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "Start",
                Type = "UNIT",

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.yPos1,

                xPos2 = PositionManagements.xPos2,
                yPos2 = PositionManagements.yPos2,

                xPos3 = PositionManagements.xPos3,
                yPos3 = PositionManagements.yPos3
            };

            var place1 = CreatePlace(placeTemplate, pl);

            return (pl, place1);
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

                Name = IdManagements.GetlastestTransitionName(),

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.GetLastestyPos1(),

            };

            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceName(),

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.GetLastestyPos1(),

                xPos2 = PositionManagements.GetLastestxPos2(),
                yPos2 = PositionManagements.GetLastestyPos2(),

                Type = "INTs"
            };

            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = placeRule1.Id1,

                xPos = PositionManagements.xArcPos,
                yPos = PositionManagements.yArcPos,

                Orientation = "PtoT", //Place to Transition
                Type = "arr"
            };

            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = PositionManagements.GetLastestxArcPos(),
                yPos = PositionManagements.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = "arr"
            };

            var place1 = CreatePlace(placeTemplate, placeRule1);
            var arc1 = CreateArc(arcTemplate, a1);
            var transition = CreateTransition(transitionTemplate, tr);
            var arc2 = CreateArc(arcTemplate, a2);
            var place2 = CreatePlace(placeTemplate, pl);

            var allNode = place1 + place2 + transition + arc1 + arc2;
            return (pl, tr, a2, allNode);

        }

        //Rule 3 : Transform Nested Structure into Hierarchical
        public (PlaceModel, TransitionModel, ArcModel, string, string) Rule3(string transitionTemplate, string placeTemplate, string arcTemplate, string subStrTemplate, string portTemplate,
            PreviousNode previousNode, bool isHierarchy, string page2Id, string CodeSegmentValue)
        {
            if (!isHierarchy)
            {
                //T2 Code Segment Inscription
                //Define i=1 in Code Segment Inscription
                var codeSeg = "input (); \n " +
                    "output(i); \n " +
                    "action \n " +
                    "let \n" +
                    "val " + CodeSegmentValue + " \n" +
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

                    Name = IdManagements.GetlastestTransitionName(),

                    xPos1 = PositionManagements.xPos1,
                    yPos1 = PositionManagements.GetLastestyPos1(),

                    xPos4 = PositionManagements.GetLastestxPos4(),
                    yPos4 = PositionManagements.GetLastestyPos4(),

                    CodeSegment = codeSeg
                };

                //P2 Place
                PlaceModel pl = new PlaceModel()
                {
                    Id1 = IdManagements.GetlastestPlaceId(),
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),

                    Name = IdManagements.GetlastestPlaceName(),

                    xPos1 = PositionManagements.xPos1,
                    yPos1 = PositionManagements.GetLastestyPos1(),

                    xPos2 = PositionManagements.GetLastestxPos2(),
                    yPos2 = PositionManagements.GetLastestyPos2(),

                    Type = "loopi"
                };

                //Arc from P1 to T2
                ArcModel a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr.Id1,
                    PlaceEnd = previousNode.previousPlaceModel.Id1,

                    xPos = PositionManagements.GetLastestxArcPos(),
                    yPos = PositionManagements.GetLastestyArcPos(),

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

                    xPos = PositionManagements.GetLastestxArcPos(),
                    yPos = PositionManagements.GetLastestyArcPos(),

                    Orientation = "TtoP", //Transition to Place
                    Type = "(i,arr)"
                };

                var place1 = CreatePlace(placeTemplate, pl);
                var arc1 = CreateArc(arcTemplate, a1);
                var transition = CreateTransition(transitionTemplate, tr);
                var arc2 = CreateArc(arcTemplate, a2);

                return (pl, tr, a2, (place1 + transition + arc1 + arc2), string.Empty);
            }
            else
            {
                var inputPortPlaceName = IdManagements.GetlastestPlaceName();
                var outoutPortPlaceName = IdManagements.GetlastestPlaceName();

                var p3oldId = IdManagements.GetlastestPlaceId();
                var p3newId = IdManagements.GetlastestPlaceId();

                var p4oldId = IdManagements.GetlastestPlaceId();
                var p4newId = IdManagements.GetlastestPlaceId();

                #region Main Page
                var a0 = new ArcModel();

                //TODO: if node before is place create transition
                if(previousNode.Type == "place")
                {
                    //TODO: create Transition

                    //TODO: connect arc to P3old
                }
                else
                {
                    a0 = new ArcModel()
                    {
                        Id1 = IdManagements.GetlastestArcId(),
                        Id2 = IdManagements.GetlastestArcId(),

                        TransEnd = previousNode.previousTransitionModel.Id1,
                        PlaceEnd = p3oldId,

                        xPos = PositionManagements.xArcPos,
                        yPos = PositionManagements.yArcPos,

                        Orientation = "TtoP", //Transition to Place
                        Type = "(i,arr)"
                    };
                }

                //P3 Place (Input port place) old page
                PlaceModel p3old = new PlaceModel()
                {
                    Id1 = p3oldId,
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),

                    Name = inputPortPlaceName,

                    xPos1 = PositionManagements.xPos1,
                    yPos1 = PositionManagements.GetLastestyPos1(),

                    xPos2 = PositionManagements.GetLastestxPos2(),
                    yPos2 = PositionManagements.GetLastestyPos2(),

                    Type = "loopi"
                };

                if(previousNode.Type == "place")
                {
                    //TODO: Connect arc to P3 Place
                }

                var p3InputPort = new HierarchyPortModel()
                {
                    Id = IdManagements.GetlastestPortId(),
                    Type = "In",
                    xPos = -4,
                    yPos = -167
                };


                var tr_subpage_yPos = PositionManagements.GetLastestyPos1();
                var subst = new HierarchySubStModel()
                {
                    SubPageId = page2Id,
                    NewInputPlaceId = p3newId,
                    OldInputPlaceId = p3oldId,

                    NewOutputPlaceId = p4newId,
                    OldOutputPlaceId = p4oldId,

                    Id = IdManagements.GetlastestSubStrId(),
                    Name = "New Subpage",

                    xPos = PositionManagements.xPos1,
                    yPos = tr_subpage_yPos + 30
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

                    xPos1 = PositionManagements.xPos1,
                    yPos1 = tr_subpage_yPos,

                    xPos2 = PositionManagements.GetLastestxPos2(),
                    yPos2 = PositionManagements.GetLastestyPos2(),

                    SubsitutetionTransition = CreateHierarchySubSt(subStrTemplate, subst)

                };

                //P4 Place (output port place) old page
                PlaceModel p4old = new PlaceModel()
                {
                    Id1 = p4oldId,
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),
                    Name = outoutPortPlaceName,

                    xPos1 = PositionManagements.xPos1,
                    yPos1 = PositionManagements.GetLastestyPos1(),

                    xPos2 = PositionManagements.GetLastestxPos2(),
                    yPos2 = PositionManagements.GetLastestyPos2(),

                    Type = "loopi"
                };

                ArcModel a1 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr_subpage.Id1,
                    PlaceEnd = p3oldId,

                    xPos = PositionManagements.xArcPos,
                    yPos = PositionManagements.yArcPos,

                    Orientation = "PtoT", //Place to Transition
                    Type = "(i,arr)"
                };

                ArcModel a2 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = tr_subpage.Id1,
                    PlaceEnd = p4oldId,

                    xPos = PositionManagements.GetLastestxArcPos(),
                    yPos = PositionManagements.GetLastestyArcPos(),

                    Orientation = "TtoP", //Transition to Place
                    Type = "(i,arr)"
                };

                #endregion


                #region SubPage
                //TODO: Create position for sub page
                //TODO: find solution for position on sub page
                //P3 Place (Input port place) new page
                PlaceModel p3new = new PlaceModel()
                {
                    Id1 = p3newId,
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),
                    Name = inputPortPlaceName,

                    xPos1 = previousNode.previousPlaceModel.xPos1 - 4,
                    yPos1 = previousNode.previousPlaceModel.yPos1 - 168,

                    xPos2 = previousNode.previousPlaceModel.xPos2 - 4,
                    yPos2 = previousNode.previousPlaceModel.yPos2 - 167,

                    xPos3 = previousNode.previousPlaceModel.xPos3 - 4,
                    yPos3 = previousNode.previousPlaceModel.yPos3 - 167,

                    Type = "loopi",
                    Port = CreateHierarchyPort(portTemplate, p3InputPort)
                };

                //Define i=1 in Code Segment Inscription
                var codeSeg = "input (); \n " +
                    "output(j); \n " +
                    "action \n " +
                    "let \n" +
                    "val " + CodeSegmentValue + " \n" +
                    "in \n" +
                    "(j) \n" +
                    "end";

                //TS1 transition
                TransitionModel ts1 = new TransitionModel()
                {
                    Id1 = IdManagements.GetlastestTransitionId(),
                    Id2 = IdManagements.GetlastestTransitionId(),
                    Id3 = IdManagements.GetlastestTransitionId(),
                    Id4 = IdManagements.GetlastestTransitionId(),
                    Id5 = IdManagements.GetlastestTransitionId(),

                    Name = IdManagements.GetlastestSubPageTransitionName(),

                    xPos1 = previousNode.previousPlaceModel.xPos1 - 9,
                    yPos1 = previousNode.previousPlaceModel.yPos1 - 168,

                    xPos2 = previousNode.previousPlaceModel.xPos2 - 9,
                    yPos2 = previousNode.previousPlaceModel.yPos2 - 168,

                    xPos3 = previousNode.previousPlaceModel.xPos3 - 9,
                    yPos3 = previousNode.previousPlaceModel.yPos3 - 168,

                    CodeSegment = codeSeg
                };

                var p4OutputPort = new HierarchyPortModel()
                {
                    Id = IdManagements.GetlastestPortId(),
                    Type = "Out",
                    xPos = -4,
                    yPos = -168
                };

                //P4 Place (output port place) new page
                PlaceModel p4new = new PlaceModel()
                {
                    Id1 = p4newId,
                    Id2 = IdManagements.GetlastestPlaceId(),
                    Id3 = IdManagements.GetlastestPlaceId(),
                    Name = outoutPortPlaceName,

                    xPos1 = previousNode.previousPlaceModel.xPos1 - 4,
                    yPos1 = previousNode.previousPlaceModel.yPos1 - 168,

                    xPos2 = previousNode.previousPlaceModel.xPos2 - 4,
                    yPos2 = previousNode.previousPlaceModel.yPos2 - 167,

                    xPos3 = previousNode.previousPlaceModel.xPos3 - 4,
                    yPos3 = previousNode.previousPlaceModel.yPos3 - 167,

                    Type = "loopi",
                    Port = CreateHierarchyPort(portTemplate, p4OutputPort)
                };

                ArcModel a3 = new ArcModel()
                {
                    Id1 = IdManagements.GetlastestArcId(),
                    Id2 = IdManagements.GetlastestArcId(),

                    TransEnd = ts1.Id1,
                    PlaceEnd = p3newId,

                    xPos = PositionManagements.xArcPos,
                    yPos = PositionManagements.yArcPos,

                    Orientation = "PtoT", //Place to Transition
                    Type = "(i,arr)"
                };

                #endregion




                //Main Page
                var place3old = CreatePlace(placeTemplate, p3old);
                var tr_subpage1 = CreateTransition(transitionTemplate, tr_subpage);
                var place4old = CreatePlace(placeTemplate, p4old);
                var arc0 = CreateArc(arcTemplate, a0);
                var arc1 = CreateArc(arcTemplate, a1);
                var arc2 = CreateArc(arcTemplate, a2);

                var oldPageAllNode = place3old + tr_subpage1 + place4old + arc0 + arc1 + arc2;


                //Sub Page
                var place3new = CreatePlace(placeTemplate, p3new);
                var place4new = CreatePlace(placeTemplate, p4new);
                var transition1 = CreateTransition(transitionTemplate, ts1);
                var arc3 = CreateArc(arcTemplate, a3);

                var newPageAllNode = place3new + place4new + transition1 + arc3;

                //Return
                //1. P4new => for next node
                //2. tr_subpage => create instance
                //3. Arc => null
                //4. oldPageAllNode => all node for old page
                //5. newPageAllNode => all node for new page
                return (p4new, tr_subpage, null, oldPageAllNode, newPageAllNode);
            }
        }

        //Rule 4 Simple Process


        //Rule 5 : Connector
        public (PlaceModel, TransitionModel, ArcModel, string) Rule5(string transitionTemplate, string placeTemplate, string arcTemplate,
            PlaceModel previousPlace)
        {
            //T3 Transition
            TransitionModel tr = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = IdManagements.GetlastestTransitionName(),

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.GetLastestyPos1(),

                xPos4 = PositionManagements.GetLastestxPos4(),
                yPos4 = PositionManagements.GetLastestyPos4(),
            };

            //CN1 Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceConnectorName(),

                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.GetLastestyPos1(),

                xPos2 = PositionManagements.GetLastestxPos2(),
                yPos2 = PositionManagements.GetLastestyPos2(),

                Type = "loopi"
            };

            //Arc from P2 to T3
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = previousPlace.Id1,

                xPos = PositionManagements.GetLastestxArcPos(),
                yPos = PositionManagements.GetLastestyArcPos(),

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

                xPos = PositionManagements.GetLastestxArcPos(),
                yPos = PositionManagements.GetLastestyArcPos(),

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
            PreviousNode previousNode, string trueCondition, string falseCondition)
        {

            var xPos1 = PositionManagements.xPos1;
            var yPos1 = PositionManagements.GetLastestyPos1();

            var xPosArc = PositionManagements.GetLastestxArcPos();
            var yPosArc = PositionManagements.GetLastestyArcPos();

            //GF1 Transition
            TransitionModel tr1 = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = IdManagements.GetlastestFalseGuardTransitionName(),

                xPos1 = xPos1 - 39,
                yPos1 = yPos1,

                xPos2 = xPos1 - 80,
                yPos2 = yPos1 + 30,

                Condition = falseCondition

            };

            //GT1 Transition
            TransitionModel tr2 = new TransitionModel()
            {
                Id1 = IdManagements.GetlastestTransitionId(),
                Id2 = IdManagements.GetlastestTransitionId(),
                Id3 = IdManagements.GetlastestTransitionId(),
                Id4 = IdManagements.GetlastestTransitionId(),
                Id5 = IdManagements.GetlastestTransitionId(),

                Name = IdManagements.GetlastestTrueGuardTransitionName(),

                xPos1 = xPos1 + 39,
                yPos1 = yPos1,

                xPos2 = xPos1 + 80,
                yPos2 = yPos1 + 30,

                Condition = trueCondition
            };

            //Arc from CN1 to GF1
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr1.Id1,
                PlaceEnd = previousNode.previousPlaceModel.Id1,

                xPos = xPosArc - 84,
                yPos = yPosArc,

                Orientation = "PtoT", //Place to Transition
                Type = "(i,arr)"
            };

            //Arc from CN1 to GT1
            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr2.Id1,
                PlaceEnd = previousNode.previousPlaceModel.Id1,

                xPos = xPosArc + 34,
                yPos = yPosArc,

                Orientation = "PtoT", //Place to Transition
                Type = "(i,arr)"
            };

            var transition1 = CreateTransition(transitionTemplate, tr1);
            var transition2 = CreateTransition(transitionTemplate, tr2);

            var arc1 = CreateArc(arcTemplate, a1);
            var arc2 = CreateArc(arcTemplate, a2);

            //Tr1 = GF
            //Tr2 = GT
            return (previousNode.previousPlaceModel, tr1, tr2, a2, transition1 + transition2 + arc1 + arc2);
        }

        //Rule 7 : End
        public (PlaceModel, string) Rule7(string placeTemplate, string arcTemplate, 
            PreviousNode previousNode)
        {
            if (previousNode.Type == "place")
            {
                //TODO: Create Transition
            }

            //End Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = "End",
                Type = "INTs",
                InitialMarking = string.Empty,

                xPos1 = previousNode.previousPlaceModel.xPos1 - 4,
                yPos1 = previousNode.previousPlaceModel.yPos1 - 168,

                xPos2 = previousNode.previousPlaceModel.xPos2 - 4,
                yPos2 = previousNode.previousPlaceModel.yPos2 - 167,

                xPos3 = previousNode.previousPlaceModel.xPos3 - 4,
                yPos3 = previousNode.previousPlaceModel.yPos3 - 167,

            };

            //TODO: find solution to create Arc
            if(previousNode.Type == "place")
            {
                //TODO: Connect Arc to pl
            }

            //Arc from GF1 to End
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = previousNode.previousTransitionModel.Id1,
                PlaceEnd = pl.Id1,

                xPos = PositionManagements.GetLastestxArcPos(),
                yPos = PositionManagements.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = "arr"
            };

            var place1 = CreatePlace(placeTemplate, pl);
            var arc1 = CreateArc(arcTemplate, a1);
            return (pl, place1 + arc1);
        }

    }
}
