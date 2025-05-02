using env.Vehicles.Infrastructure.Storage;
using enV.Vehicles.Services;
using LiteDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ILiteDatabase>(var => new LiteDatabase("Filename=vehicles.db;Connection=shared"));
builder.Services.AddScoped<IQueryableDataStore<env.Vehicles.Infrastructure.Models.Vehicle>, VehicleDataStore> ();
builder.Services.AddScoped<IBackingFileStore, LiteDbBackingFileStore>();

builder.Services.AddHttpClient("nhtsa", client =>
{
    client.BaseAddress = new Uri("https://vpic.nhtsa.dot.gov");

    client.DefaultRequestHeaders.Add("Accept", "application/json");

});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleAugmentingService, VehicleAugmentingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
