using AutoMapper;
using CustomerListing.Persistence.Helpers;
using CustomerListing.Persistence.Interfaces.Services;
using CustomerListing.Persistence.V1.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomerListing.Server.Controllers.V1
{
    public class ControllerBase : Controller
    {
        public readonly IUriService _uriService;
        public readonly IMapper _mapper;
        private IDateTimeProvider _dateTimeProvider;

        public ControllerBase(IMapper mapper, IUriService uriService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _uriService = uriService ?? throw new ArgumentNullException(nameof(uriService));
        }

        public IDateTimeProvider DateTimeProvider
        {
            get { return _dateTimeProvider ??= new DefaultDateTimeProvider(); }
            set
            {
                if (_dateTimeProvider != null) throw new InvalidOperationException("DateTimeProvider is already set");
                _dateTimeProvider = value;
            }
        }

        public void SetDefaultFieldsFor(CreateCustomerRequest customerRequest)
        {
            if (customerRequest != null)
            {
                customerRequest.CustomerId = Guid.NewGuid();
                customerRequest.DateCreated = DateTimeProvider.Now;
                customerRequest.DateLastModified = DateTimeProvider.Now;
                customerRequest.CreatedBy = "SYS";
                customerRequest.LastUpdatedBy = "SYS";
            }
        }

        public void UpdateBaseFieldsOn(UpdateCustomerRequest request)
        {
            request.DateLastModified = DateTimeProvider.Now;
            request.LastUpdatedBy = "SYS";
        }
    }
}
