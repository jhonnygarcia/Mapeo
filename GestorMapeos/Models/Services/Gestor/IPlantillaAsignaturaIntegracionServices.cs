using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters.PlantillaAsignaturaIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IPlantillaAsignaturaIntegracionServices
    {
        BaseResult Crear(SavePlantillaAsignaturaIntegracionParameters model);
        BaseResult Modificar(SavePlantillaAsignaturaIntegracionParameters model);
        BaseResult Eliminar(int[] ids);
        ResultList<PlantillaAsignaturaIntegracionDto> GetPagedPlantillaAsignatura(SearchPlantillaAsignaturaIntegracionParameters parameters);
        ResultValue<PlantillaAsignaturaIntegracionDto> Get(int id);
        /// <summary>
        /// Obtiene un listado de Plantillas asignaturas en base a los filtros
        /// </summary>
        /// <param name="idPlantillaEstudio"></param>
        /// <param name="idPlantillaAsignatura"></param>
        /// <param name="idPlanErp"></param>
        /// <param name="idAsignaturaPlanErp"></param>
        /// <returns></returns>
        ResultList<PlantillaAsignaturaIntegracionDto> GetFilteredList(int? idPlantillaEstudio,
            int? idPlantillaAsignatura, int? idPlanErp, int? idAsignaturaPlanErp);
    }
}