using PizzaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register controllers, API documentation services, and the in-memory service for the controller-based architecture.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PizzaService>();

var app = builder.Build();

// Expose Swagger UI at the application root so the browser immediately shows documentation.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza API V1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
