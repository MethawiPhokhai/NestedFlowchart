using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using NestedFlowchart.Rules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Rules.Tests
{
    [TestClass()]
    public class Rule2Tests
    {
        [TestMethod]
        public void ApplyRule_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            PositionManagements page1Position = new PositionManagements();

            List<PreviousNode> previousNodes = new List<PreviousNode>();
            PreviousNode previousNode = new PreviousNode
            {
                previousPlaceModel = new PlaceModel()
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

            TempArrow arrow = new TempArrow();

            // Act
            Rule2 rule2 = new Rule2();
            var (pl, tr, a1, a2) = rule2.ApplyRule(previousNodes, arrayName, page1Position, arrow);

            // Assert
            Assert.IsNotNull(pl);
            Assert.IsNotNull(tr);
            Assert.IsNotNull(a1);
            Assert.IsNotNull(a2);
            Assert.AreEqual(previousNode.previousPlaceModel.Id1, a1.PlaceEnd);
            Assert.IsTrue(a1.Type.Contains(arrayName));
            Assert.IsTrue(a2.Type.Contains(arrayName));
        }
    }
}
