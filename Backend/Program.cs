using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Middleware;
using ProductManager.API.Services.Interfaces;
using ProductManager.API.Services;
using ProductManager.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext <AppDbContext> (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://localhost:7063")//("http://localhost:4200", "https://localhost:7063")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
                     
            });
    });
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
 }); 
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
app.UseMiddleware<LoggingMiddleware>();

app.UseRouting();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapHub<NotificationHub>("/notificationHub");

app.UseAuthorization();

app.MapControllers();

app.Run();
