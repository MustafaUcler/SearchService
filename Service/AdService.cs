using microservice_search_ads.Model;

namespace microservice_search_ads.Service
{
    public class AdService
    {
        private readonly SearchDbContext database;

        public AdService(SearchDbContext database)
        {
            this.database = database;
        }

        public void CreateAd(AdDTO ad)
        {
            AdModel newAd = new AdModel
            {
                Id = ad.Id,
                Title = ad.Title,
                Description = ad.Description,
                Price = ad.Price,

                CreatedAt = ad.CreatedAt
            };
            database.AdModels.Add(newAd);
            database.SaveChanges();
        }

        public void DeleteAd(int listingId)
        {
            var itemToDelete = database.AdModels.Where(i => i.Id == listingId).FirstOrDefault();

            database.AdModels.Remove(itemToDelete);
            database.SaveChanges();
        }
    }
}
