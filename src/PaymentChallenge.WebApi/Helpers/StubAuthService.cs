using System.Threading.Tasks;
using PaymentChallenge.Domain.Merchants;

namespace PaymentChallenge.WebApi.Helpers
{
    //TODO : we need to implement a real authService
    public class StubAuthService : IAuthService
    {
        public Task<bool> Authenticate(MerchantId merchantId, string passphrase)
        {
            return Task.FromResult(true);
        }
    }
}