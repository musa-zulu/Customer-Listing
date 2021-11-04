using CustomerListing.DB.Domain;
using PeanutButter.RandomGenerators;
using System;

namespace CustomerListing.Tests.Common.Builders.Domain
{
    public class CustomerBuilder : GenericBuilder<CustomerBuilder, Customer>
    {
        public CustomerBuilder WithId(Guid id)
        {
            return WithProp(x => x.CustomerId = id);
        }
    }
}
