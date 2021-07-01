using Microsoft.AspNetCore.Authorization;
using RestaurantApp.Core.Entity;
using RestaurantApp.Core.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.Web.Authorization
{
    public enum OperationType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class GeneralAuthorization : IAuthorizationRequirement
    {
        public readonly OperationType operationType;
        public GeneralAuthorization(OperationType operationType)
        {
            this.operationType = operationType;
        }
    }

    public class GeneralAuthorizationHandler<TEntity> : AuthorizationHandler<GeneralAuthorization, TEntity> where TEntity : class, new()
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GeneralAuthorization requirement, TEntity resource)
        {
            var userId = context.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("id")).Value;
            var accountType = context.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("accountType")).Value;

            if (resource is Account)
            {
                var account = resource as Account;

                if(userId == account.Id.ToString())
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
