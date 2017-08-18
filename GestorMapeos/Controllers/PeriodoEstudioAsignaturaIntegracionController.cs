using System.Linq;
using System.Web.Mvc;
using GestorMapeos.Parameters.AsignaturaOfertadaIntegracion;
using GestorMapeos.Parameters.PeriodoEstudioAsignaturaIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Services.Gestor;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Controllers
{
    [Authorize]
    public class PeriodoEstudioAsignaturaIntegracionController : Controller
    {
        // GET: AsignaturaOfertadaIntegracion
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
        public ActionResult PopupPeriodosEstudioAsignatura()
        {
            return View("~/Views/PeriodoEstudioAsignaturaIntegracion/NavPeriodoEstudioAsignatura/PopupPeriodosEstudioAsignatura.cshtml");
        }
        public ActionResult PopupAsignaturaOfertada()
        {
            return View("~/Views/PeriodoEstudioAsignaturaIntegracion/NavAsignaturaOfertada/PopupAsignaturaOfertada.cshtml");
        }
        
    }
}