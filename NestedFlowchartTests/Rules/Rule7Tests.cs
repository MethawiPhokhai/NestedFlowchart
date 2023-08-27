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

            //Act
            Rule7 rule7 = new Rule7();
            var endPlace = rule7.ApplyRule(
                       page1Position,
                       0,
                       0);

            //Assert
            Assert.IsNotNull(endPlace);

        }
    }
}