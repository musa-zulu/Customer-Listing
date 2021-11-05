using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using CustomerListing.Tests.Common.Builders.Controllers;
using CustomerListing.Tests.Common.Builders.Domain;
using CustomerListing.Tests.Common.Builders.V1.Requests;
using CustomerListing.DB.Domain;
using CustomerListing.Persistence.Helpers;
using CustomerListing.Persistence.Interfaces.Services;
using CustomerListing.Persistence.V1.Requests;
using CustomerListing.Persistence.V1.Requests.Queries;
using CustomerListing.Persistence.V1.Responses;
using CustomerListing.Server.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomerListing.Server.MappingProfiles;

namespace CustomerListing.Tests.Server.Controllers
{
    [TestFixture]
    public class TestCustomerController
    {
        private static IMapper _mapper;

        public TestCustomerController()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new RequestToDomainProfile());
                    mc.AddProfile(new DomainToResponseProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }
        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
                new CustomerController(Substitute.For<ICustomerService>(), Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenICustomerServiceIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new CustomerController(null, Substitute.For<IMapper>(), Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("customerService", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIMapperIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new CustomerController(Substitute.For<ICustomerService>(), null, Substitute.For<IUriService>()));
            //---------------Test Result -----------------------
            Assert.AreEqual("mapper", ex.ParamName);
        }

        [Test]
        public void Construct_GivenIUriServiceIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() =>
                    new CustomerController(Substitute.For<ICustomerService>(), Substitute.For<IMapper>(), null));
            //---------------Test Result -----------------------
            Assert.AreEqual("uriService", ex.ParamName);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProvider_ShouldSetDateTimeProviderOnFirstCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateCustomersControllerBuilder().Build();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            controller.DateTimeProvider = dateTimeProvider;
            //---------------Test Result -----------------------
            Assert.AreSame(dateTimeProvider, controller.DateTimeProvider);
        }

        [Test]
        public void DateTimeProvider_GivenSetDateTimeProviderIsSet_ShouldThrowOnCall()
        {
            //---------------Set up test pack-------------------
            var controller = CreateCustomersControllerBuilder().Build();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var dateTimeProvider1 = Substitute.For<IDateTimeProvider>();
            controller.DateTimeProvider = dateTimeProvider;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<InvalidOperationException>(() => controller.DateTimeProvider = dateTimeProvider1);
            //---------------Test Result -----------------------
            Assert.AreEqual("DateTimeProvider is already set", ex.Message);
        }

        [Test]
        public void GetAll_ShouldHaveHttpGetAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(CustomerController)
                .GetMethod("GetAll");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpGetAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task GetAll_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------            
            var mappingEngine = Substitute.For<IMapper>();
            var paginationQuery = CreatePaginationQuery();          

            var controller = CreateCustomersControllerBuilder()
                .WithMapper(mappingEngine)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<PaginationFilter>(paginationQuery);
        }

        [Test]
        public async Task GetAll_ShouldReturnOkResultObject_WhenCustomerExist()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            List<Customer> customers = CreateCustomers(customer);
            var uriService = Substitute.For<IUriService>();
            var customerService = Substitute.For<ICustomerService>();
            var paginationQuery = CreatePaginationQuery();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            customerService.GetCustomersAsync(Arg.Any<PaginationFilter>()).Returns(customers);
            var controller = CreateCustomersControllerBuilder()
                                   .WithCustomerService(customerService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAll_ShouldReturnCountOfOne_WhenCustomerExist()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            List<Customer> customers = CreateCustomers(customer);
            var customerService = Substitute.For<ICustomerService>();
            var paginationQuery = CreatePaginationQuery();

            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            customerService.GetCustomersAsync(Arg.Any<PaginationFilter>()).Returns(customers);

            var controller = CreateCustomersControllerBuilder()
                                   .WithCustomerService(customerService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<CustomerResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(1, pagedResponse.Data.Count());
        }

        [Test]
        public async Task GetAll_ShouldReturnCustomer_WhenCustomerExist()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            List<Customer> customers = CreateCustomers(customer);
            var customerService = Substitute.For<ICustomerService>();
            var paginationQuery = CreatePaginationQuery();
            var uriService = Substitute.For<IUriService>();
            Uri uri = CreateUri();

            uriService.GetAllUri(Arg.Any<PaginationQuery>()).Returns(uri);
            customerService.GetCustomersAsync(Arg.Any<PaginationFilter>()).Returns(customers);

            var controller = CreateCustomersControllerBuilder()
                                   .WithCustomerService(customerService)
                                   .WithMapper(_mapper)
                                   .WithUriService(uriService)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.GetAll(paginationQuery) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as PagedResponse<CustomerResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(customer.FirstName, pagedResponse.Data.FirstOrDefault().FirstName);
        }

        [Test]
        public void Get_ShouldHaveHttpGetAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(CustomerController)
                .GetMethod("Get");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpGetAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Get_ShouldReturnOkResultObject_WhenCustomerExist()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            var customerId = customer.CustomerId;
            var customerService = Substitute.For<ICustomerService>();

            customerService.GetCustomerByIdAsync(customerId).Returns(customer);

            var controller = CreateCustomersControllerBuilder()
                                   .WithCustomerService(customerService)
                                   .WithMapper(_mapper)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Get(customerId) as OkObjectResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Get_ShouldReturnNotFound_WhenCustomerDoesNotExist()
        {
            //---------------Set up test pack-------------------
            var controller = CreateCustomersControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Get(Guid.Empty) as NotFoundResult;
            //---------------Test Result -----------------------            
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task Get_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            var customerId = customer.CustomerId;
            var customerService = Substitute.For<ICustomerService>();
            var mappingEngine = Substitute.For<IMapper>();

            customerService.GetCustomerByIdAsync(customerId).Returns(customer);

            var controller = CreateCustomersControllerBuilder()
                .WithMapper(mappingEngine)
                .WithCustomerService(customerService)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Get(customerId);
            //---------------Test Result -----------------------
            mappingEngine.Received(1).Map<CustomerResponse>(customer);
        }

        [Test]
        public async Task Get_ShouldReturnCustomer_WhenCustomerExist()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            var customerId = customer.CustomerId;
            var customerService = Substitute.For<ICustomerService>();
            customerService.GetCustomerByIdAsync(customerId).Returns(customer);

            var controller = CreateCustomersControllerBuilder()
                .WithMapper(_mapper)
                .WithCustomerService(customerService)
                .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Get(customerId) as OkObjectResult;
            //---------------Test Result -----------------------
            var pagedResponse = result.Value as Response<CustomerResponse>;
            Assert.IsNotNull(pagedResponse);
            Assert.AreEqual(customer.CustomerId, pagedResponse.Data.CustomerId);
            Assert.AreEqual(customer.FirstName, pagedResponse.Data.FirstName);
        }


        [Test]
        public void Create_ShouldHaveHttpPostAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(CustomerController)
                .GetMethod("Create");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPostAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Create_ShouldReturnStatusOf201_GivenACustomerHasBeenSaved()
        {
            //---------------Set up test pack-------------------      
            Uri uri = CreateUri();
            var customerRequest = CreateCustomerRequestBuilder.BuildRandom();
            var uriService = Substitute.For<IUriService>();

            uriService.GetCustomerUri(Arg.Any<string>()).Returns(uri);

            var controller = CreateCustomersControllerBuilder()
                                   .WithUriService(uriService)
                                   .WithMapper(_mapper)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Create(customerRequest) as CreatedResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
        }

        [Test]
        public async Task Create_ShouldSaveCustomer_GivenAValidCustomerObject()
        {
            //---------------Set up test pack-------------------      
            Uri uri = CreateUri();
            var customerRequest = CreateCustomerRequestBuilder.BuildRandom();
            var uriService = Substitute.For<IUriService>();
            uriService.GetCustomerUri(Arg.Any<string>()).Returns(uri);

            var controller = CreateCustomersControllerBuilder()
                                   .WithUriService(uriService)
                                   .WithMapper(_mapper)
                                   .Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = await controller.Create(customerRequest) as CreatedResult;
            //---------------Test Result -----------------------
            var createdCustomer = (result.Value as Response<CustomerResponse>).Data;

            Assert.AreEqual(customerRequest.FirstName, createdCustomer.FirstName);
            Assert.AreEqual(customerRequest.CustomerId, createdCustomer.CustomerId);
        }

        [Test]
        public void Update_ShouldHaveHttpPutAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(CustomerController)
                .GetMethod("Update");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpPutAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Update_ShouldReturnNotfound_GivenACustomerHasNotBeenUpdated()
        {
            //---------------Set up test pack-------------------
            var request = new UpdateCustomerRequest
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "BBB123"
            };
            var customer = CustomerBuilder.BuildRandom();
            var customerService = Substitute.For<ICustomerService>();
            await customerService.CreateCustomerAsync(customer);
            var controller = CreateCustomersControllerBuilder()
                .WithCustomerService(customerService)
                .WithMapper(_mapper).Build();
            //---------------Assert Precondition----------------      

            //---------------Execute Test ----------------------
            var result = await controller.Update(request) as NotFoundResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnOkStatus_GivenACustomerHasBeenUpdated()
        {
            //---------------Set up test pack-------------------
            var request = new UpdateCustomerRequest
            {
                CustomerId = Guid.NewGuid(),
                FirstName = "BBB123"
            };

            var customerService = Substitute.For<ICustomerService>();
            customerService.UpdateCustomerAsync(Arg.Any<Customer>()).Returns(true);
            var controller = CreateCustomersControllerBuilder()
                .WithCustomerService(customerService)
                .WithMapper(_mapper).Build();
            //---------------Assert Precondition----------------           

            //---------------Execute Test ----------------------
            var result = await controller.Update(request) as OkObjectResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void Delete_ShouldHaveHttpDeleteAttribute()
        {
            //---------------Set up test pack-------------------
            var methodInfo = typeof(CustomerController)
                .GetMethod("Delete");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(methodInfo);
            //---------------Execute Test ----------------------
            var httpPostAttribute = methodInfo.GetCustomAttribute<HttpDeleteAttribute>();
            //---------------Test Result -----------------------
            Assert.NotNull(httpPostAttribute);
        }

        [Test]
        public async Task Delete_ShouldReturnNoContent_GivenACustomerIdIsEmpty()
        {
            //---------------Set up test pack-------------------
            var controller = CreateCustomersControllerBuilder()
                                   .Build();
            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            var result = await controller.Delete(Guid.Empty) as NoContentResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNotFound_GivenACustomerHasNotBeenDeleted()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            var customerService = Substitute.For<ICustomerService>();
            customerService.GetCustomerByIdAsync(customer.CustomerId).Returns(customer);
            var controller = CreateCustomersControllerBuilder()
                                   .WithCustomerService(customerService)
                                   .Build();
            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            var result = await controller.Delete(customer.CustomerId) as NotFoundResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnNoContent_GivenACustomerHasBeenDeleted()
        {
            //---------------Set up test pack-------------------
            var customer = CustomerBuilder.BuildRandom();
            var customerService = Substitute.For<ICustomerService>();
            customerService.DeleteCustomerAsync(customer.CustomerId).Returns(true);
            var controller = CreateCustomersControllerBuilder()
                                   .WithCustomerService(customerService)
                                   .Build();
            //---------------Assert Precondition----------------            
            //---------------Execute Test ----------------------
            var result = await controller.Delete(customer.CustomerId) as NoContentResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        private static PaginationQuery CreatePaginationQuery()
        {
            return new PaginationQuery();
        }
        private static Uri CreateUri()
        {
            return new Uri("localhost:4000?pageNumber=1&pageSize=10");
        }
        private static List<Customer> CreateCustomers(Customer customer)
        {
            return new List<Customer>
            {
                customer
            };
        }

        private static CustomerControllerBuilder CreateCustomersControllerBuilder()
        {
            return new CustomerControllerBuilder();
        }
    }
}
