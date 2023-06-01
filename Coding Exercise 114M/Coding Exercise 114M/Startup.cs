using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Data.SqlClient;

namespace Coding_Exercise_114M
{
    public class Startup
    {
        private readonly string connectionString;
        private const int MIN_WIDTH = 6;
        private const int MIN_HEIGHT = 4;
        private const int MAX_WIDTH = 20;
        private const int MAX_HEIGHT = 20;
        private const int MAX_RECTANGLES = 200;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("Database");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Coding_Exercise_114M", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coding_Exercise_114M v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedData();
        }

        private void SeedData()
        {
            Random random = new Random();

            using (var cnn = new SqlConnection(connectionString))
            {
                cnn.Execute("Delete From dbo.RectangleCoordinates");
                cnn.Execute("Delete From dbo.Rectangle");

                for (int i = 0; i < MAX_RECTANGLES; i++)
                {
                    int width = random.Next(MIN_WIDTH, MAX_WIDTH + 1);
                    int height = random.Next(MIN_HEIGHT, MAX_HEIGHT + 1);
                    int x = random.Next(0, 1000);
                    int y = random.Next(0, 1000);

                    Rectangle rectangle = new Rectangle()
                    {
                        Name = $"Rectangle {i + 1}",
                        Width = width,
                        Height = height
                    };

                    int rectangleId = cnn.QueryFirstOrDefault<int>(@"
                        Insert Into dbo.Rectangle (Name, Width, Height)
                        Output Inserted.Id
                        Values (@Name, @Width, @Height)", rectangle);

                    RectangleCoordinates coordinates = new RectangleCoordinates()
                    {
                        RectangleId = rectangleId,
                        X = x,
                        Y = y
                    };

                    cnn.Execute($@"Insert Into dbo.RectangleCoordinates (RectangleId, X, Y)
                        Values (@RectangleId, @X, @Y)", coordinates);
                }
            }
        }
    }
}
