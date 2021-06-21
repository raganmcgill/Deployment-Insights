using DeploymentInsights.Models;
using DeploymentInsights.Portal.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace DeploymentInsights.Portal.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IDataProvider _data_provider;

        public TeamController(IDataProvider data_provider)
        {
            _data_provider = data_provider;
        }

        [HttpGet("{team_code}")]
        public IActionResult Get(string team_code)
        {
            var team = _data_provider.GetTeam(team_code);

            if (team != null)
            {
                return Ok(team);
            }

            return NotFound();
        }


        [HttpDelete("{team_code}")]
        public IActionResult Delete(string team_code)
        {
            _data_provider.DeleteTeam(team_code);

            return Ok();
        }


        [HttpPost]
        public IActionResult UploadTeam([FromBody] Team team)
        {
            _data_provider.SaveTeam(team);

            return Ok(team);
        }

    }
}
