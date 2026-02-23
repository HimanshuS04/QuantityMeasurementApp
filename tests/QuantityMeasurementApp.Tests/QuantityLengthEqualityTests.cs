using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthEqualityTests
    {
        [TestMethod]
        public void TestEquality_FeetToFeet_SameValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Feet);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_InchToInch_SameValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Inch);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Inch);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_InchToFeet_EquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(12.0, LengthUnit.Inch);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Feet);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_FeetToFeet_DifferentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength secondQuantity = new QuantityLength(2.0, LengthUnit.Feet);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_InchToInch_DifferentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Inch);
            QuantityLength secondQuantity = new QuantityLength(2.0, LengthUnit.Inch);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_InvalidUnit()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, (LengthUnit)999);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Feet);

            try
            {
                bool _ = firstQuantity.Equals(secondQuantity);
                Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
                // Expected exception
            }
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength sameReference = firstQuantity;

            bool areEqual = firstQuantity.Equals(sameReference);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Feet);

            bool areEqual = firstQuantity.Equals(null);

            Assert.IsFalse(areEqual);
        }
    }
}