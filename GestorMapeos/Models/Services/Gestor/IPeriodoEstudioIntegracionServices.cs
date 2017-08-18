
using GestorMapeos.Models.Dto.Gestor;
using GestorMapeos.Parameters.PeriodoEstudioIntegracion;
using GestorMapeos.TransferStructs;

namespace GestorMapeos.Models.Services.Gestor
{
    public interface IPeriodoEstudioIntegracionServices
    {
        ResultList<PeriodoEstudioIntegracionDto> GetPagedPeriodoEstudio(SearchPeriodoEstudioIntegracionParameters parameters);
        ResultValue<PeriodoEstudioIntegracionDto> Get(int id);
        BaseResult Crear(SavePeriodoEstudioIntegracionParameters model);
        BaseResult Modificar(SavePeriodoEstudioIntegracionParameters model);
        BaseResult Eliminar(int[] ids);
    }
}