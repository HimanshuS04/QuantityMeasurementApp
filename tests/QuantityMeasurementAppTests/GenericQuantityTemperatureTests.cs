using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class GenericQuantityTemperatureTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestTemperatureEquality_CelsiusToCelsius_SameValue()
        {
            Quantity<TemperatureUnit> first = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> second = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);

            bool areEqual = first.Equals(second);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestTemperatureEquality_CelsiusToFahrenheit_Zero()
        {
            Quantity<TemperatureUnit> celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> fahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            bool areEqual = celsius.Equals(fahrenheit);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestTemperatureEquality_CelsiusToFahrenheit_Hundred()
        {
            Quantity<TemperatureUnit> celsius = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> fahrenheit = new Quantity<TemperatureUnit>(212.0, TemperatureUnit.Fahrenheit);

            bool areEqual = celsius.Equals(fahrenheit);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestTemperatureConversion_CelsiusToFahrenheit()
        {
            Quantity<TemperatureUnit> celsius = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> fahrenheit = celsius.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.AreEqual(212.0, fahrenheit.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Fahrenheit, fahrenheit.Unit);
        }

        [TestMethod]
        public void TestTemperatureConversion_FahrenheitToCelsius()
        {
            Quantity<TemperatureUnit> fahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);
            Quantity<TemperatureUnit> celsius = fahrenheit.ConvertTo(TemperatureUnit.Celsius);

            Assert.AreEqual(0.0, celsius.Value, Epsilon);
            Assert.AreEqual(TemperatureUnit.Celsius, celsius.Unit);
        }

        [TestMethod]
        public void TestTemperatureConversion_CelsiusToKelvin()
        {
            Quantity<TemperatureUnit> celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> kelvin = celsius.ConvertTo(TemperatureUnit.Kelvin);

            Assert.AreEqual(273.15, kelvin.Value, 1e-4);
            Assert.AreEqual(TemperatureUnit.Kelvin, kelvin.Unit);
        }

        [TestMethod]
        public void TestTemperatureConversion_KelvinToCelsius()
        {
            Quantity<TemperatureUnit> kelvin = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);
            Quantity<TemperatureUnit> celsius = kelvin.ConvertTo(TemperatureUnit.Celsius);

            Assert.AreEqual(0.0, celsius.Value, 1e-4);
            Assert.AreEqual(TemperatureUnit.Celsius, celsius.Unit);
        }

        [TestMethod]
        public void TestTemperatureUnsupported_Add_Throws()
        {
            Quantity<TemperatureUnit> first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> second = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            try
            {
                Quantity<TemperatureUnit> _ = first.Add(second);
                Assert.Fail("Expected NotSupportedException was not thrown for temperature addition.");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void TestTemperatureUnsupported_Subtract_Throws()
        {
            Quantity<TemperatureUnit> first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> second = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            try
            {
                Quantity<TemperatureUnit> _ = first.Subtract(second);
                Assert.Fail("Expected NotSupportedException was not thrown for temperature subtraction.");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void TestTemperatureUnsupported_Divide_Throws()
        {
            Quantity<TemperatureUnit> first = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            Quantity<TemperatureUnit> second = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            try
            {
                double _ = first.Divide(second);
                Assert.Fail("Expected NotSupportedException was not thrown for temperature division.");
            }
            catch (NotSupportedException)
            {
            }
        }

        [TestMethod]
        public void TestTemperatureVsLength_Incompatible()
        {
            Quantity<TemperatureUnit> temperature = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            Quantity<LengthUnit> length = new Quantity<LengthUnit>(100.0, LengthUnit.Feet);

            bool areEqual = temperature.Equals(length);

            Assert.IsFalse(areEqual);
        }
    }
}