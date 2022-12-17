using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Functions;
using NestedFlowchart.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace NestedFlowchart.Rules.Tests
{
    [TestClass()]
    public class Rule1Tests
    {
        [TestMethod]
        public void ApplyRule_ValidInput_ReturnsExpectedOutput()
        {
            // Arrange
            PlaceModel expectedPlace = new PlaceModel()
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

            // Act
            Rule1 rule1 = new Rule1();
            PlaceModel actualPlace = rule1.ApplyRule();

            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(expectedPlace), JsonConvert.SerializeObject(actualPlace));
        }
    }
}