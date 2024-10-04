namespace DemoAPIProject.Controllers
{
    using DemoAPIProject.Constants;
    using DemoAPIProject.Entity;
    using DemoAPIProject.Model;
    using DemoAPIProject.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Controller responsible for URL shortening operations.
    /// </summary>
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Constructor for UrlShortenerController class.
        /// </summary>
        /// <param name="repository">The repository for URL mappings.</param>
        public UrlShortenerController(IRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Shortens a given long URL.
        /// </summary>
        /// <param name="shortenUrl">The object containing the long URL to shorten.</param>
        /// <returns>Returns an asynchronous action result with the shortened URL.</returns>
        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortenUrl shortenUrl)
        {
            var existingMapping = await _repository.GetUrlMappingByLongUrlAsync(shortenUrl.LongUrl);
            if (existingMapping != null)
            {
                return Ok(new { shortUrl = $"{Request.Scheme}://{Request.Host}/{existingMapping.ShortUrl}" });
            }

            var shortUrl = GenerateShortUrl(shortenUrl.LongUrl);
            var urlMapping = new UrlMapping
            {
                LongUrl = shortenUrl.LongUrl,
                ShortUrl = shortUrl,
                VisitCount = 0,
                CreatedBy = shortenUrl.CreatedBy,
            };

            await _repository.CreateUrlMappingAsync(urlMapping);

            return Ok(new { shortUrl = $"{Request.Scheme}://{Request.Host}/{shortUrl}" });
        }

        /// <summary>
        /// Redirects to the long URL associated with the given short URL.
        /// </summary>
        /// <param name="shortUrl">The short URL to redirect from.</param>
        /// <returns>Returns an asynchronous action result for redirecting to the long URL.</returns>
        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> RedirectToLongUrl(string shortUrl)
        {
            var urlMapping = await _repository.GetUrlByShortUrlAsync(shortUrl);

            if (urlMapping == null)
            {
                return NotFound(ErrorMessages.UrlNotFound);
            }

            await _repository.UpdateUrlMappingAsync(shortUrl);

            return Redirect(urlMapping.LongUrl);
        }

        /// <summary>
        /// Generates a short URL based on the given long URL.
        /// </summary>
        /// <param name="longUrl">The long URL to generate a short URL for.</param>
        /// <returns>Returns the generated short URL.</returns>
        private string GenerateShortUrl(string longUrl)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
                string base64Hash = Convert.ToBase64String(hashBytes);
                return base64Hash.Substring(0, 6).Replace("+", "").Replace("/", "").Replace("=", ""); // Cleaning the string
            }
        }
    }
}