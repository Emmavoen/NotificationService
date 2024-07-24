global using Serilog;
global using NotificationService.Persistence;
using NotificationService.Application.Configuration;
using NotificationService.Application.Contract.Service;
using NotificationService.Application.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, config) =>
{
    config.Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:dd-MMM-yyyy:HH:mm:ss} {Level:u3}] {Message}{NewLine}{Exception}")
        .ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDependencies(builder.Configuration);

var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
                                      .Get<EmailConfiguration>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton(emailConfig);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
