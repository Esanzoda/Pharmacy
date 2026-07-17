using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Pharmasy.Data;
using Pharmasy.Repositories;
using Pharmasy.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Pharmasy.Consumers;
using Pharmasy.Jobs;
using Pharmasy.Middlewares;
using Serilog;using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pharmasy.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(sp.GetRequiredService<AuditableInterceptor>());
});

builder.Services.AddScoped<AuditableInterceptor>();

//redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Pharmacy";
});

//mediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
    

//Hangfire
builder.Services.AddHangfire(static (_, cfg) =>
{
    cfg
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UsePostgreSqlStorage(options =>
        {
            options.UseNpgsqlConnection(
                "Host=localhost;Port=5432;Database=Pharmacy;Username=postgres;Password=1234");
            //(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
}).AddHangfireServer();

//MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer(typeof(OrderCreatedConsumer));
    x.AddConsumer(typeof(OrderCanselledConsumer));
    x.AddConsumer(typeof(OrderCompletedConsumer));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", hostConfigure =>
        {
            hostConfigure.Username("guest");
            hostConfigure.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File("logs/pharmacy-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT Token (dont need 'Bearer')"
    });

    
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document), 
            new List<string>()
        }
    });
});
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ChekExpiraDateProduct>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();

builder.Services.AddScoped<IDeliverRepository, DeliverRepository>();
builder.Services.AddScoped<IRefreshToken,RefreshTokenRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IApplicationDbContext, AppDbContext>();

var app = builder.Build();

//Job
using (var scope = app.Services.CreateScope())
{
    var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJob.AddOrUpdate<ChekExpiraDateProduct>(
        "check-expiry-data-products",
        job => job.ChekExpiraDateAsync(24),
        Cron.Daily());
    recurringJob.AddOrUpdate<Report>(
        "report-to-ceo",
        job => job.ReportToCeo(24),
        Cron.Daily());
}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy API"); });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


// documentatsiya + prezentatsiya
// maqsadi project chi  , ki istifoda mebarad 
// chi problemaya reshat mkunad
// kodi navictageta fam
//Task<Category> SearchByNameAsync(string name)å