using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters.PlantillaEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IPlantillaEstudioIntegracionServices
    {
        ResultValue<PlantillaEstudioIntegracionDto> Get(int id);
        ResultList<PlantillaEstudioIntegracionDto> GetFilteredList(int? idRefPlan, int? idPlantillaEstudio);
        BaseResult Eliminar(int[] ids);
        BaseResult Crear(SavePlantillaEstudioIntegracionParameters model);
        BaseResult Modificar(SavePlantillaEstudioIntegracionParameters model);
        ResultList<PlantillaEstudioIntegracionDto> GetPagedPlantillasEstudio(SearchPlantillaEstudioIntegracionParameters parameters);

    }
}