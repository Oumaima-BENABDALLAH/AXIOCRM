using AXIOCRM.Application.Clients.Commands.CreateClient;
using AXIOCRM.Application.Clients.Commands.DeleteClient;
using AXIOCRM.Application.Clients.Commands.UpdateClient;
using AXIOCRM.Application.Clients.Queries;
using AXIOCRM.Application.EventScheduler.Commands.CreateEvent;
using AXIOCRM.Application.EventScheduler.Commands.DeleteEvent;
using AXIOCRM.Application.EventScheduler.Commands.UpdateEvent;
using AXIOCRM.Application.EventScheduler.Queries.GetAllEvents;
using AXIOCRM.Application.EventScheduler.Queries.GetEventsById;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Orders.Commands.CreateOrder;
using AXIOCRM.Application.Orders.Commands.DeleteOrder;
using AXIOCRM.Application.Orders.Commands.UpdateOrder;
using AXIOCRM.Application.Orders.Queries;
using AXIOCRM.Application.Products.Commands.CreateProduct;
using AXIOCRM.Application.Products.Commands.DeleteProduct;
using AXIOCRM.Application.Products.Commands.UpdateProduct;
using AXIOCRM.Application.Products.Queries;
using AXIOCRM.Application.Services;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Domain.Middleware;
using AXIOCRM.Infrastructure.Hubs;
using AXIOCRM.Infrastructure.Persistence;
using AXIOCRM.Infrastructure.Security;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductManager.API.AI.ChurnPrediction;
using ProductManager.API.AI.ChurnPrediction.Interfaces;
using ProductManager.API.AI.ChurnPrediction.Services;
using ProductManager.API.Services;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

#region DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region Services
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDeliveryMethod, DeliveryMethodService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IEventReminderJob, EventReminderJob>();
builder.Services.AddScoped <ICommercialEmailReminderJob, CommercialEmailReminderJob>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IBookingTaskService, BookingTaskService>();
builder.Services.AddScoped<ChurnFeatureBuilder>();
builder.Services.AddScoped<IChurnTrainingService, ChurnTrainingService>();
builder.Services.AddScoped<IChurnService, ChurnService>();
IdentityModelEventSource.ShowPII = true;
#endregion

#region CORS
builder.Services.AddScoped<CreateOrderCommandHandler>();
builder.Services.AddScoped<GetOrdersQueryHandler>();
builder.Services.AddScoped<GetOrderByIdQueryHandler>();
builder.Services.AddScoped<DeleteOrderCommandHandler>();
builder.Services.AddScoped<UpdateOrderCommandHandler>();

//Products
builder.Services.AddScoped<CreateProductCommandHandler>();
builder.Services.AddScoped<GetProductByIdQueryHandler>();
builder.Services.AddScoped<DeleteProductCommandHandler>();
builder.Services.AddScoped<UpdateProductCommandHandler>();
builder.Services.AddScoped<GetAllProductsQueryHandler>();

// client
builder.Services.AddScoped<CreateClientCommandHandler>();
builder.Services.AddScoped<GetClientByIdQueryHandler>();
builder.Services.AddScoped<DeleteClientCommandHandler>();
builder.Services.AddScoped<UpdateClientCommandHandler>();
builder.Services.AddScoped<GetClientsQueryHandler>();

// Scheduler
builder.Services.AddScoped<CreateEventCommandHandler>();
builder.Services.AddScoped<GetEventByIdQueryHandler>();
builder.Services.AddScoped<DeleteEventCommandHandler>();
builder.Services.AddScoped<UpdateEventCommandHandler>();
builder.Services.AddScoped<GetAllEventsQueryHandler>();


#endregion


#region Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = false,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero 
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("JWT");

            logger.LogError(context.Exception, " JWT FAILED");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("JWT");

            logger.LogWarning(" JWT CHALLENGE: {Error} - {Description}",
                context.Error, context.ErrorDescription);

            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("JWT");

            logger.LogInformation(" JWT VALIDATED");
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
       
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        },
    };
});

#endregion
#region SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:7063")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
#endregion
#region Controllers / Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
#endregion

#region Hangfire
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new Hangfire.SqlServer.SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.FromSeconds(15),
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        });
});

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 5;
});
#endregion

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider
        .GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<IEventReminderJob>(
        "event-reminder-job",
        job => job.CheckTodayEvents(),
        Cron.Minutely
    );

    recurringJobManager.AddOrUpdate<ICommercialEmailReminderJob>(
        "commercial-email-reminder",
        job => job.SendEmailReminders(),
        Cron.Minutely
    );
    recurringJobManager.AddOrUpdate<IChurnTrainingService>(
       "churn-training",
       x => x.TrainModelAsync(),
       Cron.Weekly
   );
}
#region Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();

app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("Cross-Origin-Opener-Policy");
    context.Response.Headers.Remove("Cross-Origin-Embedder-Policy");
    await next();
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire");

app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();
#endregion

#region Roles Seed
app.Lifetime.ApplicationStarted.Register(async () =>
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Client", "User", "Manager", "Commercial" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
});
#endregion

app.Run();
