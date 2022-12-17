﻿using NestedFlowchart.Functions;
using NestedFlowchart.Models;

namespace NestedFlowchart.Rules
{
    /// <summary>
    /// Transform Initialize Process into transition and place connected by arc
    /// </summary>
    public class Rule2
    {
        public (PlaceModel, TransitionModel, ArcModel, ArcModel)
            ApplyRule(
            PlaceModel placeRule1,
            string arrayName)
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
                Type = arrayName
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
                Type = arrayName
            };

            return (pl, tr, a1, a2);
        }

        public string AssignInitialMarking(
            List<XMLCellNode> sortedFlowcharts, 
            string arrayName, 
            PlaceModel rule1Place, 
            int i)
        {
            if (sortedFlowcharts[i].ValueText.Contains('['))
            {
                arrayName = SubstringBefore(sortedFlowcharts[i].ValueText.Trim(), '=').Trim();
                var arrayValue = SubstringAfter(sortedFlowcharts[i].ValueText.Trim(), '=');

                rule1Place.Type = "INTs";
                rule1Place.InitialMarking = arrayValue;
            }

            return arrayName;
        }

        private string SubstringBefore(string str, char ch)
        {
            int index = str.IndexOf(ch);
            return index >= 0 ? str.Substring(0, index) : str;
        }

        private string SubstringAfter(string str, char ch)
        {
            int index = str.IndexOf(ch);
            return index >= 0 ? str.Substring(index + 1) : string.Empty;
        }
    }
}