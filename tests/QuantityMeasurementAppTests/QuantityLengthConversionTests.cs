using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class LengthUnitConversionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestConvertToBaseUnit_FeetToFeet()
        {
            double result = LengthUnit.Feet.ConvertToBaseUnit(5.0);
            Assert.AreEqual(5.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConvertToBaseUnit_InchesToFeet()
        {
            double result = LengthUnit.Inch.ConvertToBaseUnit(12.0);
            Assert.AreEqual(1.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConvertToBaseUnit_YardsToFeet()
        {
            double result = LengthUnit.Yard.ConvertToBaseUnit(1.0);
            Assert.AreEqual(3.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConvertToBaseUnit_CentimetersToFeet()
        {
            double result = LengthUnit.Centimeter.ConvertToBaseUnit(30.48);
            Assert.AreEqual(1.0, result, 1e-4);
        }

        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToFeet()
        {
            double result = LengthUnit.Feet.ConvertFromBaseUnit(2.0);
            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToInches()
        {
            double result = LengthUnit.Inch.ConvertFromBaseUnit(1.0);
            Assert.AreEqual(12.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToYards()
        {
            double result = LengthUnit.Yard.ConvertFromBaseUnit(3.0);
            Assert.AreEqual(1.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConvertFromBaseUnit_FeetToCentimeters()
        {
            double result = LengthUnit.Centimeter.ConvertFromBaseUnit(1.0);
            Assert.AreEqual(30.48, result, 1e-4);
        }
    }
}