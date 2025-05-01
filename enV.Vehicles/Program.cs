using env.Vehicles.Infrastructure;
using enV.Vehicles.Services;
using LiteDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ILiteDatabase>(_ => new LiteDatabase("Filename=vehicles.db;Connection=shared"));
builder.Services.AddScoped<IQueryableDataStore, LiteDbQueryableDataStore>();
builder.Services.AddScoped<IBackingFileStore, LiteDbBackingFileStore>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IVehicleService, VehicleService>();

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
