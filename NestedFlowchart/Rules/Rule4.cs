﻿using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System.Configuration;

namespace NestedFlowchart.Rules
{
    public class Rule4 : ArcBaseRule
    {
        private readonly ITypeBaseRule _typeBaseRule;

        public Rule4()
        {
            _typeBaseRule = new TypeBaseRule(); ;
        }

        /// <summary>
        /// Transform simple process into place and transition connected by arc
        /// </summary>
        /// <param name="transitionTemplate"></param>
        /// <param name="placeTemplate"></param>
        /// <param name="arcTemplate"></param>
        /// <param name="previousNode"></param>
        /// <returns></returns>
        public (PlaceModel, TransitionModel, ArcModel, string) ApplyRule(
            string transitionTemplate,
            string placeTemplate,
            string arcTemplate,
            string arrayName,
            PreviousNode previousNode,
            PositionManagements position,
            int type)
        {
            //T4 Transition
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
            };

            //P4
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

                Type = _typeBaseRule.GetTypeByInitialMarkingType(type, previousNode.CurrentMainPage)
            };

            //Arc from P2 to T3
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

            TransformationApproach approach = new TransformationApproach();
            var place1 = approach.CreatePlace(placeTemplate, pl);
            var arc1 = approach.CreateArc(arcTemplate, a1);
            var transition = approach.CreateTransition(transitionTemplate, tr);

            var allNode = place1 + transition + arc1;
            return (pl, tr, a1, allNode);
        }

        public (PlaceModel, TransitionModel, ArcModel) ApplyRuleWithCodeSegment(
            string arrayName,
            PreviousNode previousNode,
            PositionManagements position,
            int type)
        {
            var loop2 = ConfigurationManager.AppSettings["loop2"]?.ToString() ?? "loop2";
            int currentMainPage = previousNode.CurrentMainPage;

            //PS4
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

                Type = _typeBaseRule.GetTypeByPageOnly(currentMainPage)
            };

            var codeSeg = $"input (array,{loop2});\r\n" +
                "output (array2);\r\n" +
                "action\r\n" +
                "let\r\n" +
                $" (* get {loop2} to temp*)\r\n " +
                $"val temp = List.nth(array,{loop2})\r\n " +
                $"(* get {loop2}+1 to temp2 *)\r\n " +
                $"val temp2 = List.nth(array,{loop2}+1)\r\n " +
                $"(* return first {loop2} element of array *)\r\n " +
                $"val array2 = List.take(array,{loop2})\r\n " +
                "(* insert element temp2 after array2 *)\r\n " +
                "val array2 = ins array2 temp2\r\n " +
                "(* insert element temp after array2 *)\r\n " +
                "val array2 = ins array2 temp\r\n " +
                "(* removes all elements in list array2 from list array1 *)\r\n " +
                "val array  = listsub array array2\r\n " +
                "(* concat array2 with array1 *)\r\n " +
                "val array2 = array2^^array\r\n\r\n" +
                "in\r\n " +
                "array2\r\n" +
                "end";

            //TS2 Transition
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

                xPos2 = position.xPos2,
                yPos2 = position.GetLastestyPos2(),

                xPos4 = position.GetLastestxPos4() + 180,
                yPos4 = position.GetLastestyPos4() - 320,

                CodeSegment = codeSeg
            };

            //Arc from PS4 to TS2
            ArcModel a1 = new ArcModel()
            {
                Id1 = IdManagements.GetlastestArcId(),
                Id2 = IdManagements.GetlastestArcId(),

                TransEnd = tr.Id1,
                PlaceEnd = pl.Id1,

                xPos = position.GetLastestxArcPos(),
                yPos = position.GetLastestyArcPos(),

                Orientation = "PtoT", //Place to Transition
                Type = GetArcVariableByPageAndType(arrayName, currentMainPage, type)
            };

            return (pl, tr, a1);
        }

        //i++ j++, k++, l++, m++
        public TransitionModel ApplyRuleWithCodeSegment2(
        string loopVariable,
        PositionManagements position)
        {
            var codeSeg = $"input ({loopVariable});\r\n" +
                $"output ({loopVariable}2);\r\n" +
                "action\r\n" +
                "let\r\n" +
                $"val {loopVariable}2 = {loopVariable}+1\r\n" +
                "in\r\n " +
                $"{loopVariable}2\r\n" +
                "end";

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

                xPos2 = position.xPos2,
                yPos2 = position.GetLastestyPos2(),

                xPos4 = position.xPos4 + 40,
                yPos4 = position.GetLastestyPos4() - 290,

                CodeSegment = codeSeg
            };

            return (tr);
        }
    }
}