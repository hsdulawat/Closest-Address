﻿using ClosestAddress.Controllers;
using ClosestAddress.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ClosestAddress.Tests.Controllers
{
    [TestClass]
    public class DistanceCalculatorControllerTest
    {
        [TestMethod]
        public void GetAddress_ShouldReturnResult()
        {
            DistanceCalculatorController controller = new DistanceCalculatorController();
            List<Address> result = controller.Get("37 Sharp Street, Newtown VIC 3220");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAddress_ShouldReturnFiveResult()
        {
            DistanceCalculatorController controller = new DistanceCalculatorController();
            List<Address> result = controller.Get("37 Sharp Street, Newtown VIC 3220");
            Assert.Equals(result.Count, 5);
        }

        [TestMethod]
        public void GetAddress_ShouldReturnNearestResult()
        {
            DistanceCalculatorController controller = new DistanceCalculatorController();
            List<Address> result = controller.Get("37 Sharp Street, Newtown VIC 3220");
            var expectedResult = result.OrderBy(x => x.KM);
            Assert.IsTrue(expectedResult.SequenceEqual(result));
        }
    }
}