using Microsoft.AspNetCore.Authorization;

namespace AzureAdAuthWithAdGroups
{
    public class GroupAuthorizationRequirement:IAuthorizationRequirement
    {
        public List<string> GroupIds { get; } = new List<string>();

        public GroupAuthorizationRequirement(List<string> groupIds)
        {
            GroupIds = groupIds;
        }
    }
}
