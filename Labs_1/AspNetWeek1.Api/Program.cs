using AspNetWeek1.Api.Models;
using AspNetWeek1.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Read config and environment
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Application: {builder.Environment.ApplicationName}");

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

//Middleware pipeline
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

// Minimal API endpoints
app.MapGet("/", () =>  Results.Ok(new {message = "AspNet Week 1 Mini Stationery Store API is running"}));
// app.MapGet("/")
app.MapGet("/env", () => Results.Ok(new 
{
    Environment = app.Environment.EnvironmentName,
}));
app.MapGet("/config", (IConfiguration config) => Results.Ok(new
{
    AppName = config["AppSettings:AppName"],
    Version = config["AppSettings:BaseUrl"]
}));

//Controller endpoints
app.MapControllers();

app.Run();