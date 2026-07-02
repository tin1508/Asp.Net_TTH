using StationeryStore.Mvc.Data;
using Microsoft.EntityFrameworkCore;
using StationeryStore.Mvc.Services;
using StationeryStore.Mvc.Exception;
using StationeryStore.Mvc.Repositories;
using StationeryStore.Mvc.Options;  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//add appsetting configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<StationeryStoreSettings>(builder.Configuration.GetSection("StationeryStoreSettings"));

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



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
