using Basket.Api.EventBusConsumer;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Discount.Grpc.Protos;
using EventBus.Message;
using EventBus.Message.Common;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddTransient<IBasketRepository, BasketRepository>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
    });

builder.Services.AddScoped<DiscountGrpcService>();

#region Add Config MassTransit RabbitMQ

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<HadiTestConsumer>();
    config.AddConsumer<HadiTestConsumer2>();

    config.UsingRabbitMq((ctx, conf) =>
    {
        conf.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
        
        conf.ReceiveEndpoint(EventBusConstants.HadiTestQueue, c =>
        {
            c.UseMessageRetry(r => r.Interval(2, 100)); // ????? Retry Policy
            c.ConfigureConsumer<HadiTestConsumer>(ctx);
        });
        conf.ReceiveEndpoint(EventBusConstants.HadiTestQueueTwo, c =>
        {
            c.UseMessageRetry(r => r.Interval(2, 100)); // ????? Retry Policy
            c.ConfigureConsumer<HadiTestConsumer2>(ctx);
        });
        
    });
});

builder.Services.AddScoped<HadiTestConsumer>();
builder.Services.AddScoped<HadiTestConsumer2>();

#endregion

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

app.UseAuthorization();

app.MapControllers();

app.Run();
