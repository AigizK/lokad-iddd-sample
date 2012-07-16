using System;
using NUnit.Framework;
using Sample.Domain;

namespace Sample.CustomerAggregate
{
    /// <summary>
    /// Unit tests for <see cref="Customer.Charge"/>
    /// </summary>
    public class WhenChargeCustomer : customer_specs
    {
        [Test]
        public void given_non_existent_customer()
        {
            Given = NoEvents;

            When = c => c.Charge("charge", 1m.Eur(), DateTime.UtcNow);

            ThenEx = ex => ex.Message == "Customer currency was not assigned!";
        }
        [Test]
        public void given_existing_customer_with_balance()
        {
            Given = new IEvent[]
                {
                    new CustomerCreated()
                        {
                            Id = new CustomerId(2),
                            Name = "Microsoft",
                            Currency = Currency.Eur
                        },
                    new CustomerPaymentAdded()
                        {
                            Id = new CustomerId(2),
                            NewBalance = 1000m.Eur(),
                            Payment = 1000m.Eur(),
                            PaymentName = "Bonus",
                            Transaction = 1
                        },
                };
            When = c => c.Charge("Sales forecast fee", 200m.Eur(), new DateTime(2012, 3, 2));
            Then = new IEvent[]
                {
                    new CustomerChargeAdded()
                        {
                            Charge = 200m.Eur(),
                            ChargeName = "Sales forecast fee",
                            Id = new CustomerId(2),
                            NewBalance = 800m.Eur(),
                            TimeUtc = new DateTime(2012, 3, 2),
                            Transaction = 2
                        }, 
                };
        }
    }
}