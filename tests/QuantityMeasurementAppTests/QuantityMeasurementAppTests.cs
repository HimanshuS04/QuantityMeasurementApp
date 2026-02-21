using QuantityMeasurementApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Alias for the inner Feet class to make usage cleaner
using Feet = QuantityMeasurementApp.QuantityMeasurementApp.Feet;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class QuantityMeasurementAppTests
    {
        [TestMethod]
        public void TestFeetEquality_SameValue()
        {
            // given
            Feet firstFeet = new Feet(1.0);
            Feet secondFeet = new Feet(1.0);

            // when
            bool areEqual = firstFeet.Equals(secondFeet);

            // then
            Assert.IsTrue(areEqual, "Two Feet instances with the same value should be equal.");
        }

        [TestMethod]
        public void TestFeetEquality_DifferentValue()
        {
            // given
            Feet firstFeet = new Feet(1.0);
            Feet secondFeet = new Feet(2.0);

            // when
            bool areEqual = firstFeet.Equals(secondFeet);

            // then
            Assert.IsFalse(areEqual, "Feet instances with different values should not be equal.");
        }

        [TestMethod]
        public void TestFeetEquality_NullComparison()
        {
            // given
            Feet firstFeet = new Feet(1.0);

            // when
            bool areEqual = firstFeet.Equals(null);

            // then
            Assert.IsFalse(areEqual, "Feet instance should not be equal to null.");
        }

        [TestMethod]
        public void TestFeetEquality_DifferentClass()
        {
            // given
            Feet firstFeet = new Feet(1.0);
            object nonFeetObject = "1.0"; // non-numeric  different type input

            // when
            bool areEqual = firstFeet.Equals(nonFeetObject);

            // then
            Assert.IsFalse(
                areEqual,
                "Feet instance should not be equal to an instance of a different class (non-numeric input)."
            );
        }

        [TestMethod]
        public void TestFeetEquality_SameReference()
        {
            // given
            Feet firstFeet = new Feet(1.0);
            Feet sameReferenceFeet = firstFeet;

            // when
            bool areEqual = firstFeet.Equals(sameReferenceFeet);

            // then
            Assert.IsTrue(areEqual, "Feet instance should be equal to itself (same reference).");
        }
    }
}