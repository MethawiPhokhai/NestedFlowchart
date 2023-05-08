using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Models;
using NestedFlowchart.Position;

namespace NestedFlowchart.Rules.Tests
{
    [TestClass()]
    public class Rule7Tests
    {
        [TestMethod()]
        public void ApplyRule_TransitionNodeInput_ReturnsExpectedOutput()
        {
            //Arrange
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
                    Type = "transition",
                    xPos1 = page1Position.xPos1,
                    yPos1 = page1Position.yPos1,
                    xPos2 = page1Position.xPos2,
                    yPos2 = page1Position.yPos2,
                    xPos3 = page1Position.xPos3,
                    yPos3 = page1Position.yPos3
                }
            };
            previousNodes.Add(previousNode);

            string arrayName = "array";

            //Act
            Rule7 rule7 = new Rule7();
            var endPlace = rule7.ApplyRule(
                       arrayName,
                       page1Position,
                       previousNodes.LastOrDefault());

            //Assert
            Assert.IsNotNull(endPlace);

        }
    }
}