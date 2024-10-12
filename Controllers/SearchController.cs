using microservice_search_ads.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace microservice_search_ads.Controllers
{

        [Route("api")]
        [ApiController]
        public class SearchController : ControllerBase
        {
            private readonly SearchDbContext database;
            private readonly MessageService _messageService;

            public SearchController(SearchDbContext database, MessageService messageService)
            {
                this.database = database;
                _messageService = messageService;
            }

            [HttpGet]
            [Route("searchAd")]
            public IActionResult SearchForAd(string title)
            {
                var result = database.AdModels.Where(r => r.Title.Contains(title))
                    .Select(r => new ReturnAdDto {Title = r.Title, Description = r.Description, Price = r.Price})
                    .ToList();

                _messageService.SendLoggingActions("User searched for: " + title);

                return Ok(result);
            }
        }

    }

public class ReturnAdDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}
