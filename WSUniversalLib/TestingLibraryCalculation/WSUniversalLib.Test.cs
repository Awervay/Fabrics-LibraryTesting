using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WSUniversalLib;

namespace TestingLibraryCalculation
{
    [TestClass]
    public class WSUniversalLibTesting
    {
        [TestMethod]
        public void Checking_the_calculation_of_the_first_type_of_product()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = 45;
            float productType = 1;
            float materialType = 1;
            int expectedResult = 14895;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_the_calculation_of_the_second_type_of_product()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = 45;
            float productType = 2;
            float materialType = 1;
            int expectedResult = 33852;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_the_calculation_of_the_third_type_of_product()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = 45;
            float productType = 3;
            float materialType = 1;
            int expectedResult = 114148;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_the_calculation_of_the_Second_type_of_material()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = 45;
            float productType = 1;
            float materialType = 2;
            int expectedResult = 14868;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_the_calculation_of_a_non_existing_product_type()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = 45;
            float productType = 4;
            float materialType = 1;
            int expectedResult = -1;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_the_calculation_of_a_non_existing_material_type()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = 45;
            float productType = 1;
            float materialType = 3;
            int expectedResult = -1;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_with_a_negative_count()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = -1;
            float width = 20;
            float length = 45;
            float productType = 1;
            float materialType = 1;
            int expectedResult = -1;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_with_a_negative_width()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = -20;
            float length = 45;
            float productType = 1;
            float materialType = 1;
            int expectedResult = -1;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_with_a_negative_length()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = -45;
            float productType = 1;
            float materialType = 1;
            int expectedResult = -1;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_with_a_zero_value_length()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 20;
            float length = 0;
            float productType = 1;
            float materialType = 1;
            int expectedResult = -1;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_with_a_zero_value_width()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 0;
            float length = 10;
            float productType = 1;
            float materialType = 1;
            int expectedResult = -1;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Checking_with_fractional_values()
        {
            //Arrange
            Calculation calculation = new Calculation();
            int count = 15;
            float width = 2.5f;
            float length = 3.5f;
            float productType = 1;
            float materialType = 1;
            int expectedResult = 145;
            //Act
            int result = calculation.CalculateRawMaterial(count, width, length, productType, materialType);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

    }
}

