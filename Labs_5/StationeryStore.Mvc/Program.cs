using StationeryStore.Mvc.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.Exception;
using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.Options;
using StationeryStore.Mvc.Mapper;
using StationeryStore.Mvc.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//add appsetting configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<StationeryStoreSettings>(builder.Configuration.GetSection("StationeryStoreSettings"));

//add mapper
var mapperConfig = new MapperConfiguration(mc =>

    mc.AddProfile(new StationeryMapper()),
    NullLoggerFactory.Instance
);
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//register database 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

//register repositories
builder.Services.AddScoped<IStationeryRepository, StationeryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStationeryOrderRepository, StationeryOrderRepository>();
//register services 
builder.Services.AddScoped<IStationeryService, StationeryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderStationeryService, OrderStationeryService>();
builder.Services.AddScoped<IDataHealthService, DataHealthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

//healthy checks
builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("Application is running."), tags: new[] { "live" })
                .AddDbContextCheck<AppDbContext>("database", tags: new[] { "ready" });
//config serilog and Ilogger
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/lab05-.txt", rollingInterval: RollingInterval.Day));

//add problem details
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] =
            context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["timestamp"] =
            DateTimeOffset.UtcNow;
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponse.WriteHtmlResponse
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});
app.MapGet("/api/stationeries/{id:int}", async (int id, AppDbContext db, HttpContext http) =>
{
    var stationery = await db.Stationeries.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
    if (stationery == null)
    {
        return Results.Problem(
            type: "https://example.com/problems/stationery-not-found",
            title: "Stationery not found",
            detail: $"The Stationery with id {id} was not found.",
            statusCode: StatusCodes.Status404NotFound,
            instance: http.Request.Path);
    }

    return Results.Ok(stationery);
});


app.Run();
