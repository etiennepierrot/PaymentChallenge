using System.Collections.Generic;
using PaymentChallenge.Domain;
using PaymentChallenge.Domain.Shoppers;

namespace PaymentChallenge.Tests
{
    public class InMemoryShopperRepository : ShopperRepository
    {
        private static Dictionary<ShopperId, Shopper> _databag = new Dictionary<ShopperId, Shopper>();
        public Shopper Get(ShopperId shopperId)
        {
            return _databag[shopperId];
        }

        public void Save(Shopper shopper)
        {
            _databag.Add(shopper.Id, shopper);
        }
    }
}