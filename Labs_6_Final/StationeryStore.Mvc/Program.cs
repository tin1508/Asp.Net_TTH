using Serilog;
using StationeryStore.Mvc.Data;
using StationeryStore.Mvc.Exception;
using StationeryStore.Mvc.Configuration;
using StationeryStore.Mvc.Options;
using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);


//add appsetting configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<StationeryStoreSettings>(builder.Configuration.GetSection("StationeryStoreSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews();

//add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
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

//add google authentication
builder.Services.AddGoogleAuthentication(builder.Configuration);
//configure password
builder.Services.AddIdentityConfigurations();
//auhorize
builder.Services.AddAppAuthorization();

//data
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//App layers
builder.Services.AddHttpContextAccessor();
builder.Services.AddAppRepositories();
builder.Services.AddAppServices();
builder.Services.AddAppMapper();
builder.Services.AddAppHealthChecks();

// Logging
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/lab05-.txt", rollingInterval: RollingInterval.Day));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapAppHealthChecks();
app.MapStationeryApiEndpoints();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeedingService>();
    await seeder.SeedAsync();
}

app.Run();
