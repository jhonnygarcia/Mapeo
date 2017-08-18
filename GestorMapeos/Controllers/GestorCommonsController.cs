using System.Linq;
using System.Web.Mvc;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Parameters.GestorCommons;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Parameters;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Controllers
{
    [Authorize]
    public class GestorCommonsController : Controller
    {
        private readonly GestorContext _gestorContext;
        private readonly ErpContext _erpContext;
        public GestorCommonsController(GestorContext gestorContext, ErpContext erpContext)
        {
            _gestorContext = gestorContext;
            _erpContext = erpContext;
        }

        // GET: GestorCommons
        public ActionResult SimpleSearchPlantillaEstudio(SimpleListViewModel parameters)
        {
            var transfer = new ClientTransfer();
            var plantillaQueriable = _gestorContext.PlantillaEstudio.Where(p => true);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                plantillaQueriable = plantillaQueriable.Where(p => (p.Nombre + " - " + p.AnyoPlan).Contains(parameters.Descripcion));

            var selectQuery = plantillaQueriable.Select(r => new
            {
                r.Id,
                Descripcion = r.Nombre + " - " + r.AnyoPlan,
                TipoEstudio = r.TipoEstudio.Nombre,
                Rama = r.RamaId.HasValue ? r.Rama.Nombre : ""
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
        public ActionResult SimpleSearchPeriodoMatriculacion(SimpleListViewModel parameters)
        {
            var transfer = new ClientTransfer();
            var periodoMatriculacionQueriable = _gestorContext.PeriodosMatriculacionesUnir.Where(p => p.Borrado.Equals("No"));
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                periodoMatriculacionQueriable = periodoMatriculacionQueriable.Where(a => a.Nombre.Contains(parameters.Descripcion));

            var selectQuery = periodoMatriculacionQueriable.Select(a => new
            {
                a.Id,
                Descripcion = a.Nombre,
                a.AnyoAcademico,
                a.FechaInicio,
                a.FechaFin,
                a.Nro
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
                FechaInicio = a.FechaInicio?.ToString("dd/MM/yyyy"),
                FechaFin = a.FechaFin?.ToString("dd/MM/yyyy"),
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
        public ActionResult SimpleSearchPlantillaAsignatura(SimpleListPlantillaAsignaturaParameters parameters)
        {
            var transfer = new ClientTransfer();
            var plantillaQueriable = _gestorContext.PlantillaAsignatura.Where(pa => true);
            if (parameters.IdPlantillaEstudio.HasValue)
                plantillaQueriable = plantillaQueriable.Where(pa => pa.PlantillaEstudioId == parameters.IdPlantillaEstudio.Value);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                plantillaQueriable = plantillaQueriable.Where(pa => pa.NombreAsignatura.Contains(parameters.Descripcion));

            var selectQuery = plantillaQueriable.Select(pa => new
            {
                pa.Id,
                Descripcion = pa.NombreAsignatura,
                pa.Codigo,
                TipoAsignatura = pa.TipoAsignatura.Nombre,
                pa.Creditos,
                PlantillaEstudio = pa.PlantillaEstudio.Nombre + " - " + pa.PlantillaEstudio.AnyoPlan
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
        public ActionResult SimpleSearchAsignatura(SimpleListAsignaturaParameters parameters)
        {
            var transfer = new ClientTransfer();
            var asignaturaQueriable = _gestorContext.AsignaturaUnir.Where(pa => pa.Borrado.Equals("No"));
            if (parameters.IdPeriodoMatriculacion.HasValue)
                asignaturaQueriable = asignaturaQueriable.Where(a => a.EstudioUnir.Borrado.Equals("No") &&
                    a.EstudioUnir.PeriodosEstudioUnir.Any(p => p.Borrado.Equals("No") &&
                                                               p.PeriodoMatriculacionId ==
                                                               parameters.IdPeriodoMatriculacion.Value));
            if (parameters.IdEstudio.HasValue)
                asignaturaQueriable = asignaturaQueriable.Where(a => a.Borrado.Equals("No") && a.EstudioUnirId == parameters.IdEstudio.Value);
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                asignaturaQueriable = asignaturaQueriable.Where(a => a.Nombre.Contains(parameters.Descripcion));

            var selectQuery = asignaturaQueriable.Select(a => new
            {
                a.Id,
                Descripcion = a.Nombre,
                a.TipoAsignatura,
                a.Creditos,
                a.PeriodoLectivo,
                a.Curso,
                a.Activo,
                Estudio = a.EstudioUnir.Nombre
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
        public ActionResult SimpleSearchEstudio(SimpleListEstudioParameters parameters)
        {
            var transfer = new ClientTransfer();
            var estudioQueriable = _gestorContext.EstudioUnir.Where(p => p.Borrado.Equals("No"));
            if (parameters.IdPeriodoMatriculacion.HasValue)
                estudioQueriable = estudioQueriable.Where(e => e.PeriodosEstudioUnir.Any(pe => pe.Borrado.Equals("No") 
                            && pe.PeriodoMatriculacionUnir.Borrado.Equals("No")
                            && pe.PeriodoMatriculacionId == parameters.IdPeriodoMatriculacion.Value));
            if (!string.IsNullOrEmpty(parameters.Descripcion))
                estudioQueriable = estudioQueriable.Where(e => e.Nombre.Contains(parameters.Descripcion));

            var selectQuery = estudioQueriable.Select(r => new
            {
                r.Id,
                Descripcion = r.Nombre,
                Plan = r.PlanEstudio,
                r.Activo,
                TipoEstudio = new
                {
                    r.TipoEstudioSegunUnir.Nombre
                },
                Rama = r.RamaEstudio,
                Titulo = r.EstudioPrincipalUnir != null ? new
                {
                    r.EstudioPrincipalUnir.Id,
                    r.EstudioPrincipalUnir.Codigo,
                    r.EstudioPrincipalUnir.Nombre,
                    DisplayName = r.EstudioPrincipalUnir.Codigo + " - " + r.EstudioPrincipalUnir.Nombre
                } : null
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