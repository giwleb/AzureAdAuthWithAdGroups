using Microsoft.AspNetCore.Authorization;

namespace AzureAdAuthWithAdGroups
{
    public class GroupAuthorizationRequirementOld:IAuthorizationRequirement
    {
        public string GroupIdDev { get; } = string.Empty;
        public string GroupIdTest { get; } = string.Empty;
        public string GroupIdQa { get; } = string.Empty;
        public string GroupIdProd { get; } = string.Empty;


        public GroupAuthorizationRequirementOld(string groupIdDev, string groupIdTest, string groupIdQa, string groupIdProd)
        {
            GroupIdDev = groupIdDev;
            GroupIdTest = groupIdTest;
            GroupIdQa = groupIdQa;
            GroupIdProd = groupIdProd;
        }
    }
}
