using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters.EstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IEstudioIntegracionServices
    {       
        ResultList<EstudioIntegracionDto> GetPagedEstudios(SearchEstudioIntegracionParameters parameters);
        BaseResult Crear(SaveEstudioIntegracionParameters model);
        BaseResult Modificar(SaveEstudioIntegracionParameters model);
        BaseResult Eliminar(int[] ids);
        ResultValue<EstudioIntegracionDto> Get(int id);
        ResultList<EstudioIntegracionDto> GetFilteredList(int? idEstudioGestor, int? idPlanErp,
            int? idEspecializacionErp);
    }
}