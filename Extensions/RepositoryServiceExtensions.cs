using DevExpress.XtraReports.Web.Extensions;
using voyage_pro_report_service.Interfaces.IRepository;
using voyage_pro_report_service.Interfaces.IServices;
using voyage_pro_report_service.Interfaces.Shared;
using voyage_pro_report_service.Reports.Quotation;
using voyage_pro_report_service.Repositories;
using voyage_pro_report_service.Services;

namespace voyage_pro_report_service.Extensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IReportDataRepository, ReportDataRepository>();

            services.AddScoped<IReportDataBinder, QuotationReportBinder>();

            services.AddScoped<ReportStorageWebExtension, ReportStorage>();

            return services;
        }
    }
}
