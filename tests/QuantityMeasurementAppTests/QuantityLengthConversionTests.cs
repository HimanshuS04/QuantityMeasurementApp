using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityLengthAdditionTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestAddition_SameUnit_FeetPlusFeet()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(2.0, LengthUnit.Feet);

            QuantityLength result = QuantityLength.Add(first, second);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestAddition_SameUnit_InchPlusInch()
        {
            QuantityLength first = new QuantityLength(6.0, LengthUnit.Inch);
            QuantityLength second = new QuantityLength(6.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second);

            Assert.AreEqual(12.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_FeetPlusInches()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_InchPlusFeet()
        {
            QuantityLength first = new QuantityLength(12.0, LengthUnit.Inch);
            QuantityLength second = new QuantityLength(1.0, LengthUnit.Feet);

            QuantityLength result = QuantityLength.Add(first, second);

            Assert.AreEqual(24.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_YardPlusFeet()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength second = new QuantityLength(3.0, LengthUnit.Feet);

            QuantityLength result = QuantityLength.Add(first, second);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yard, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_CentimeterPlusInch()
        {
            QuantityLength first = new QuantityLength(2.54, LengthUnit.Centimeter);
            QuantityLength second = new QuantityLength(1.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second);

            Assert.AreEqual(5.08, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Centimeter, result.Unit);
        }

        [TestMethod]
        public void TestAddition_Commutativity_WithExplicitResultUnit()
        {
            QuantityLength sum1 = QuantityLength.Add(1.0, LengthUnit.Feet, 12.0, LengthUnit.Inch, LengthUnit.Feet);
            QuantityLength sum2 = QuantityLength.Add(12.0, LengthUnit.Inch, 1.0, LengthUnit.Feet, LengthUnit.Feet);

            Assert.AreEqual(sum1.Value, sum2.Value, Epsilon);
            Assert.AreEqual(sum1.Unit, sum2.Unit);
        }

        [TestMethod]
        public void TestAddition_WithZero()
        {
            QuantityLength result = QuantityLength.Add(5.0, LengthUnit.Feet, 0.0, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(5.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestAddition_NegativeValues()
        {
            QuantityLength result = QuantityLength.Add(5.0, LengthUnit.Feet, -2.0, LengthUnit.Feet, LengthUnit.Feet);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestAddition_NullSecondOperand_Throws()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = null;

            try
            {
                QuantityLength _ = QuantityLength.Add(first, second);
                Assert.Fail("Expected ArgumentNullException for null second operand was not thrown.");
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void TestAddition_InvalidUnit_Throws()
        {
            try
            {
                QuantityLength _ = QuantityLength.Add(1.0, (LengthUnit)999, 1.0, LengthUnit.Feet, LengthUnit.Feet);
                Assert.Fail("Expected ArgumentOutOfRangeException for invalid first unit was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                QuantityLength _ = QuantityLength.Add(1.0, LengthUnit.Feet, 1.0, (LengthUnit)999, LengthUnit.Feet);
                Assert.Fail("Expected ArgumentOutOfRangeException for invalid second unit was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                QuantityLength _ = QuantityLength.Add(1.0, LengthUnit.Feet, 1.0, LengthUnit.Feet, (LengthUnit)999);
                Assert.Fail("Expected ArgumentOutOfRangeException for invalid result unit was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void TestAddition_LargeValues()
        {
            QuantityLength result = QuantityLength.Add(1e6, LengthUnit.Feet, 1e6, LengthUnit.Feet, LengthUnit.Feet);

            Assert.AreEqual(2e6, result.Value, 1e-1);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestAddition_SmallValues()
        {
            QuantityLength result = QuantityLength.Add(0.001, LengthUnit.Feet, 0.002, LengthUnit.Feet, LengthUnit.Feet);

            Assert.AreEqual(0.003, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        // UC7 explicit target unit tests using QuantityLength operands

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Feet()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second, LengthUnit.Feet);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Inches()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second, LengthUnit.Inch);

            Assert.AreEqual(24.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, result.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Yards()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second, LengthUnit.Yard);

            Assert.AreEqual(2.0 / 3.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yard, result.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Centimeters()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Inch);
            QuantityLength second = new QuantityLength(1.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second, LengthUnit.Centimeter);

            Assert.AreEqual(5.08, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Centimeter, result.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Commutativity()
        {
            QuantityLength first = new QuantityLength(1.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(12.0, LengthUnit.Inch);

            QuantityLength result1 = QuantityLength.Add(first, second, LengthUnit.Yard);
            QuantityLength result2 = QuantityLength.Add(second, first, LengthUnit.Yard);

            Assert.AreEqual(result1.Value, result2.Value, Epsilon);
            Assert.AreEqual(result1.Unit, result2.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_WithZero()
        {
            QuantityLength first = new QuantityLength(5.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(0.0, LengthUnit.Inch);

            QuantityLength result = QuantityLength.Add(first, second, LengthUnit.Yard);

            Assert.AreEqual(5.0 / 3.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Yard, result.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_NegativeValues()
        {
            QuantityLength first = new QuantityLength(5.0, LengthUnit.Feet);
            QuantityLength second = new QuantityLength(-2.0, LengthUnit.Feet);

            QuantityLength result = QuantityLength.Add(first, second, LengthUnit.Inch);

            Assert.AreEqual(36.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, result.Unit);
        }
    }
}