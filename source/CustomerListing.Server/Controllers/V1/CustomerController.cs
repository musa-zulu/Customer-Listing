using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CustomerListing.DB.Domain;
using CustomerListing.Persistence.Interfaces.Services;
using CustomerListing.Persistence.V1;
using CustomerListing.Persistence.V1.Requests;
using CustomerListing.Persistence.V1.Requests.Queries;
using CustomerListing.Persistence.V1.Responses;
using CustomerListing.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerListing.Server.Controllers.V1
{
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService, IMapper mapper, IUriService uriService) : base(mapper, uriService)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }


        [HttpGet(ApiRoutes.Customers.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);

            var customers = await _customerService.GetCustomersAsync(pagination);

            var customerResponse = _mapper.Map<List<CustomerResponse>>(customers);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<CustomerResponse>(customerResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, customerResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Customers.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);

            if (customer == null)
                return NotFound();

            var customerResponse = _mapper.Map<CustomerResponse>(customer);
            return Ok(new Response<CustomerResponse>(customerResponse));
        }

        [HttpPost(ApiRoutes.Customers.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest customerRequest)
        {
            SetDefaultFieldsFor(customerRequest);
 
            var customer = _mapper.Map<CreateCustomerRequest, Customer>(customerRequest);

            await _customerService.CreateCustomerAsync(customer);

            var locationUri = _uriService.GetCustomerUri(customer.CustomerId.ToString());
            return Created(locationUri, new Response<CustomerResponse>(_mapper.Map<CustomerResponse>(customer)));
        }

        [HttpPut(ApiRoutes.Customers.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerRequest request)
        {
            UpdateBaseFieldsOn(request);

            var customer = _mapper.Map<UpdateCustomerRequest, Customer>(request);

            var isUpdated = await _customerService.UpdateCustomerAsync(customer);

            if (isUpdated)
                return Ok(new Response<CustomerResponse>(_mapper.Map<CustomerResponse>(customer)));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Customers.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid customerId)
        {
            if (customerId == Guid.Empty)
                return NoContent();

            var deleted = await _customerService.DeleteCustomerAsync(customerId);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}
