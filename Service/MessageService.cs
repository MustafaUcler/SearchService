using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace microservice_search_ads.Service
{
    public class MessageService : IHostedService
    {

        private IConnection connection;
        private IModel channel;
        private IServiceProvider provider;
        private HttpClient httpClient;

        public MessageService(IServiceProvider provider)
        {
            this.provider = provider;
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5118")
            };
        }

        // Anslut till RabbitMQ
        public void Connect()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare("logging", ExchangeType.Fanout);
        }

        // Anropas när programmet startas
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Connect();
            ListenForListingCreations();
            return Task.CompletedTask;
        }

        // Koppla bort när programmet stoppas
        public Task StopAsync(CancellationToken cancellationToken)
        {
            channel.Close();
            connection.Close();
            return Task.CompletedTask;
        }

        // Börja lyssna (consume) efter meddelanden från andra microservices
        void ListenForListingCreations()
        {
            // Skapa/referera till samma exchange som i "create"-servicen
            channel.ExchangeDeclare(exchange: "create-listing", type: ExchangeType.Fanout);

            // Skapa/referera till en queue som håller alla meddelanden som skickas
            var queueName = channel.QueueDeclare("listing", true, false, false);
            channel.QueueBind(queue: queueName, exchange: "create-listing", routingKey: string.Empty);

            // Skapa en metod som anropas när ett meddelande kommer in (listener)
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                try
                {
                    var ad = JsonSerializer.Deserialize<AdDTO>(json);
                    // Skicka vidare informationen till "AdService"
                    // så att datan kan sparas i databas
                    if (ad != null)
                    {
                        using (var scope = provider.CreateScope())
                        {
                            var adService = scope.ServiceProvider.GetRequiredService<AdService>();
                            adService.CreateAd(ad);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            };

            // Börja lyssna efter meddelanden (subscribe)
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        //Logging metod
        public void SendLoggingActions(string action)
        {
            var message = Encoding.UTF8.GetBytes(action);
            channel.BasicPublish("logging", string.Empty, null, message);
        }

        // Börja lyssna (consume) efter meddelanden från andra microservices
        void ListenForListingUpdate()
        {
            // Skapa/referera till samma exchange som i "update"-servicen
            channel.ExchangeDeclare(exchange: "updated-listing", type: ExchangeType.Fanout);

            // Skapa/referera till en queue som håller alla meddelanden som skickas
            var queueName = channel.QueueDeclare("listing", true, false, false);
            channel.QueueBind(queue: queueName, exchange: "updated-listing", routingKey: string.Empty);

            // Skapa en metod som anropas när ett meddelande kommer in (listener)
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var listingId = int.Parse(json);

                try
                {
                    //var ad = JsonSerializer.Deserialize<int>(json);
                    // Skicka vidare informationen till "AdService"
                    // så att datan kan sparas i databas
                    if (json != null)
                    {
                        using (var scope = provider.CreateScope())
                        {
                            var adService = scope.ServiceProvider.GetRequiredService<AdService>();
                            adService.DeleteAd(listingId);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            };

            // Börja lyssna efter meddelanden (subscribe)
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        // Hämta en annons från en annan microservice
        // genom att skicka ett sync HTTP anrop
    }

    // Hämta en annons från en annan microservice
    // genom att skicka ett sync HTTP anrop







    public class AdDTO
    {

        public required int Id { get; set; }  // Primärnyckel (Primary Key)
        public required string Title { get; set; }  // Annonsens titel
        public required string Description { get; set; }  // Beskrivning av annonsen
        public required decimal Price { get; set; }  // Pris på produkten
        public required string UserId { get; set; } //Användare Id (Primary Key)
        public required DateTime CreatedAt { get; set; } = DateTime.Now; //Annons skapad
    }
}

