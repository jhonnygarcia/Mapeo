using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters.AsignaturaIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IAsignaturaIntegracionServices
    {
        ResultList<AsignaturaIntegracionDto> GetPagedAsignatura(SearchAsignaturaIntegracionParameters parameters);
        ResultValue<AsignaturaIntegracionDto> Get(int id);
        ResultList<AsignaturaIntegracionDto> GetFilteredList(int? idEstudioGestor, int? idAsignaturaEstudioGestor,
            int? idPlanErp, int? idAsignaturaPlanErp);
        BaseResult Crear(SaveAsignaturaIntegracionParameters model);
        BaseResult Modificar(SaveAsignaturaIntegracionParameters model);
        BaseResult Eliminar(int[] ids);
    }
}