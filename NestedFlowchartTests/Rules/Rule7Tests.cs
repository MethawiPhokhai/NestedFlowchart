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

            string arrayName = "array";
            var previousNode = new PreviousNode
            {
                Type = "transition",
                previousTransitionModel = new TransitionModel
                {
                    Id1 = "ID1412848787"
                }
            };

            //Act
            Rule7 rule7 = new Rule7();
            var (pl, tr, a1) = rule7.ApplyRule(arrayName, previousNode, page1Position);

            //Assert
            Assert.IsNotNull(pl);
            Assert.IsNull(tr);
            Assert.IsNotNull(a1);
        }

        [TestMethod()]
        public void ApplyRule_PlaceNodeInput_ReturnsExpectedOutput()
        {
            //Arrange
            PositionManagements page1Position = new PositionManagements();

            string arrayName = "array";
            var previousNode = new PreviousNode
            {
                Type = "place"
            };

            //Act
            Rule7 rule7 = new Rule7();
            var (pl, tr, a1) = rule7.ApplyRule(arrayName, previousNode, page1Position);

            //Assert
            Assert.IsNotNull(pl);
            Assert.IsNotNull(tr);
            Assert.IsNotNull(a1);
        }

    }
}