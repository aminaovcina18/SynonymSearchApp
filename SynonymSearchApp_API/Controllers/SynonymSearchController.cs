using Microsoft.AspNetCore.Mvc;
using SynonymSearchApp_ApplicationCore.IServices;
using SynonymSearchApp_Domain.Models;

namespace SynonymSearchApp_API.Controllers
{
    [Route("api/synonym")]
    [ApiController]
    public class SynonymSearchController : ControllerBase
    {
        private readonly ISynonymSearchService _synonymSearchService;
        public SynonymSearchController(ISynonymSearchService synonymSearchService)
        {
            _synonymSearchService = synonymSearchService;
        }
        [HttpGet("{key}")]
        public IActionResult GetSynonymList(string key)
        {
            return Ok(_synonymSearchService.GetSynonymList(key));
        }
        [HttpPost]
        public IActionResult Add([FromBody] Synonym request)
        {
            return Ok(_synonymSearchService.AddSynonym(request));
        }
    }
}
