using CustomerListing.DB;
using CustomerListing.DB.Domain;
using CustomerListing.Persistence.Services;
using CustomerListing.Tests.Common.Builders.Domain;
using CustomerListing.Tests.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CustomerListing.Tests.Persistence.Services
{
    public class TestCustomerService
    {
        private FakeDbContext _db;
        [SetUp]
        public void SetUp()
        {
            _db = new FakeDbContext(Guid.NewGuid().ToString());
            _db.DbContext.Database.EnsureCreated();
        }

        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
                new CustomerService(Substitute.For<IApplicationDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public async Task GetCustomersAsync_GivenNoCustomerExist_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------            
            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.GetCustomersAsync();
            //---------------Test Result -----------------------
            Assert.IsEmpty(results);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public async Task GetCustomersAsync_GivenOneCustomerExist_ShouldReturnListWithThatCustomer()
        {
            //---------------Set up test pack-------------------
            var customer = CreateRandomCustomer(Guid.NewGuid());
            await _db.Add(customer);

            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.GetCustomersAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(customer.FirstName, results[0].FirstName);
            Assert.AreEqual(customer.LastName, results[0].LastName);
            Assert.AreEqual(customer.Email, results[0].Email);
            Assert.AreEqual(customer.Cellphone, results[0].Cellphone);
        }

        [Test]
        public async Task GetCustomersAsync_GivenTwoCustomersExist_ShouldReturnAListWithTwoCustomers()
        {
            //---------------Set up test pack-------------------
            var firstCustomer = CreateRandomCustomer(Guid.NewGuid());
            var secondCustomer = CreateRandomCustomer(Guid.NewGuid());

            await _db.Add(firstCustomer, secondCustomer);

            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.GetCustomersAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public async Task GetCustomersAsync_GivenManyCustomersExist_ShouldReturnAListWithAllCustomers()
        {
            //---------------Set up test pack-------------------
            var firstCustomer = CreateRandomCustomer(Guid.NewGuid());
            var secondCustomer = CreateRandomCustomer(Guid.NewGuid());
            var thirdCustomer = CreateRandomCustomer(Guid.NewGuid());
            var forthCustomer = CreateRandomCustomer(Guid.NewGuid());

            await _db.Add(firstCustomer, secondCustomer, thirdCustomer, forthCustomer);

            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.GetCustomersAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count);
        }

        [Test]
        public async Task CreateCustomerAsync_GivenACustomerExistOnDb_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------            
            var customer = CreateRandomCustomer(Guid.NewGuid());
            await _db.Add(customer);

            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.CreateCustomerAsync(customer);
            //---------------Test Result -----------------------
            Assert.IsFalse(results);
        }

        [Test]
        public async Task CreateCustomerAsync_GivenACustomer_ShouldAddCustomerToRepo()
        {
            //---------------Set up test pack-------------------
            var customer = CreateRandomCustomer(Guid.NewGuid()); ;

            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.CreateCustomerAsync(customer);
            //---------------Test Result -----------------------
            var customerFromRepo = await customerService.GetCustomerByIdAsync(customer.CustomerId);
            Assert.IsTrue(results);
            Assert.AreEqual(customerFromRepo.CustomerId, customer.CustomerId);
            Assert.AreEqual(customerFromRepo.FirstName, customer.FirstName);
            Assert.AreEqual(customerFromRepo.Email, customer.Email);
        }

        [Test]
        public async Task CreateCustomerAsync_GivenCustomerHasBeenSavedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var customer = CreateRandomCustomer(Guid.NewGuid());
            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.CreateCustomerAsync(customer);
            //---------------Test Result -----------------------            
            Assert.IsTrue(results);
        }

        [Test]
        public async Task GetCustomerByIdAsync_GivenNoCustomerExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------  
            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.GetCustomerByIdAsync(Guid.NewGuid());
            //---------------Test Result -----------------------
            Assert.IsNull(results);
        }

        [Test]
        public async Task GetCustomerByIdAsync_GivenCustomerExistInRepo_ShouldReturnThatCustomer()
        {
            //---------------Set up test pack-------------------
            var customer = CreateRandomCustomer(Guid.NewGuid());
            var customerService = CreateCustomerService();
            await _db.Add(customer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.GetCustomerByIdAsync(customer.CustomerId);
            //---------------Test Result -----------------------         
            Assert.AreEqual(results.FirstName, customer.FirstName);
            Assert.AreEqual(results.CustomerId, customer.CustomerId);
            Assert.AreEqual(results.Cellphone, customer.Cellphone);
            Assert.AreEqual(results.Email, customer.Email);
        }

        [Test]
        public async Task DeleteCustomerAsync_GivenNoCustomerExist_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------            
            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await customerService.DeleteCustomerAsync(Guid.Empty);
            //---------------Test Result -----------------------            
            Assert.IsFalse(results);
        }

        [Test]
        public async Task UpdateCustomerAsync_GivenCustomerExistInRepo_ShouldUpdateThatCustomer()
        {
            //---------------Set up test pack-------------------
            var customer = CreateRandomCustomer(Guid.NewGuid());
            await _db.Add(customer);
            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            customer.FirstName = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await customerService.UpdateCustomerAsync(customer);
            //---------------Test Result -----------------------         
            var customerFromRepo = await customerService.GetCustomerByIdAsync(customer.CustomerId);
            Assert.AreEqual(customerFromRepo.FirstName, "This has been updated");
        }

        [Test]
        public async Task UpdateCustomerAsync_GivenCustomerHasBeenUpdatedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var customer = CreateRandomCustomer(Guid.NewGuid());
            await _db.Add(customer);
            var customerService = CreateCustomerService();
            //---------------Assert Precondition----------------
            customer.FirstName = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await customerService.UpdateCustomerAsync(customer);
            //---------------Test Result -----------------------                     
            Assert.IsTrue(results);
        }

        private CustomerService CreateCustomerService()
        {
            return new CustomerService(_db.DbContext);
        }

        private static Customer CreateRandomCustomer(Guid id)
        {
            var customer = new CustomerBuilder().WithId(id).WithRandomProps().Build();
            return customer;
        }

        public void Dispose()
        {
            _db.DbContext.Database.EnsureDeleted();
            _db.DbContext.Dispose();
        }
    }
}
