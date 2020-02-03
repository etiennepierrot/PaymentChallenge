using Xunit;
using FluentAssertions;
using PaymentChallenge.Domain;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Shoppers;

namespace PaymentChallenge.Tests
{
    public class ShopperRegistrationTest
    {
        private readonly Merchant _merchant;
        private readonly ShopperRepository _shopperRepository;
        private readonly IdGenerator _idGenerator;
        private readonly ShopperRegistrationService _shopperRegistrationService;

        public ShopperRegistrationTest()
        {
            _merchant = new Merchant("FancyShop");
            _shopperRepository = new InMemoryShopperRepository();
            _idGenerator = new FakeIdGenerator();
            _shopperRegistrationService = new ShopperRegistrationService(_shopperRepository, _idGenerator);
        }

        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given a Shopper (John Doe)
        /// When the Merchant ask for registration
        /// Then the shopper should get a queryable shopper
        /// </summary>
        [Fact]
        public void Register_A_Shopper_On_Merchant()
        {
            Shopper shopper = _shopperRegistrationService.RegisterShopper(new RegisterChopper
            {
                FirstName = "John",
                LastName = "Doe",
                MerchantId = _merchant.Id,
            });

            _shopperRepository.Get(shopper.Id).Should()
                .BeEquivalentTo(new Shopper(
                    new ShopperId("FakeShopperId"),
                    "John", 
                    "Doe", 
                    new MerchantId("FancyShop") ));
        }
    }
}