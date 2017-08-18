using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using GestorMapeos.Parameters.ErpCommons;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Erp20.Entities;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Parameters;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Controllers
{
    [Authorize]
    public class ErpCommonsController : Controller
    {
        // GET: ErpCommons
        private readonly ErpContext _erpContext;

        public ErpCommonsController(ErpContext erpContext)
        {
            _erpContext = erpContext;
        }
        public ActionResult SimpleSearchPlan(SimpleListPlanParameters parameters)
        {
            var transfer = new ClientTransfer();
            var plantillaQueriable = _erpContext.Plan.Where(p => true);
            if (parameters.IdPeriodoAcademico.HasValue)
                plantillaQueriable = plantillaQueriable.Where(p => p.PlanesOfertados
                    .Any(po => po.PeriodoAcademicoId == parameters.IdPeriodoAcademico.Value));
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                plantillaQueriable = plantillaQueriable.Where(p => (p.Codigo + " - " + p.Nombre).Contains(parameters.Descripcion));

            var selectQuery = plantillaQueriable.Select(r => new
            {
                r.Id,
                Descripcion = r.Codigo + " - " + r.Nombre,
                r.Anyo,
                r.EsOficial,
                Estudio = r.Estudio.CodigoRuct + " - " + r.Estudio.Nombre,
                TipoEstudio = r.Estudio.TipoEstudio.Nombre,
                Rama = r.Estudio.RamaConocimiento.Nombre,
                Titulo = r.Titulo.CodigoMec + " - " + r.Titulo.Nombre
            }).OrderBy(o => o.Descripcion);

            var listado = selectQuery
                .Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            var totalElements = selectQuery.Count();
            var totalpages = totalElements / parameters.ItemsPerPage;

            transfer.Data = listado;
            transfer.Pagination.TotalPages = totalpages + ((totalElements % parameters.ItemsPerPage) > 0 ? 1 : 0);
            transfer.Pagination.TotalRecords = totalElements; //Total de elementos segun filtro
            transfer.Pagination.TotalDisplayRecords = listado.Count; //Total de elementos segun pagina

            return Json(transfer);
        }
        public ActionResult SimpleSearchTitulos(SimpleListViewModel parameters)
        {
            var transfer = new ClientTransfer();
            var tituloQueriable = _erpContext.Titulo.Where(t => true);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                tituloQueriable = tituloQueriable.Where(p => (p.CodigoMec + " - " + p.Nombre).Contains(parameters.Descripcion));
            var selectQuery = tituloQueriable.Select(r => new
            {
                r.Id,
                r.CodigoMec,
                Descripcion = r.CodigoMec + " - " + r.Nombre,
                DisplayName = r.CodigoMec + " - " +r.Nombre,
            }).OrderBy(o => o.Descripcion);
            var listado = selectQuery
                .Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            var totalElements = selectQuery.Count();
            var totalpages = totalElements / parameters.ItemsPerPage;

            transfer.Data = listado;
            transfer.Pagination.TotalPages = totalpages + ((totalElements % parameters.ItemsPerPage) > 0 ? 1 : 0);
            transfer.Pagination.TotalRecords = totalElements; //Total de elementos segun filtro
            transfer.Pagination.TotalDisplayRecords = listado.Count; //Total de elementos segun pagina
            return Json(transfer);
        }
        public ActionResult SimpleSearchEspecializaciones(SimpleListEspecializacionViewModel parameters)
        {
            var transfer = new ClientTransfer();

            var plan = new Plan();
            if (parameters.FilterPlanEstudio.HasValue)
            {
                plan = _erpContext.Plan.Find(parameters.FilterPlanEstudio.Value);

                var especializacionQueriable =
                    _erpContext.Especializacion.Where(e => e.Titulo.Id == plan.Titulo.Id);
                
                if (!string.IsNullOrEmpty(parameters.Descripcion))
                    especializacionQueriable =
                        especializacionQueriable.Where(p => p.Nombre.Contains(parameters.Descripcion));


                var selectQuery = especializacionQueriable.Select(r => new
                {
                    r.Id,
                    Descripcion = r.Nombre
                }).OrderBy(o => o.Descripcion).ToList();
                var listado = selectQuery
                    .Skip((parameters.PageIndex - 1)*parameters.ItemsPerPage)
                    .Take(parameters.ItemsPerPage)
                    .ToList();
                var totalElements = selectQuery.Count();
                var totalpages = totalElements/parameters.ItemsPerPage;

                transfer.Data = listado;
                transfer.Pagination.TotalPages = totalpages + ((totalElements%parameters.ItemsPerPage) > 0 ? 1 : 0);
                transfer.Pagination.TotalRecords = totalElements; //Total de elementos segun filtro
                transfer.Pagination.TotalDisplayRecords = listado.Count; //Total de elementos segun pagina
            }
            else
            {
                transfer.Pagination.TotalPages = 0;
                transfer.Pagination.TotalRecords = 0;
                transfer.Pagination.TotalDisplayRecords = 0;
            }
            return Json(transfer);
        }
        public ActionResult SimpleSearchPeriodoAcademico(SimpleListViewModel parameters)
        {
            var transfer = new ClientTransfer();
            var periodoQueriable = _erpContext.PeriodosAcademicos.Where(t => true);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                periodoQueriable = periodoQueriable.Where(p => p.Nombre.Contains(parameters.Descripcion));

            var selectQuery = periodoQueriable.Select(r => new
            {
                r.Id,
                Descripcion = r.Nombre,
                AnyoAcademico = new
                {
                    DisplayName = r.AnyoAcademico.AnyoInicio + "/" + r.AnyoAcademico.AnyoFin
                },
                r.FechaInicio,
                r.FechaFin,
                r.Nro
            }).OrderBy(o => o.Descripcion);
            var tempListado = selectQuery
                .Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            var listado = tempListado.Select(a => new
            {
                a.Id,
                a.Descripcion,
                a.AnyoAcademico,
                FechaInicio = a.FechaInicio.ToString("dd/MM/yyyy"),
                FechaFin = a.FechaFin.ToString("dd/MM/yyyy"),
                a.Nro
            }).ToList();

            var totalElements = selectQuery.Count();
            var totalpages = totalElements / parameters.ItemsPerPage;

            transfer.Data = listado;
            transfer.Pagination.TotalPages = totalpages + ((totalElements % parameters.ItemsPerPage) > 0 ? 1 : 0);
            transfer.Pagination.TotalRecords = totalElements; //Total de elementos segun filtro
            transfer.Pagination.TotalDisplayRecords = listado.Count; //Total de elementos segun pagina
            return Json(transfer);
        }
        public ActionResult SimpleSearchAsignaturaPlan(SimpleListAsignaturaPlanParameters parameters)
        {
            var transfer = new ClientTransfer();
            var plantillaQueriable = _erpContext.AsignaturaPlan.Where(ap => true);
            if (parameters.IdPlanEstudio.HasValue)
                plantillaQueriable = plantillaQueriable.Where(ap => ap.PlanId == parameters.IdPlanEstudio.Value);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                plantillaQueriable = plantillaQueriable.Where(ap => (ap.Asignatura.Codigo + " - " + ap.Asignatura.Nombre).Contains(parameters.Descripcion));

            var selectQuery = plantillaQueriable.Select(ap => new
            {
                ap.Id,
                Descripcion = ap.Asignatura.Codigo + " - " + ap.Asignatura.Nombre,
                ap.Asignatura.Creditos,
                TipoAsignatura = ap.Asignatura.TipoAsignatura.Nombre,
                PlanEstudio = ap.Plan.Codigo + " - " + ap.Plan.Nombre,
                DuracionPeriodoLectivo = ap.DuracionPeriodoLectivo.Nombre,
                ap.UbicacionPeriodoLectivo,
                Curso = ap.CursoId.HasValue ? ap.Curso.Numero : (int?)null
            }).OrderBy(o => o.Descripcion);

            var listado = selectQuery
                .Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            var totalElements = selectQuery.Count();
            var totalpages = totalElements / parameters.ItemsPerPage;

            transfer.Data = listado;
            transfer.Pagination.TotalPages = totalpages + ((totalElements % parameters.ItemsPerPage) > 0 ? 1 : 0);
            transfer.Pagination.TotalRecords = totalElements; //Total de elementos segun filtro
            transfer.Pagination.TotalDisplayRecords = listado.Count; //Total de elementos segun pagina

            return Json(transfer);
        }
        public ActionResult SimpleSearchAsignaturaOfertada(SimpleListAsignaturaOfertadaParameters parameters)
        {
            var transfer = new ClientTransfer();
            var asignaturaQueriable = _erpContext.AsignaturaOfertada.Where(ao => true);
            if (parameters.IdPeriodoAcademico.HasValue)
                asignaturaQueriable = asignaturaQueriable.Where(ao => ao.PlanOfertado.PeriodoAcademicoId == parameters.IdPeriodoAcademico.Value);
            if (parameters.IdPlanEstudio.HasValue)
                asignaturaQueriable = asignaturaQueriable.Where(ao => ao.PlanOfertado.PlanId == parameters.IdPlanEstudio.Value);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                asignaturaQueriable = asignaturaQueriable.Where(ao => (ao.Codigo + " - " + ao.Nombre).Contains(parameters.Descripcion));

            var selectQuery = asignaturaQueriable.Select(ao => new
            {
                ao.Id,
                Descripcion = ao.Codigo + " - " + ao.Nombre
            }).OrderBy(o => o.Descripcion);

            var listado = selectQuery
                .Skip((parameters.PageIndex - 1) * parameters.ItemsPerPage)
                .Take(parameters.ItemsPerPage)
                .ToList();

            var totalElements = selectQuery.Count();
            var totalpages = totalElements / parameters.ItemsPerPage;

            transfer.Data = listado;
            transfer.Pagination.TotalPages = totalpages + ((totalElements % parameters.ItemsPerPage) > 0 ? 1 : 0);
            transfer.Pagination.TotalRecords = totalElements; //Total de elementos segun filtro
            transfer.Pagination.TotalDisplayRecords = listado.Count; //Total de elementos segun pagina

            return Json(transfer);
        }
    }
}