using System.Linq;
using System.Web.Mvc;
using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using GestorMapeos.Parameters.PlanOfertado;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Services.Gestor;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Controllers
{
    [Authorize]
    public class PeriodoEstudioIntegracionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult PopupPeriodosEstudio()
        {
            return View("~/Views/PeriodoEstudioIntegracion/NavPeriodoEstudio/PopupPeriodoEstudio.cshtml");
        }
        public ActionResult PopupPlanesOfertados()
        {
            return View("~/Views/PeriodoEstudioIntegracion/NavPlanOfertado/PopupPlanOfertado.cshtml");
        }
    }
}