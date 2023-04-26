using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            string arrayName = "array";


            //Act
            Rule7 rule7 = new Rule7();
            var rule7Place = rule7.ApplyRule(
                       arrayName,
                       page1Position);

            var rule7Arc1 = rule7.CreateArcforEndPlace(page1Position);

            //Assert
            Assert.IsNotNull(rule7Place);
            Assert.IsNotNull(rule7Arc1);

        }
    }
}