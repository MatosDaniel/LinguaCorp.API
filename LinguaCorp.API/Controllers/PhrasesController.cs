using LinguaCorp.API.Models;
using LinguaCorp.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;


namespace LinguaCorp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhrasesController : Controller
    {
        private readonly IPhraseService _phraseService;

        //logging service dependency injection:      
        private readonly ILogger<PhrasesController> _logger;


        //Add configuration dependency injection to access the value of the configuration
        private readonly IConfiguration _configuration;


        // The service is automatically injected via dependency injection
        public PhrasesController(ILogger<PhrasesController> logger, IPhraseService phraseService, IConfiguration configuration)
        {
            _logger = logger;
            _phraseService = phraseService;
            _configuration = configuration;
        }


        /// <summary>
        /// Retrieves all stored phrases.
        /// </summary>
        /// <returns>List of phrases or 204 if empty.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllPhrases([FromHeader(Name = "X-API-KEY")] string apiKey)
        {
            try
            {
                var configuredKey = _configuration["ApiSettings:ApiKey"];
                if (apiKey != configuredKey)
                {
                    return Unauthorized("Invalid API key.");
                }

                var phrases = _phraseService.GetAllPhrases();

                if (phrases.Count == 0)
                {
                    return NoContent();
                }

                return Ok(phrases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all phrases.");
                return StatusCode(500, "An error occurred while retrieving phrases.");
            }

        }


        /// <summary>
        /// Retrieves a specific phrase by its unique identifier.
        /// </summary>
        /// <param name="id">Phrase ID</param>
        /// <returns>Returns the phrase if found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPhraseById(int id, [FromHeader(Name = "X-API-KEY")] string apiKey)
        {


            _logger.LogInformation("Request received to get phrase with ID {Id}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID {Id} provided", id);
                return BadRequest("ID must be a positive integer.");
            }

            try
            {
                var configuredKey = _configuration["ApiSettings:ApiKey"];
                if (apiKey != configuredKey)
                {
                    return Unauthorized("Invalid API key.");
                }

                var phrase = _phraseService.GetPhraseById(id);

                _logger.LogInformation("Phrase with ID {Id} retrieved successfully", id);
                return Ok(phrase);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving phrase with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the phrase.");
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody] Phrase phrase)
        {
            //This verifies if the Model is filled correctly. If not, the error message that was defined is returned
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var created = _phraseService.CreatePhrase(phrase);

            _logger.LogInformation("Created phrase with ID {Id}", created.Id);

            return CreatedAtAction(nameof(GetPhraseById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePhrase(int id, [FromBody] Phrase updatedPhrase)
        {
            //This verifies if the Model is filled correctly. If not, the error message that was defined is returned
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _phraseService.UpdatePhrase(id, updatedPhrase);
            if (!success)
            {
                return NotFound($"Phrase with ID {id} not found.");
            }

            _logger.LogInformation("Request received to update phrase with ID {Id}", id);
            return Ok(updatedPhrase);

        }

        [HttpDelete("{id}")]
        public IActionResult DeletePhrase(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be a positive integer.");
            }

            var success = _phraseService.DeletePhrase(id);

            if (!success)
            {
                return NotFound($"Phrase with ID {id} not found.");
            }

            _logger.LogInformation("Deleted phrase with ID {Id}", id);
            return NoContent();
        }
    }
}
