using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.Parameters.PeriodoMatriculacionIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IPeriodoMatriculacionIntegracionServices
    {
        ResultList<PeriodoMatriculacionIntegracionDto> GetPagedPeriodoMatriculacion(
            SearchPeriodoMatriculacionIntegracionParameters parameters);
        ResultValue<PeriodoMatriculacionIntegracionDto> Get(int id);
        BaseResult Crear(SavePeriodoMatriculacionIntegracionParameters model);
        BaseResult Modificar(SavePeriodoMatriculacionIntegracionParameters model);
        BaseResult Eliminar(int[] ids);
    }
}