using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityWeightEqualityTests
    {
        [TestMethod]
        public void TestEquality_KilogramToKilogram_SameValue()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(1.0, WeightUnit.Kilogram);

            bool areEqual = first.Equals(second);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_KilogramToKilogram_DifferentValue()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(2.0, WeightUnit.Kilogram);

            bool areEqual = first.Equals(second);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_KilogramToGram_EquivalentValue()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(1000.0, WeightUnit.Gram);

            bool areEqual = first.Equals(second);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_GramToKilogram_EquivalentValue()
        {
            QuantityWeight first = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight second = new QuantityWeight(1.0, WeightUnit.Kilogram);

            bool areEqual = first.Equals(second);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_WeightVsLength_Incompatible()
        {
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityLength length = new QuantityLength(1.0, LengthUnit.Feet);

            bool areEqual = weight.Equals(length);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_NullComparison()
        {
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.Kilogram);

            bool areEqual = weight.Equals(null);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_SameReference()
        {
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight sameReference = weight;

            bool areEqual = weight.Equals(sameReference);

            Assert.IsTrue(areEqual);
        }
    }
}