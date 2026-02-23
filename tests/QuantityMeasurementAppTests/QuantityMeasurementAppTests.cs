using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementAppTests
    {
        [TestMethod]
        public void TestFeetEquality_SameValue()
        {
            Feet firstFeet = new Feet(1.0);
            Feet secondFeet = new Feet(1.0);

            bool areEqual = firstFeet.Equals(secondFeet);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestFeetEquality_DifferentValue()
        {
            Feet firstFeet = new Feet(1.0);
            Feet secondFeet = new Feet(2.0);

            bool areEqual = firstFeet.Equals(secondFeet);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestFeetEquality_NullComparison()
        {
            Feet firstFeet = new Feet(1.0);

            bool areEqual = firstFeet.Equals(null);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestFeetEquality_DifferentClass()
        {
            Feet firstFeet = new Feet(1.0);
            object nonFeetObject = "1.0";

            bool areEqual = firstFeet.Equals(nonFeetObject);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestFeetEquality_SameReference()
        {
            Feet firstFeet = new Feet(1.0);
            Feet sameReferenceFeet = firstFeet;

            bool areEqual = firstFeet.Equals(sameReferenceFeet);

            Assert.IsTrue(areEqual);
        }
    }
}