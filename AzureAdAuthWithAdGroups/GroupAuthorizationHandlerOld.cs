using Microsoft.AspNetCore.Authorization;

namespace AzureAdAuthWithAdGroups
{
    public class GroupAuthorizationHandlerOld : AuthorizationHandler<GroupAuthorizationRequirementOld>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAuthorizationRequirementOld requirement)
        {
            var x = context.User.Claims;

            if (context.User.Claims.Any(c=>c.Type=="groups" && (c.Value==requirement.GroupIdDev || c.Value == requirement.GroupIdTest || c.Value == requirement.GroupIdQa || c.Value == requirement.GroupIdProd)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
