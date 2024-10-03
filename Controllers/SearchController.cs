using microservice_search_ads.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace microservice_search_ads.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class SearchController : ControllerBase
        {
            private readonly SearchDbContext _context;
            private readonly MessageService _messageService;

            public SearchController(SearchDbContext context, MessageService messageService)
            {
                _context = context;
                _messageService = messageService;
            }

            [HttpGet]
            public async Task<IActionResult> Get([FromQuery] string Title, [FromQuery] string Description, [FromQuery] decimal Price)
            {
                var query = _context.AdModels.AsQueryable();

                if (!string.IsNullOrEmpty(Title))
                {
                    query = query.Where(q => q.Title.Contains(Title));
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    query = query.Where(q => q.Description.Contains(Description));
                }

                var results = await query.ToListAsync();

                _messageService.SendLoggingActions("User searched for: " + Title);

                return Ok(results);
            }
        }

    }

