using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Implementations.Auditoria
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaService(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<bool> RegistrarAuditoriaDescargaArchivo(AuditoriaDescargaArchivo auditoriaDescargaArchivo)
        {
            return await _auditoriaRepository.RegistrarAuditoriaDescargaArchivo(auditoriaDescargaArchivo);
        }

        public async Task<bool> RegistrarAuditoriaEnvioEmail(AuditoriaEnvioEmail auditoriaEmail)
        {
            return await _auditoriaRepository.RegistrarAuditoriaEnvioEmail(auditoriaEmail);
        }

        public async Task<bool> RegistrarAuditoriaEnvioSMS(AuditoriaEnvioSMS auditoriaEnvioSms)
        {
            return await _auditoriaRepository.RegistrarAuditoriaEnvioSMS(auditoriaEnvioSms);
        }

        public async Task<bool> RegistrarAuditoriaEnvioWpp(AuditoriaEnvioWpp auditoriaEnvioWpp)
        {
            return await _auditoriaRepository.RegistrarAuditoriaEnvioWpp(auditoriaEnvioWpp);
        }

        public async Task<bool> RegistrarAuditoriaEvento(AuditoriaEvento auditoriaEvento)
        {
            return await _auditoriaRepository.RegistrarAuditoriaEvento(auditoriaEvento);
        }

        public async Task<bool> RegistrarAuditoriaNavegacion(AuditoriaNavegacion auditoriaNavegacion)
        {
            return await _auditoriaRepository.RegistrarAuditoriaNavegacion(auditoriaNavegacion);
        }
    }
}
