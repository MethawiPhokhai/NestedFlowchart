using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedFlowchart.Models;
using NestedFlowchart.Position;
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
            PositionManagements page1Position = new PositionManagements();

            PlaceModel expectedPlace = new PlaceModel()
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

            // Act
            Rule1 rule1 = new Rule1();
            PlaceModel actualPlace = rule1.ApplyRule(page1Position);

            // Assert
            Assert.AreEqual(JsonConvert.SerializeObject(expectedPlace), JsonConvert.SerializeObject(actualPlace));
        }
    }
}