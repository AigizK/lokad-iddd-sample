using System;
using NUnit.Framework;
using Sample.Domain;

namespace Sample
{
    public class when_rename_customer : customer_specs
    {
        [Test]
        public void given_matching_name()
        {
            Given = new IEvent[]
                {
                    new CustomerCreated()
                        {
                            Id = new CustomerId(1),
                            Currency = Currency.Eur,
                            Name = "Lokad"
                        }, 
                };
            When = c => c.Rename("Lokad", DateTime.UtcNow);
            Then = NoEvents;
        }

        [Test]
        public void given_different_name()
        {
            Given = new IEvent[]
                {
                    new CustomerCreated()
                        {
                            Id = new CustomerId(1),
                            Currency = Currency.Eur,
                            Name = "Lokad"
                        },
                };
            
            When = c => c.Rename("Lokad SAS", new DateTime(2012,07,16));
            Then = new IEvent[]
                {
                    new CustomerRenamed()
                        {
                            Id = new CustomerId(1),
                            Name = "Lokad SAS",
                            OldName = "Lokad",
                            Renamed = new DateTime(2012,07,16)
                        }
                };
        }
    }
}