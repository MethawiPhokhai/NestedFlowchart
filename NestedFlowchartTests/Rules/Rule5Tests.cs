using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using NestedFlowchart.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Rules.Tests
{
    [TestClass()]
    public class Rule5Tests
    {
        [TestMethod()]
        public void ApplyRule_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            PositionManagements page1Position = new PositionManagements();

            PreviousNode previousNode = new PreviousNode();
            previousNode.previousPlaceModel = new PlaceModel()
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
            };
            var arrayName = "array";
            int countSubPage = 0;

            // Act
            Rule5 rule5 = new Rule5();
            var (pl, tr, a1, a2) = rule5.ApplyRule(arrayName, previousNode, page1Position, countSubPage);

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