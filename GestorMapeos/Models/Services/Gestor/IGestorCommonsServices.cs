using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters;
using GestorMapeos.Parameters.AsignaturaOfertadaIntegracion;
using GestorMapeos.Parameters.GestorCommons;
using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IGestorCommonsServices
    {
        ResultList<PlantillaEstudioDto> SimpleSearchPlantillaEstudio(SimpleListViewModel parameters);
        ResultList<PeriodoMatriculacionUnirDto> SimpleSearchPeriodoMatriculacion(SimpleListViewModel parameters);
        ResultList<PlantillaAsignaturaDto> SimpleSearchPlantillaAsignatura(SimpleListPlantillaAsignaturaParameters parameters);
        ResultList<AsignaturaUnirDto> SimpleSearchAsignatura(SimpleListAsignaturaParameters parameters);
        ResultList<EstudioUnirDto> SimpleSearchEstudio(SimpleListEstudioParameters parameters);
        ResultList<PeriodoEstudioUnirDto> GetPagedPeriodoEstudio(SearchPeriodoEstudioParameters parameters);
        ResultList<PeriodoEstudioAsignaturaUnirDto> SearchPeriodoEstudioAsignatura(SearchPeriodoEstudioAsignaturaParameters parameters);
        
    }
}