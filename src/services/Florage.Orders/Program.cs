using Florage.Orders.AsyncServices.Publishers;
using Florage.Orders.Contracts;
using Florage.Orders.Services;
using Florage.Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderPublishingService, OrderPublishingService>();
builder.Services.AddHttpContextAccessor();

PersistanceConfigurations.AddMongoDb(builder.Services);
AsyncMessagingConfigurations.AddRabbitMq(builder.Services, builder.Configuration);
SwaggerAuthorizationConfigurations.AddSwaggerAuth(builder.Services, builder.Configuration);
JwtConfiguration.AddJwtAuth(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/orders/docs/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "api/orders/docs";
    c.SwaggerEndpoint("/api/orders/docs/swagger/v1/swagger.json", "Florage Orders API");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
