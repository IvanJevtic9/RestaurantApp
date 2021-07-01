using Microsoft.AspNetCore.Identity;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.IdentityProvider;
using RestaurantApp.Core.Interface;
using RestaurantApp.Core.Model;
using RestaurantApp.Core.RepositoryInterface;
using System;

namespace RestaurantApp.Core.Manager
{
    public class AccountManager : IAccountManager<Account>
    {
        private readonly ILoggerAdapter<Account> logger;
        private readonly IPasswordHasher<Account> passwordHasher;
        private readonly IJwtProvider jwtProvider;
        private readonly IUnitOfWork unitOfWork;

        public ILoggerAdapter<Account> Logger { get => logger; }
        public IPasswordHasher<Account> PasswordHasher { get => passwordHasher; }
        public IJwtProvider JwtProvider { get => jwtProvider; }

        public AccountManager(ILoggerAdapter<Account> logger,
                              IPasswordHasher<Account> passwordHasher,
                              IUnitOfWork unitOfWork,
                              IJwtProvider jwtProvider)
        {
            this.logger = logger;
            this.passwordHasher = passwordHasher;
            this.unitOfWork = unitOfWork;
            this.jwtProvider = jwtProvider;
        }

        public Account GetByEmail(string email, string includeProperties = null)
        {
            return unitOfWork.Account.GetFirstOrDefault(a => a.Email == email, includeProperties);
        }

        public Account GetById(int accountId, string includeProperties = null)
        {
            return unitOfWork.Account.GetFirstOrDefault(a => a.Id == accountId, includeProperties);
        }

        public void Delete(Account account)
        {
            unitOfWork.Account.Remove(account);
            unitOfWork.SaveChanges();
            Logger.LogInformation($"Account with email {0} has been removed", account.Email);
        }

        public OperationResult CreateAccount(Account account, string password)
        {
            /*Here we should do some manual validation (AbstractValidatior)*/
            var op = new OperationResult() { Succeeded = true };

            try
            {
                var passwordHasher = PasswordHasher.HashPassword(account, password);
                account.PasswordHash = passwordHasher;

                unitOfWork.Account.Add(account);
                unitOfWork.SaveChanges();

                op.Succeeded = true;
            }
            catch (Exception e)
            {
                /*Missing error handling*/
                op.Succeeded = false;
                Logger.LogError(e, e.Message);
            }
            return op;
        }

        public bool CheckPassword(Account account, string password)
        {
            return PasswordHasher.VerifyHashedPassword(account, account.PasswordHash, password) == PasswordVerificationResult.Success;
        }

        public OperationResult ChangePassword(Account account, string oldPassword, string newPassword)
        {
            var op = new OperationResult() { Succeeded = false };

            if (CheckPassword(account, oldPassword))
            {
                account.PasswordHash = PasswordHasher.HashPassword(account, newPassword);
                unitOfWork.SaveChanges();
                op.Succeeded = true;
            }

            return op;
        }
    }
}
