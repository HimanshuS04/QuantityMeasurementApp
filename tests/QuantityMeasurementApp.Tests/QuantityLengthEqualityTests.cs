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

        [TestMethod]
        public void TestEquality_YardToYard_SameValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Yard);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_YardToYard_DifferentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength secondQuantity = new QuantityLength(2.0, LengthUnit.Yard);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_YardToFeet_EquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength secondQuantity = new QuantityLength(3.0, LengthUnit.Feet);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_FeetToYard_EquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(3.0, LengthUnit.Feet);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Yard);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_YardToInches_EquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength secondQuantity = new QuantityLength(36.0, LengthUnit.Inch);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_InchesToYard_EquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(36.0, LengthUnit.Inch);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Yard);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_YardToFeet_NonEquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength secondQuantity = new QuantityLength(2.0, LengthUnit.Feet);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_CentimetersToInches_EquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Centimeter);
            QuantityLength secondQuantity = new QuantityLength(0.393701, LengthUnit.Inch);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_CentimetersToFeet_NonEquivalentValue()
        {
            QuantityLength firstQuantity = new QuantityLength(1.0, LengthUnit.Centimeter);
            QuantityLength secondQuantity = new QuantityLength(1.0, LengthUnit.Feet);

            bool areEqual = firstQuantity.Equals(secondQuantity);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_MultiUnit_TransitiveProperty()
        {
            QuantityLength yardQuantity = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength feetQuantity = new QuantityLength(3.0, LengthUnit.Feet);
            QuantityLength inchQuantity = new QuantityLength(36.0, LengthUnit.Inch);

            bool yardEqualsFeet = yardQuantity.Equals(feetQuantity);
            bool feetEqualsInch = feetQuantity.Equals(inchQuantity);
            bool yardEqualsInch = yardQuantity.Equals(inchQuantity);

            Assert.IsTrue(yardEqualsFeet && feetEqualsInch && yardEqualsInch);
        }

        [TestMethod]
        public void TestEquality_AllUnits_ComplexScenario()
        {
            QuantityLength firstQuantity = new QuantityLength(2.0, LengthUnit.Yard);
            QuantityLength secondQuantity = new QuantityLength(6.0, LengthUnit.Feet);
            QuantityLength thirdQuantity = new QuantityLength(72.0, LengthUnit.Inch);

            bool firstEqualsSecond = firstQuantity.Equals(secondQuantity);
            bool secondEqualsThird = secondQuantity.Equals(thirdQuantity);
            bool firstEqualsThird = firstQuantity.Equals(thirdQuantity);

            Assert.IsTrue(firstEqualsSecond && secondEqualsThird && firstEqualsThird);
        }
    }
}