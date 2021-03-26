using System.Threading.Tasks;
using ExoAuthEtRoleSolution.Models.ExoAuthEtRoleSolution.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ExoAuthEtRoleSolution.Authorization
{
    public class VetementAdminAuthorizationHandler
                  : AuthorizationHandler<OperationAuthorizationRequirement, Vetement>
    {
        protected override Task HandleRequirementAsync(
                                    AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement,
                                    Vetement resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }
            
            if (context.User.IsInRole(Constants.VetementAdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}