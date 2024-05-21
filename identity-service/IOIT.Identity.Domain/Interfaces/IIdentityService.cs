using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetAccountNameAsync(string accountId);
        Task<(IResult Result, string AccountId)> CreateAccountAsync(string userName,string password);
        Task<IResult> DeleteUserAsync(string userId);
    }
}
