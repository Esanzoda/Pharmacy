using Pharmacy.Infrastructure.Configurations.Extension;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddConfigurations(builder);
builder.Services.AddDatabase();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddHangfire();
builder.Services.AddConsumers(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddDependencyInjection();
builder.Services.AddSwagger();
builder.AddSeriaLogger();

var app = builder.Build();
app.AddJobs();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacy API"); });
}

app.AddUseApplication();
app.MapControllers();
app.Run();