using System.Linq;
using System.Web.Mvc;
using GestorMapeos.Parameters.PlantillaAsignaturaIntegracion;
using GestorMapeos.Globalization.Services;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Services.Gestor;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Controllers
{
    [Authorize]
    public class PlantillaAsignaturaIntegracionController : Controller
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
    }
}