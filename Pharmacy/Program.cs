using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Pharmasy.Data;
using Pharmasy.Repositories;
using Pharmasy.Services;
using Microsoft.EntityFrameworkCore;
using Pharmasy.Consumers;
using Pharmasy.Jobs;
using Pharmasy.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File("logs/pharmacy-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseNpgsql("Host=localhost;Port=5432;Database=Pharmacy;Username=postgres;Password=1234;");
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ChekExpiraDateProduct>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

builder.Services.AddScoped<ICategoryServise, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();

builder.Services.AddScoped<ISupplierService, DeliverService>();
builder.Services.AddScoped<IDeliverRepository, DeliverRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<AuditableInterceptor>();

builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(sp.GetRequiredService<AuditableInterceptor>());
});
builder.Services.AddHangfire(static (provider, cfg) =>
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
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var recurringJob = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJob.AddOrUpdate<ChekExpiraDateProduct>(
        "dksjkfj",
        job => job.ChekExpiraDateAsync(24),
        Cron.Daily());
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy API"); });
}


app.UseMiddleware<ExseptionMiddleware>();
app.MapControllers();
app.Run();


// documentatsiya + prezentatsiya
// maqsadi project chi  , ki istifoda mebarad 
// chi problemaya reshat mkunad
// kodi navictageta fam
//Task<Category> SearchByNameAsync(string name)å