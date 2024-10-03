namespace microservice_search_ads.Model
{
    public class AdModel
    {
        public required int Id { get; set; }  // Primärnyckel (Primary Key)
        public required string Title { get; set; }  // Annonsens titel
        public required string Description { get; set; }  // Beskrivning av annonsen
        public required decimal Price { get; set; }  // Pris på produkten
        public required DateTime CreatedAt { get; set; } = DateTime.Now; // Annonsens skapad
    }
}

