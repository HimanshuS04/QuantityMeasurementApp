using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class GenericQuantityLengthTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestGenericLength_Equality_FeetAndInches_Equivalent()
        {
            Quantity<LengthUnit> feetQuantity = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            Quantity<LengthUnit> inchQuantity = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            bool areEqual = feetQuantity.Equals(inchQuantity);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestGenericLength_Conversion_FeetToInches()
        {
            Quantity<LengthUnit> feetQuantity = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            Quantity<LengthUnit> inchQuantity = feetQuantity.ConvertTo(LengthUnit.Inch);

            Assert.AreEqual(12.0, inchQuantity.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, inchQuantity.Unit);
        }

        [TestMethod]
        public void TestGenericLength_Addition_CrossUnit_ResultFeet()
        {
            Quantity<LengthUnit> feetQuantity = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            Quantity<LengthUnit> inchQuantity = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            Quantity<LengthUnit> result = feetQuantity.Add(inchQuantity, LengthUnit.Feet);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestGenericLength_Addition_CrossUnit_ResultInches()
        {
            Quantity<LengthUnit> feetQuantity = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            Quantity<LengthUnit> inchQuantity = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            Quantity<LengthUnit> result = feetQuantity.Add(inchQuantity, LengthUnit.Inch);

            Assert.AreEqual(24.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, result.Unit);
        }
    }
}