using CustomerListing.DB.Domain;
using CustomerListing.DB.Enums;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;
using System;

namespace CustomerListing.Tests.DB.Domain
{
    [TestFixture]
    public class TestCustomer
    {
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => new Customer());
            //---------------Test Result -----------------------
        }

        [TestCase("CustomerId", typeof(Guid))]
        [TestCase("FirstName", typeof(string))]
        [TestCase("LastName", typeof(string))]
        [TestCase("Email", typeof(string))]
        [TestCase("Cellphone", typeof(string))]
        [TestCase("AmountTotal", typeof(decimal))]
        [TestCase("Type", typeof(CustomerType))]
        public void Customer_ShouldHaveProperty(string propertyName, Type propertyType)
        {
            //---------------Set up test pack-------------------
            var sut = typeof(Customer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            sut.ShouldHaveProperty(propertyName, propertyType);
            //---------------Test Result -----------------------
        }
    }
}
