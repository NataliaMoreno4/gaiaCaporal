using PlantillaBlazor.Domain.Entities.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Interfaces.Auditoria
{
    public interface IAuditoriaService
    {
        /// <summary>
        /// Registra la auditoría de un intento de envío de un e-mail. 
        /// </summary>
        /// <param name="auditoriaEmail">Objeto <see cref="AuditoriaEnvioEmail"/> que contiene toda la información de un envío e-mail</param>
        /// <returns><see langword="true" /> si el registro fue exitoso, <see langword="false" /> en caso de que no</returns>
        public Task<bool> RegistrarAuditoriaEnvioEmail(AuditoriaEnvioEmail auditoriaEmail);
        /// <summary>
        /// Registra la auditoría de un evento de navegación a través del sitio web por parte de un usuario
        /// </summary>
        /// <param name="auditoriaNavegacion">Objeto <see cref="AuditoriaNavegacion"/> el cual contiene todos los detalles de un evento de navegación de un usuario a través del sitio web</param>
        /// <returns><see langword="true" /> si la inserción fue correcta, <see langword="false" /> en caso de que no</returns>
        public Task<bool> RegistrarAuditoriaNavegacion(AuditoriaNavegacion auditoriaNavegacion);
        /// <summary>
        /// Registra la auditoría de un evento de descarga de un archivo desde el sitio web
        /// </summary>
        /// <param name="auditoriaDescargaArchivo">Objeto <see cref="AuditoriaDescargaArchivo"/> que contiene todos los detalles de la descarga del archivo</param>
        /// <returns><see langword="true" /> si la inserción fue correcta, <see langword="false" /> en caso de que no</returns>
        public Task<bool> RegistrarAuditoriaDescargaArchivo(AuditoriaDescargaArchivo auditoriaDescargaArchivo);
        /// <summary>
        /// Registra la auditoría de un evento dentro del aplicativo
        /// </summary>
        /// <param name="auditoriaEvento">Objeto <see cref="AuditoriaEvento"/> que contiene todos los detalles del evento ocurrido</param>
        /// <returns><see langword="true" /> si la inserción fue correcta, <see langword="false" /> en caso de que no</returns>
        public Task<bool> RegistrarAuditoriaEvento(AuditoriaEvento auditoriaEvento);
        /// <summary>
        /// Registra la auditoría un envío de un mensaje SMS
        /// </summary>
        /// <param name="auditoriaEnvioSms">Objeto <see cref="AuditoriaEnvioSMS"/> del mensaje SMS envíado</param>
        /// <returns><see langword="true" /> si la inserción fue correcta, <see langword="false" /> en caso de que no</returns>
        public Task<bool> RegistrarAuditoriaEnvioSMS(AuditoriaEnvioSMS auditoriaEnvioSms);
        /// <summary>
        /// Registra la auditoría un envío de un mensaje vía Wpp
        /// </summary>
        /// <param name="auditoriaEnvioWpp">Objeto <see cref="AuditoriaEnvioWpp"/> del mensaje Wpp envíado</param>
        /// <returns><see langword="true" /> si la inserción fue correcta, <see langword="false" /> en caso de que no</returns>
        public Task<bool> RegistrarAuditoriaEnvioWpp(AuditoriaEnvioWpp auditoriaEnvioWpp);
    }
}
