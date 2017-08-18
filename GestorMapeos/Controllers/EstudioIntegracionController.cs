using System.Linq;
using System.Web.Mvc;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Erp20.Entities;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Model.Gestor.Entities;
using GestorMapeos.Models.Services.Gestor;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Controllers
{
    [Authorize]
    public class EstudioIntegracionController : Controller
    {
        // GET: EstudioIntegracion
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