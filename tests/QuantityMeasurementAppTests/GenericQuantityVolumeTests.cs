using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementAppTests
{
    [TestClass]
    public class GenericQuantityVolumeTests
    {
        private const double Epsilon = 1e-6;

        [TestMethod]
        public void TestEquality_LitreToLitre_SameValue()
        {
            Quantity<VolumeUnit> first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> second = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            bool areEqual = first.Equals(second);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_LitreToLitre_DifferentValue()
        {
            Quantity<VolumeUnit> first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> second = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

            bool areEqual = first.Equals(second);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_LitreToMillilitre_EquivalentValue()
        {
            Quantity<VolumeUnit> litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> milli = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            bool areEqual = litre.Equals(milli);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_MillilitreToLitre_EquivalentValue()
        {
            Quantity<VolumeUnit> milli = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            Quantity<VolumeUnit> litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            bool areEqual = milli.Equals(litre);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_LitreToGallon_EquivalentValue()
        {
            Quantity<VolumeUnit> litre = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);
            Quantity<VolumeUnit> gallon = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);

            bool areEqual = litre.Equals(gallon);

            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void TestEquality_VolumeVsLength_Incompatible()
        {
            Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<LengthUnit> length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            bool areEqual = volume.Equals(length);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestEquality_VolumeVsWeight_Incompatible()
        {
            Quantity<VolumeUnit> volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<WeightUnit> weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            bool areEqual = volume.Equals(weight);

            Assert.IsFalse(areEqual);
        }

        [TestMethod]
        public void TestConversion_LitreToMillilitre()
        {
            Quantity<VolumeUnit> litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> milli = litre.ConvertTo(VolumeUnit.Millilitre);

            Assert.AreEqual(1000.0, milli.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Millilitre, milli.Unit);
        }

        [TestMethod]
        public void TestConversion_MillilitreToLitre()
        {
            Quantity<VolumeUnit> milli = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            Quantity<VolumeUnit> litre = milli.ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(1.0, litre.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, litre.Unit);
        }

        [TestMethod]
        public void TestConversion_GallonToLitre()
        {
            Quantity<VolumeUnit> gallon = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);
            Quantity<VolumeUnit> litre = gallon.ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(3.78541, litre.Value, 1e-4);
            Assert.AreEqual(VolumeUnit.Litre, litre.Unit);
        }

        [TestMethod]
        public void TestConversion_LitreToGallon()
        {
            Quantity<VolumeUnit> litre = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);
            Quantity<VolumeUnit> gallon = litre.ConvertTo(VolumeUnit.Gallon);

            Assert.AreEqual(1.0, gallon.Value, 1e-4);
            Assert.AreEqual(VolumeUnit.Gallon, gallon.Unit);
        }

        [TestMethod]
        public void TestConversion_RoundTrip()
        {
            Quantity<VolumeUnit> original = new Quantity<VolumeUnit>(1.5, VolumeUnit.Litre);
            Quantity<VolumeUnit> toMilli = original.ConvertTo(VolumeUnit.Millilitre);
            Quantity<VolumeUnit> backToLitre = toMilli.ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(original.Value, backToLitre.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, backToLitre.Unit);
        }

        [TestMethod]
        public void TestAddition_SameUnit_LitrePlusLitre()
        {
            Quantity<VolumeUnit> first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> second = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

            Quantity<VolumeUnit> result = first.Add(second);

            Assert.AreEqual(3.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        [TestMethod]
        public void TestAddition_CrossUnit_LitrePlusMillilitre()
        {
            Quantity<VolumeUnit> first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> second = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Quantity<VolumeUnit> result = first.Add(second);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Millilitre()
        {
            Quantity<VolumeUnit> first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            Quantity<VolumeUnit> second = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Quantity<VolumeUnit> result = first.Add(second, VolumeUnit.Millilitre);

            Assert.AreEqual(2000.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Millilitre, result.Unit);
        }

        [TestMethod]
        public void TestAddition_ExplicitTargetUnit_Gallon()
        {
            Quantity<VolumeUnit> first = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);
            Quantity<VolumeUnit> second = new Quantity<VolumeUnit>(3.78541, VolumeUnit.Litre);

            Quantity<VolumeUnit> result = first.Add(second, VolumeUnit.Gallon);

            Assert.AreEqual(2.0, result.Value, 1e-4);
            Assert.AreEqual(VolumeUnit.Gallon, result.Unit);
        }
    }
}