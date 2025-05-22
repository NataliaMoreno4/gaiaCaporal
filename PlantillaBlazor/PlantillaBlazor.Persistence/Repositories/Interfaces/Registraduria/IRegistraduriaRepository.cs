using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Repositories.Common;

namespace PlantillaBlazor.Persistence.Repositories.Interfaces.Registraduria
{
    public interface IRegistraduriaRepository : IGenericRepository<AuditoriaConsumoRegistraduria>
    {
        public Task<AuditoriaConsumoRegistraduria> ConsultarRegistroRegistraduria(string cedula);
    }
}
