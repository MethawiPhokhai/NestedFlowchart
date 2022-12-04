﻿using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Rules
{
    public class Rule3
    {
        public (PlaceModel, TransitionModel, ArcModel, string, string) ApplyRule(string transitionTemplate, string placeTemplate, string arcTemplate, string subStrTemplate, string portTemplate,
            PreviousNode previousNode, bool isHierarchy, string page2Id, string CodeSegmentValue)
        {
            TransformationApproach approach = new TransformationApproach();

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

                var place1 = approach.CreatePlace(placeTemplate, pl);
                var arc1 = approach.CreateArc(arcTemplate, a1);
                var transition = approach.CreateTransition(transitionTemplate, tr);
                var arc2 = approach.CreateArc(arcTemplate, a2);

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
                if (previousNode.Type == "place")
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

                if (previousNode.Type == "place")
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


                    SubsitutetionTransition = approach.CreateHierarchySubSt(subStrTemplate, subst)

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
                    Port = approach.CreateHierarchyPort(portTemplate, p4OutputPort)
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
                var place3old = approach.CreatePlace(placeTemplate, p3old);
                var tr_subpage1 = approach.CreateTransition(transitionTemplate, tr_subpage);
                var place4old = approach.CreatePlace(placeTemplate, p4old);
                var arc0 = approach.CreateArc(arcTemplate, a0);
                var arc1 = approach.CreateArc(arcTemplate, a1);
                var arc2 = approach.CreateArc(arcTemplate, a2);

                var oldPageAllNode = place3old + tr_subpage1 + place4old + arc0 + arc1 + arc2;


                //Sub Page
                var place3new = approach.CreatePlace(placeTemplate, p3new);
                var place4new = approach.CreatePlace(placeTemplate, p4new);
                var transition1 = approach.CreateTransition(transitionTemplate, ts1);
                var arc3 =  approach.CreateArc(arcTemplate, a3);

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
    }
}