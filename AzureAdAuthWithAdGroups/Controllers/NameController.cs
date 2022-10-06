using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

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

        
        
        
        [Authorize(Policy = "FimDisbursementEntryPolicy")]
        [HttpGet("[action]")]
        public IActionResult GetName()
        {
            return Ok(new List<string> { "bob", "joe" });
        }




        [Authorize(Policy = "FimCorporateAdminPolicy")]
        [HttpGet("[action]")]
        public IActionResult GetDouble()
        {
            return Ok(new List<double> { 33.3333, 4323.222 });
        }




        [Authorize(Policy = "FimAllPermissionsPolicy")]
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            return Ok(new List<string> { "ALL!" });
        }
    }
}
