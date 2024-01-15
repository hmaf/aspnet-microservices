using Ordering.Api.Extensions;
using Ordering.Application;
using Ordering.Infrastracture;
using Ordering.Infrastracture.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastractureServices(builder.Configuration);

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

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<Context>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<ContextSeed>>();
    ContextSeed.Seed(context, logger).Wait();
});

app.Run();
