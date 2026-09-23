using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using Microsoft.AspNetCore.Mvc;

namespace voyage_pro_report_service.Controllers
{
    public class DocumentViewerController : WebDocumentViewerController
    {
        public DocumentViewerController(IWebDocumentViewerMvcControllerService service) : base(service) { }
    }
}
