using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using NestedFlowchart.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFlowchart.Rules.Tests
{
    [TestClass()]
    public class Rule6Tests
    {
        [TestMethod()]
        public void ApplyRule_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            PlaceModel previousPlace = new PlaceModel()
            {
                Id1 = "ID1412948772",
                Id2 = "ID1412948773",
                Id3 = "ID1412948774",
                Name = "Start",
                Type = "UNIT",
                xPos1 = PositionManagements.xPos1,
                yPos1 = PositionManagements.yPos1,
                xPos2 = PositionManagements.xPos2,
                yPos2 = PositionManagements.yPos2,
                xPos3 = PositionManagements.xPos3,
                yPos3 = PositionManagements.yPos3
            };
            string trueCondition = "T";
            string falseCondition = "F";
            string arrayName = "array";

            // Act
            Rule6 rule6 = new Rule6();
            var (pl, falseTransition, trueTransition, a1, a2) = rule6.ApplyRule(previousPlace, trueCondition, falseCondition, arrayName);

            // Assert
            Assert.IsNotNull(pl);
            Assert.IsNotNull(falseTransition);
            Assert.IsTrue(falseTransition.Condition.Contains(falseCondition));
            Assert.IsNotNull(trueTransition);
            Assert.IsTrue(trueTransition.Condition.Contains(trueCondition));
            Assert.IsNotNull(a1);
            Assert.IsNotNull(a2);
            Assert.IsTrue(a1.Type.Contains(arrayName));
            Assert.IsTrue(a2.Type.Contains(arrayName));
        }
    }
}