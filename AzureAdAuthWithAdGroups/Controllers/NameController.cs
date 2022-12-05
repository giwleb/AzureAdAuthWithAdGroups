using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Text.Json.Nodes;

namespace AzureAdAuthWithAdGroups.Controllers
{
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : ControllerBase
    {
        [Authorize(Policy = "FimUserPolicy")]
        [HttpGet("[action]")]
        public IActionResult GetInt()
        {
            return Ok(new List<int> { 33, 4323 });
        }

        
        
        
        [Authorize(Policy = "FimDisbursementEntryGroupPolicy")]
        [HttpGet("[action]")]
        public IActionResult GetName()
        {
            return Ok(new List<string> { "bob", "joe" });
        }




        [Authorize(Policy = "FimCorporateAdminGroupPolicy")]
        [HttpGet("[action]")]
        public IActionResult GetDouble()
        {
            return Ok(new List<double> { 33.3333, 4323.222 });
        }




        [Authorize(Policy = "FimAllPermissionsGroupPolicy")]
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            return Ok(new List<string> { "ALL!" });
        }      


        [Authorize(Policy = "RequireDaemonRolePolicy")]
        [HttpGet("[action]")]
        public IActionResult GetConsole()
        {
            return Ok(new JsonArray(new JsonObject
            {
                ["employeesList"] = new JsonArray("Aman", "Priyank", "Tejas", "Raj")
            }));
        }
    }
}
