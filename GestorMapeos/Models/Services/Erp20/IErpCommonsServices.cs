using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorMapeos.Models.Dto.Erp20;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters;
using GestorMapeos.Parameters.ErpCommons;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Parameters.PlanOfertado;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Erp20
{
    public interface IErpCommonsServices
    {
        ResultList<PlanDto> SimpleSearchPlan(SimpleListPlanParameters parameters);
        ResultList<TituloDto> SimpleSearchTitulos(SimpleListViewModel parameters);
        ResultList<EspecializacionDto> SimpleSearchEspecializaciones(SimpleListEspecializacionViewModel parameters);
        ResultList<PeriodoAcademicoDto> SimpleSearchPeriodoAcademico(SimpleListViewModel parameters);
        ResultList<AsignaturaPlanDto> SimpleSearchAsignaturaPlan(SimpleListAsignaturaPlanParameters parameters);
        ResultList<AsignaturaOfertadaDto> GetPageAsignaturaOfertada(SimpleListAsignaturaOfertadaParameters parameters);
        ResultList<PlanOfertadoDto> GetPagePlanOfertado(SearchPlanOfertadoParameters parameters);
    }
}