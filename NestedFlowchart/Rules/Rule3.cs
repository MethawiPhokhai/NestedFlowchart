using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules
{
    public class Rule3 : BaseRule
    {
        /// <summary>
        /// Transform process into place and transition connected by arc
        /// </summary>
        /// <returns></returns>
        public (PlaceModel, PlaceModel, PlaceModel, PlaceModel, PlaceModel,
            TransitionModel, TransitionModel,
            ArcModel, ArcModel, ArcModel, ArcModel, ArcModel)
            ApplyRuleWithHierarchy
            (
            string subStrTemplate,
            string portTemplate,
            string page2Id,
            string CodeSegmentValue,
            string arrayName,
            PreviousNode previousNode,
            PositionManagements position1,
            PositionManagements position2)

        {
            TransformationApproach approach = new TransformationApproach();

            var inputPortPlaceName = IdManagements.GetlastestPlaceName();
            var outoutPortPlaceName = IdManagements.GetlastestPlaceName();

            var p3MainPageId = IdManagements.GetlastestPlaceId();
            var p3SubPageId = IdManagements.GetlastestPlaceId();

            var p4OldPageId = IdManagements.GetlastestPlaceId();
            var p4SubPageId = IdManagements.GetlastestPlaceId();

            #region Main Page
            //P3 Place (Input port place) old page
            PlaceModel p3InputPlace = new PlaceModel()
            {
                Id1 = p3MainPageId,
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = inputPortPlaceName,

                xPos1 = position1.xPos1,
                yPos1 = position1.GetLastestyPos1(),

                xPos2 = position1.GetLastestxPos2(),
                yPos2 = position1.GetLastestyPos2(),

                Type = "loopi"
            };

            if (previousNode.Type == "place")
            {
                //TODO: Connect arc to P3 Place
            }

            var tr_subpage_yPos = position1.GetLastestyPos1();
            var subst = new HierarchySubStModel()
            {
                SubPageId = page2Id,
                NewInputPlaceId = p3SubPageId,
                OldInputPlaceId = p3MainPageId,

                NewOutputPlaceId = p4SubPageId,
                OldOutputPlaceId = p4OldPageId,

                Id = IdManagements.GetlastestSubStrId(),
                Name = "New Subpage",

                xPos = position1.xPos1,
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

                xPos1 = position1.xPos1,
                yPos1 = tr_subpage_yPos,

                xPos2 = position1.GetLastestxPos2(),
                yPos2 = position1.GetLastestyPos2(),

                SubsitutetionTransition = approach.CreateHierarchySubSt(subStrTemplate, subst)
            };

            //P4 Place (output port place) old page
            PlaceModel p4OutputPlace = new PlaceModel()
            {
                Id1 = p4OldPageId,
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = outoutPortPlaceName,

                xPos1 = position1.xPos1,
                yPos1 = position1.GetLastestyPos1(),

                xPos2 = position1.GetLastestxPos2(),
                yPos2 = position1.GetLastestyPos2(),

                Type = "loopi"
            };

            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr_subpage.Id1,
                PlaceEnd = p3MainPageId,

                xPos = position1.xArcPos,
                yPos = position1.yArcPos,

                Orientation = "PtoT", //Place to Transition
                Type = $"(i,{arrayName})"
            };

            ArcModel a2 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr_subpage.Id1,
                PlaceEnd = p4OldPageId,

                xPos = position1.GetLastestxArcPos(),
                yPos = position1.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = $"(i,{arrayName})"
            };

            #endregion Main Page

            #region SubPage


            var p3InputPort = new HierarchyPortModel()
            {
                Id = IdManagements.GetlastestPortId(),
                Type = "In",
                xPos = position2.xPos1 - 20,
                yPos = position2.yPos1 - 10
            };

            //P3 Place (Input port place) new page
            PlaceModel p3SubPageInputPlace = new PlaceModel()
            {
                Id1 = p3SubPageId,
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = inputPortPlaceName,

                xPos1 = position2.xPos1,
                yPos1 = position2.yPos1,

                xPos2 = position2.xPos2,
                yPos2 = position2.yPos2,

                xPos3 = position2.xPos3,
                yPos3 = position2.yPos3,

                Type = "loopi",
                Port = approach.CreateHierarchyPort(portTemplate, p3InputPort)
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

                xPos1 = position2.xPos1,
                yPos1 = position2.GetLastestyPos1(),

                xPos2 = position2.xPos2,
                yPos2 = position2.GetLastestyPos2(),

                xPos4 = position2.xPos4 + 85,
                yPos4= position2.yPos4 - 80,

                CodeSegment = codeSeg
            };

            ArcModel a3 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = ts1.Id1,
                PlaceEnd = p3SubPageId,

                xPos = position2.xArcPos,
                yPos = position2.yArcPos,

                Orientation = "PtoT", //Place to Transition
                Type = $"(i,{arrayName})"
            };

            //PS2 Place
            PlaceModel ps2 = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceName(),

                xPos1 = position2.xPos1,
                yPos1 = position2.GetLastestyPos1(),

                xPos2 = position2.GetLastestxPos2(),
                yPos2 = position2.GetLastestyPos2() + 170,

                Type = "loopj"
            };

            ArcModel a4 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = ts1.Id1,
                PlaceEnd = ps2.Id1,

                xPos = position2.xArcPos,
                yPos = position2.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = $"(i,j,{arrayName})"
            };


            var p4OutputPort = new HierarchyPortModel()
            {
                Id = IdManagements.GetlastestPortId(),
                Type = "Out",
                xPos = -4,
                yPos = -168
            };

            //P4 Place (output port place) new page
            PlaceModel p4SubPageOutputPlace = new PlaceModel()
            {
                Id1 = p4SubPageId,
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),
                Name = outoutPortPlaceName,

                xPos1 = position2.xPos1 - 100,
                yPos1 = position2.yPos1,

                xPos2 = position2.xPos2,
                yPos2 = position2.yPos2,

                xPos3 = position2.xPos3,
                yPos3 = position2.yPos3,

                Type = "loopi",
                Port = approach.CreateHierarchyPort(portTemplate, p4OutputPort)
            };

            #endregion SubPage


            return (p3InputPlace, p4OutputPlace, p3SubPageInputPlace, p4SubPageOutputPlace, ps2, tr_subpage, ts1, null, a1, a2, a3, a4);
        }

        public (PlaceModel, TransitionModel, ArcModel) ApplyRuleWithoutHierarchy(
            string CodeSegmentValue,
            string arrayName,
            PositionManagements position
            )
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

                xPos1 = position.xPos1,
                yPos1 = position.GetLastestyPos1(),

                xPos4 = position.GetLastestxPos4(),
                yPos4 = position.GetLastestyPos4(),

                CodeSegment = codeSeg
            };

            //P2 Place
            PlaceModel pl = new PlaceModel()
            {
                Id1 = IdManagements.GetlastestPlaceId(),
                Id2 = IdManagements.GetlastestPlaceId(),
                Id3 = IdManagements.GetlastestPlaceId(),

                Name = IdManagements.GetlastestPlaceName(),

                xPos1 = position.xPos1,
                yPos1 = position.GetLastestyPos1(),

                xPos2 = position.GetLastestxPos2(),
                yPos2 = position.GetLastestyPos2(),

                Type = "loopi"
            };

            //Arc from T2 to P2
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = position.GetLastestxArcPos(),
                yPos = position.GetLastestyArcPos(),

                Orientation = "TtoP", //Transition to Place
                Type = $"(i,{arrayName})"
            };

            return (pl, tr, a1);
        }
    }
}