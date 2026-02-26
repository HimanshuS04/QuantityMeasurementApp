using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityWeightAdditionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestAddition_SameUnit_KilogramPlusKilogram()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(2.0, WeightUnit.Kilogram);

            QuantityWeight result = QuantityWeight.Add(first, second);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_KilogramPlusGram()
        {
            QuantityWeight first = new QuantityWeight(1.0, WeightUnit.Kilogram);
            QuantityWeight second = new QuantityWeight(1000.0, WeightUnit.Gram);

            QuantityWeight result = QuantityWeight.Add(first, second);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_ExplicitTarget_Gram()
        {
            QuantityWeight result = QuantityWeight.Add(1.0, WeightUnit.Kilogram, 1000.0, WeightUnit.Gram, WeightUnit.Gram);

            Assert.AreEqual(2000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void TestAddition_WithZero()
        {
            QuantityWeight result = QuantityWeight.Add(5.0, WeightUnit.Kilogram, 0.0, WeightUnit.Gram, WeightUnit.Kilogram);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void TestAddition_NegativeValues()
        {
            QuantityWeight result = QuantityWeight.Add(5.0, WeightUnit.Kilogram, -2000.0, WeightUnit.Gram, WeightUnit.Kilogram);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void TestAddition_Commutativity_WithExplicitTarget()
        {
            QuantityWeight sum1 = QuantityWeight.Add(1.0, WeightUnit.Kilogram, 1000.0, WeightUnit.Gram, WeightUnit.Kilogram);
            QuantityWeight sum2 = QuantityWeight.Add(1000.0, WeightUnit.Gram, 1.0, WeightUnit.Kilogram, WeightUnit.Kilogram);

            Assert.AreEqual(sum1.Value, sum2.Value, Epsilon);
            Assert.AreEqual(sum1.Unit, sum2.Unit);
        }
    }
}