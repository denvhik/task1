using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task1.Data;
using task1.DTO;
using task1.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace task1.Controllers
{
    [Route("/url")]
    public class UrlController : Controller
    {
        private readonly UrlShortenerContext _context;


        public UrlController(UrlShortenerContext context)
        {
            _context = context;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Urls = await _context.Url.Where(x => x.UserId == currentUserId && !x.IsDeleted).ToListAsync();
            return View(Urls);
        }

        [Route("~/{urlhash}")]
        [HttpGet]
        public async Task<IActionResult> GetUrl([FromRoute] string urlhash)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var url = await _context.Url.FirstOrDefaultAsync(x => x.ShortUrl.Contains(urlhash) && x.UserId == currentUserId && !x.IsDeleted);

            if (url is null)
                return NotFound();

            return Redirect(url.URL);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create( CreateUrlDto urlDto)
        {
            if (ModelState.IsValid)
            {
                var urlToCreate = new Url()
                {
                    URL = urlDto.URL,
                    ShortUrl = CreateShortUrl(urlDto.URL),
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                await _context.Url.AddAsync(urlToCreate);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View();
        }

        private string CreateShortUrl(string url)
        {
            var urlHash = string.Empty;
            using (var md5Hasher = MD5.Create())
            {
                var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(url));
                urlHash = BitConverter.ToString(data).Replace("-", "");
            };

            return $"{Request.Scheme}://{Request.Host.Value}/{urlHash}";
        }

        //[Authorize("Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUrl([FromRoute] int id)
        {
            var url = await _context.Url.FindAsync(id);
            if (url is null)
                return BadRequest();

            _context.Remove(url.Id);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}
