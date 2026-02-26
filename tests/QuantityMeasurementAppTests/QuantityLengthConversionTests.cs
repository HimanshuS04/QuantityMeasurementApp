using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class QuantityLengthConversionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestConversion_FeetToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(12.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_InchesToFeet()
        {
            double result = QuantityLength.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet);
            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_YardsToInches()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.Yard, LengthUnit.Inch);
            Assert.AreEqual(36.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_InchesToYards()
        {
            double result = QuantityLength.Convert(72.0, LengthUnit.Inch, LengthUnit.Yard);
            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_CentimetersToInches()
        {
            double result = QuantityLength.Convert(2.54, LengthUnit.Centimeter, LengthUnit.Inch);
            Assert.AreEqual(1.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_FeetToYard()
        {
            double result = QuantityLength.Convert(6.0, LengthUnit.Feet, LengthUnit.Yard);
            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_SameUnit_ReturnsSameValue()
        {
            double result = QuantityLength.Convert(5.0, LengthUnit.Feet, LengthUnit.Feet);
            Assert.AreEqual(5.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_ZeroValue()
        {
            double result = QuantityLength.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(0.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_NegativeValue()
        {
            double result = QuantityLength.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch);
            Assert.AreEqual(-12.0, result, Epsilon);
        }

        [TestMethod]
        public void TestConversion_RoundTrip_PreservesValue()
        {
            double original = 5.0;
            double inches = QuantityLength.Convert(original, LengthUnit.Feet, LengthUnit.Inch);
            double backToFeet = QuantityLength.Convert(inches, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(original, backToFeet, Epsilon);
        }

        [TestMethod]
        public void TestConversion_InvalidUnit_Throws()
        {
            try
            {
                double _ = QuantityLength.Convert(1.0, (LengthUnit)999, LengthUnit.Feet);
                Assert.Fail("Expected ArgumentOutOfRangeException for invalid source unit was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                double _ = QuantityLength.Convert(1.0, LengthUnit.Feet, (LengthUnit)999);
                Assert.Fail("Expected ArgumentOutOfRangeException for invalid target unit was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void TestConversion_NaNOrInfinite_Throws()
        {
            try
            {
                double _ = QuantityLength.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inch);
                Assert.Fail("Expected ArgumentException for NaN was not thrown.");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                double _ = QuantityLength.Convert(double.PositiveInfinity, LengthUnit.Feet, LengthUnit.Inch);
                Assert.Fail("Expected ArgumentException for positive infinity was not thrown.");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                double _ = QuantityLength.Convert(double.NegativeInfinity, LengthUnit.Feet, LengthUnit.Inch);
                Assert.Fail("Expected ArgumentException for negative infinity was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }
    }
}