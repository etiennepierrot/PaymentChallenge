using System.Threading.Tasks;

namespace PaymentChallenge.Domain.Merchants
{
    public interface IAuthService
    {
        Task<bool> Authenticate(MerchantId merchantId , string passphrase);
    }
}