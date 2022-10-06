using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;

namespace AzureAdAuthWithAdGroups
{
    public class GroupAuthorizationHandler : AuthorizationHandler<GroupAuthorizationRequirement>
    {
        private readonly GraphServiceClient _graphServiceClient;

        public GroupAuthorizationHandler(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAuthorizationRequirement requirement)
        {
            bool hasGroupClaim = false;

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                if (context.User.Claims.Any(c => c.Type == "groups" && requirement.GroupIds.Any(y => y == c.Value)))
                {
                    hasGroupClaim = true;
                    context.Succeed(requirement);
                }

                if (!hasGroupClaim && context.User.Claims.Any(x => x.Type == "hasgroups" && x.Value == "true"))
                {
                    var groupsFromGraph = _graphServiceClient.Me.CheckMemberGroups(requirement.GroupIds).Request().PostAsync().Result;

                    if (groupsFromGraph.Any(x => requirement.GroupIds.Any(y => y == x)))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
