using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.Entities.Identity;
using IOIT.Identity.Domain.Entities.Indentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Identity.Domain.Enum.DomainEnum;

namespace IOIT.Identity.Infrastructure.Identity.Managers
{
    public class IdentityAppUserStore : IAccountAsyncRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IUserAsyncRepository _accountRepository;

        //public IdentityAppUserStore(IUnitOfWork unitOfWork, IUserAsyncRepository accountRepository)
        //{
        //    _unitOfWork = unitOfWork;
        //    _accountRepository = accountRepository;
        //}
        public IdentityAppUserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public virtual async Task<AccountSecurityToken> CreateSecurityTokenAsync(Guid userId, string code, TokenTypes tokenType, DateTime expiredDate, string ip, CancellationToken cancellationToken)
        //{
        //    var user = await FindByIdAsync(userId.ToString(), cancellationToken);
        //    if (user == null) return null;

        //    var userToken = new AccountSecurityToken
        //    {
        //        UserId = userId,
        //        Value = code,
        //        TokenType = tokenType,
        //        ExpiredDate = expiredDate,
        //        RemoteIpAddress = ip,
        //        CreatedDate = DateTimeOffset.UtcNow,
        //    };

        //    //user.UserSecurityTokens.Add(userToken);
        //    await _unitOfWork.CommitChangesAsync();

        //    return userToken;
        //}

        //public virtual async Task<bool> UpdateSecurityTokens(Guid userId, string code, Guid updatedBy, bool status, CancellationToken cancellationToken)
        //{
        //    var user = await FindByIdAsync(userId.ToString(), cancellationToken);
        //    if (user == null) return false;

        //    //var token = user.UserSecurityTokens.FirstOrDefault(c => c.UserId == userId && c.Value == code);

        //    //if (token == null) return false;

        //    await _unitOfWork.CommitChangesAsync();
        //    return true;
        //}
        public async Task<User> CreateAsync(User account, CancellationToken cancellationToken)
        {
            //await _accountRepository.AddAsync(account);
            await _unitOfWork.CommitChangesAsync();

            return account;
        }

        public async Task<IdentityResult> DeleteAsync(User account, CancellationToken cancellationToken)
        {
            //_accountRepository.Delete(account);

            await _unitOfWork.CommitChangesAsync();

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        //public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken)
        //{
        //    var user = await _accountRepository.FindByEmailAsync(email, cancellationToken);

        //    return user;
        //}

        //public async Task<User> FindByReferralCodeAsync(string referralCode, CancellationToken cancellationToken)
        //{
        //    var user = await _accountRepository.FindByReferralCode(referralCode, cancellationToken);

        //    return user;
        //}

        //public async Task<User> FindByIdAsync(string accountId, CancellationToken cancellationToken)
        //{
        //    var user = await _accountRepository.GetByKeyAsync(Guid.Parse(accountId));

        //    return user;
        //}

        //public async Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken)
        //{
        //    var user = await _accountRepository.FindByNameAsync(userName, cancellationToken);

        //    return user;
        //}

        //public async Task<User> FindByUserCode(string userCode, CancellationToken cancellationToken)
        //{
        //    return await _accountRepository.FindByUserCode(userCode, cancellationToken);
        //}
        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Password);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.Phone);
        }

        public async Task<string> GetaccountIdAsync(User account, CancellationToken cancellationToken)
        {
            return await Task.FromResult(account.Id.ToString());
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await Task.FromResult(user.UserName);
        }

        public User HashPassword(User user, string password, CancellationToken cancellationToken)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            return user;
        }

        public bool VerifyPassword(User user, string password, CancellationToken cancellationToken)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;

            return Task.CompletedTask;
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.Phone = phoneNumber;

            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _accountRepository.Update(user);

            await _unitOfWork.CommitChangesAsync();

            return IdentityResult.Success;
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
