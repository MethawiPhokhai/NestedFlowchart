using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System.Data;

namespace NestedFlowchart.Rules.Tests
{
    [TestClass()]
    public class Rule3Tests
    {
        [TestMethod]
        public void ApplyRule_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            PositionManagements page1Position = new PositionManagements();
            PositionManagements page2Position = new PositionManagements();

            List<PreviousNode> previousNodes = new List<PreviousNode>();
            PreviousNode previousNode = new PreviousNode
            {
                currentPlaceModel = new PlaceModel()
                {
                    Id1 = "ID1412948772",
                    Id2 = "ID1412948773",
                    Id3 = "ID1412948774",
                    Name = "Start",
                    Type = "UNIT",
                    xPos1 = page1Position.xPos1,
                    yPos1 = page1Position.yPos1,
                    xPos2 = page1Position.xPos2,
                    yPos2 = page1Position.yPos2,
                    xPos3 = page1Position.xPos3,
                    yPos3 = page1Position.yPos3
                }
            };
            previousNodes.Add(previousNode);

            var arrayName = "array";

            // Act
            Rule3 rule3 = new Rule3();
            var (rule3InputPlace, rule3OutputPlace, rule3InputPlace2, rule3OutputPlace2, rule3PS2,
                            rule3Transition, rule3Transition2,
                            rule3Arc1, rule3Arc2, rule3Arc3, rule3Arc4, rule3Arc5,
                            rule3Transition3, rule3Arc6) = rule3.ApplyRuleWithHierarchy(
                            "Hierarchy_SubPageTransition.txt",
                            "Hierarchy_Port.txt",
                            "ID1412950867",
                            "New Subpage1",
                            "j = 0",
                            arrayName,
                            previousNodes.LastOrDefault(),
                            page1Position,
                            page2Position,
                            0);

            // Assert
            //Main page
            Assert.IsNotNull(rule3InputPlace); //HI
            Assert.IsNotNull(rule3OutputPlace); //HO
            Assert.IsNotNull(rule3Transition); //Substitute Transition

            //Sub Page
            Assert.IsNotNull(rule3InputPlace2); //Hi
            Assert.IsNotNull(rule3OutputPlace2); //HO
            Assert.IsTrue(rule3Transition2.CodeSegment.Contains("j = 0")); //Code Segment
        }
    }
}
