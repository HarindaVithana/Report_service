using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using voyage_pro_report_service;
using voyage_pro_report_service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddRepositories();

builder.Services.AddCors(o => o.AddPolicy("Angular", p =>
    p.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDevExpressControls();
builder.Services.AddScoped<ReportStorageWebExtension, ReportStorage>();
builder.Services.AddScoped<IWebDocumentViewerReportResolver, ReportResolver>();
builder.Services.ConfigureReportingServices(configurator =>
{
    if (builder.Environment.IsDevelopment())
        configurator.UseDevelopmentMode();
    configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
    {
        viewerConfigurator.UseCachedReportSourceBuilder();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngularClient",
        policy =>
        {
            var allowedOrigins =
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? Array.Empty<string>();
            policy
                .WithOrigins(allowedOrigins) // Allow your Angular app
                .AllowAnyMethod() // Allow GET, POST, PUT, DELETE, etc.
                .AllowAnyHeader() // Allow any headers
                .AllowCredentials(); // Allow credentials if needed
        }
    );
});

var app = builder.Build();

app.UseCors("Angular");
app.UseDevExpressControls();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
