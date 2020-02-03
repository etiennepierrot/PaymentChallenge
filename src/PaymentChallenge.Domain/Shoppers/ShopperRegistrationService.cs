namespace PaymentChallenge.Domain.Shoppers
{
    public class ShopperRegistrationService
    {
        private readonly ShopperRepository _shopperRepository;
        private readonly IdGenerator _idGenerator;

        public ShopperRegistrationService(ShopperRepository shopperRepository, IdGenerator idGenerator)
        {
            _shopperRepository = shopperRepository;
            _idGenerator = idGenerator;
        }

        public Shopper RegisterShopper(RegisterChopper command)
        {
            ShopperId shopperId = _idGenerator.GenerateShopperId();
            Shopper shopper = new Shopper(shopperId, command.FirstName, command.LastName, command.MerchantId);
            _shopperRepository.Save(shopper);
            return shopper;
        }
    }
}