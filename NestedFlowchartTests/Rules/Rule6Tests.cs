﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class Rule6Tests
    {
        [TestMethod()]
        public void ApplyRule_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            PositionManagements page1Position = new PositionManagements();

            PlaceModel previousPlace = new PlaceModel()
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
            string trueCondition = "T";
            string falseCondition = "F";
            string arrayName = "array";

            // Act
            Rule6 rule6 = new Rule6();
            var (pl, falseTransition, trueTransition, a1, a2) = rule6.ApplyRule(previousPlace, trueCondition, falseCondition, arrayName, page1Position);

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