using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityWeightConversionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestConversion_KilogramToGram()
        {
            QuantityWeight weight = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight result = weight.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void TestConversion_GramToKilogram()
        {
            QuantityWeight weight = new QuantityWeight(1000.0, WeightUnit.Gram);
            QuantityWeight result = weight.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(1.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void TestConversion_SameUnit()
        {
            QuantityWeight weight = new QuantityWeight(5.0, WeightUnit.Kilogram);
            QuantityWeight result = weight.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            QuantityWeight weight = new QuantityWeight(0.0, WeightUnit.Kilogram);
            QuantityWeight result = weight.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(0.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            QuantityWeight weight = new QuantityWeight(-1.0, WeightUnit.Kilogram);
            QuantityWeight result = weight.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(-1000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            QuantityWeight original = new QuantityWeight(1.5, WeightUnit.Kilogram);
            QuantityWeight toGrams = original.ConvertTo(WeightUnit.Gram);
            QuantityWeight backToKg = toGrams.ConvertTo(WeightUnit.Kilogram);

            Assert.AreEqual(original.Value, backToKg.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, backToKg.Unit);
        }
    }
}