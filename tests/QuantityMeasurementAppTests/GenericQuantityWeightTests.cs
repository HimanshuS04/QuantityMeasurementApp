using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class GenericQuantityWeightTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestGenericWeight_Equality_KilogramAndGram_Equivalent()
        {
            Quantity<WeightUnit> kilogramQuantity = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            Quantity<WeightUnit> gramQuantity = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            bool areEqual = kilogramQuantity.Equals(gramQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestGenericWeight_Conversion_KilogramToGram()
        {
            Quantity<WeightUnit> kilogramQuantity = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            Quantity<WeightUnit> gramQuantity = kilogramQuantity.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000.0, gramQuantity.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, gramQuantity.Unit);
        }

        [TestMethod]
        public void TestGenericWeight_Addition_CrossUnit_ResultKilogram()
        {
            Quantity<WeightUnit> kilogramQuantity = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            Quantity<WeightUnit> gramQuantity = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Quantity<WeightUnit> result = kilogramQuantity.Add(gramQuantity, WeightUnit.Kilogram);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void TestGenericWeight_Addition_CrossUnit_ResultGram()
        {
            Quantity<WeightUnit> kilogramQuantity = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            Quantity<WeightUnit> gramQuantity = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Quantity<WeightUnit> result = kilogramQuantity.Add(gramQuantity, WeightUnit.Gram);

            Assert.AreEqual(2000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }
    }
}