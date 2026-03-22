using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class GenericQuantityArithmeticTests
    {
        private const double Epsilon = 1e-6;

        // Length subtraction

        [TestMethod]
        public void TestSubtraction_Length_SameUnit_FeetMinusFeet()
        {
            Quantity<LengthUnit> first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            Quantity<LengthUnit> second = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            Quantity<LengthUnit> result = first.Subtract(second);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestSubtraction_Length_CrossUnit_FeetMinusInches()
        {
            Quantity<LengthUnit> feet = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            Quantity<LengthUnit> inches = new Quantity<LengthUnit>(6.0, LengthUnit.Inch);

            Quantity<LengthUnit> result = feet.Subtract(inches, LengthUnit.Feet);

            Assert.AreEqual(9.5, result.Value, 1e-4);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestSubtraction_Length_ResultZero()
        {
            Quantity<LengthUnit> feet = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            Quantity<LengthUnit> inches = new Quantity<LengthUnit>(120.0, LengthUnit.Inch);

            Quantity<LengthUnit> result = feet.Subtract(inches, LengthUnit.Feet);

            Assert.AreEqual(0.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        // Weight subtraction

        [TestMethod]
        public void TestSubtraction_Weight_KilogramMinusGram()
        {
            Quantity<WeightUnit> kg = new Quantity<WeightUnit>(10.0, WeightUnit.Kilogram);
            Quantity<WeightUnit> g = new Quantity<WeightUnit>(5000.0, WeightUnit.Gram);

            Quantity<WeightUnit> result = kg.Subtract(g, WeightUnit.Kilogram);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        // Volume subtraction

        [TestMethod]
        public void TestSubtraction_Volume_LitreMinusMillilitre()
        {
            Quantity<VolumeUnit> first = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> second = new Quantity<VolumeUnit>(500.0, VolumeUnit.Millilitre);

            Quantity<VolumeUnit> result = first.Subtract(second);

            Assert.AreEqual(4.5, result.Value, 1e-4);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        // Length division

        [TestMethod]
        public void TestDivision_Length_SameUnit()
        {
            Quantity<LengthUnit> first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            Quantity<LengthUnit> second = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            double ratio = first.Divide(second);

            Assert.AreEqual(5.0, ratio, Epsilon);
        }

        [TestMethod]
        public void TestDivision_Length_CrossUnit()
        {
            Quantity<LengthUnit> inches = new Quantity<LengthUnit>(24.0, LengthUnit.Inch);
            Quantity<LengthUnit> feet = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            double ratio = inches.Divide(feet);

            Assert.AreEqual(1.0, ratio, Epsilon);
        }

        [TestMethod]
        public void TestDivision_Weight_KilogramByGram()
        {
            Quantity<WeightUnit> kg = new Quantity<WeightUnit>(2.0, WeightUnit.Kilogram);
            Quantity<WeightUnit> g = new Quantity<WeightUnit>(2000.0, WeightUnit.Gram);

            double ratio = kg.Divide(g);

            Assert.AreEqual(1.0, ratio, Epsilon);
        }

        [TestMethod]
        public void TestDivision_ByZero_Throws()
        {
            Quantity<LengthUnit> first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            Quantity<LengthUnit> zero = new Quantity<LengthUnit>(0.0, LengthUnit.Feet);

            try
            {
                double _ = first.Divide(zero);
                Assert.Fail("Expected DivideByZeroException was not thrown.");
            }
            catch (DivideByZeroException)
            {
            }
        }
    }
}