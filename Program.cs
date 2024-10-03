
using microservice_search_ads.Service;
using microservice_search_ads;
using Microsoft.EntityFrameworkCore;


    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SearchDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("SearchDb")));
            // Add services to the container.
            builder.Services.AddScoped<AdService>();
            builder.Services.AddHostedService<MessageService>();

            builder.Services.AddSingleton(x =>
                x.GetServices<IHostedService>().OfType<MessageService>().First()
            );

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

