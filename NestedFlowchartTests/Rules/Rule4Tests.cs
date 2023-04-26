using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using System.Data;

namespace NestedFlowchart.Rules.Tests
{
    [TestClass()]
    public class Rule4Tests
    {
        [TestMethod]
        public void ApplyRule_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            PositionManagements page1Position = new PositionManagements();

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
            Rule4 rule4 = new Rule4();
            var (rule4Transition, rule4Arc1, rule4Arc2) = rule4.ApplyRuleWithCodeSegment3(
                                                    arrayName,
                                                    previousNodes.LastOrDefault(),
                                                    page1Position);

            // Assert
            Assert.IsNotNull(rule4Transition);
            Assert.IsTrue(rule4Transition.CodeSegment.Contains("i+1"));

        }
    }
}
