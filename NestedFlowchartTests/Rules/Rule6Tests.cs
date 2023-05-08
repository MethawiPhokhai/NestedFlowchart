using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
using NestedFlowchart.Rules;
using System;
using System.Collections.Generic;
using System.Data;
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
            Rule6 rule6 = new Rule6();
            string arrayName = "array";

            string trueCondition = rule6.CreateTrueCondition("i < 5", arrayName);
            string falseCondition = rule6.CreateFalseDecision(trueCondition);

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


            // Act
            var (rule6Place, rule6FalseTransition, rule6TrueTransition, rule6Arc1, rule6Arc2) = rule6.ApplyRule(
                previousNodes.LastOrDefault(),
                trueCondition,
                falseCondition,
                arrayName,
                page1Position);

            // Assert
            Assert.IsNotNull(rule6FalseTransition);
            Assert.IsTrue(rule6FalseTransition.Condition.Contains(falseCondition));

            Assert.IsNotNull(rule6TrueTransition);
            Assert.IsTrue(rule6TrueTransition.Condition.Contains(trueCondition));

        }
    }
}