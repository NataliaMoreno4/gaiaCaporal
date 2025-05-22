using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Services.Interfaces.Messages;
using PlantillaBlazor.Domain.Entities.Auditoria;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Crmf;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace PlantillaBlazor.Services.Implementations.Messages
{
    public class SMSMessageSender : ISMSMessageSender
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SMSMessageSender> _logger;
        private readonly IAuditoriaService _auditoriaService;

        public SMSMessageSender
        (
            ILogger<SMSMessageSender> logger,
            IAuditoriaService auditoriaService,
            HttpClient httpClient
        )
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
            _httpClient = httpClient;
        }



        public async Task<bool> SendSMSMessage(string phoneNumber, string message, string concepto, string identificacionProceso, string tipoProceso)
        {
            AuditoriaEnvioSMS auditoriaSMS = new AuditoriaEnvioSMS()
            {
                CelularDestinatario = phoneNumber,
                FechaEnvio = DateTime.Now,
                Concepto = concepto,
                IdentificacionProceso = identificacionProceso,
                Mensaje = message
            };

            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message))
            {
                return false;
            }

            var body = new BodySMSRequest()
            {
                to = new string[] { $"57{phoneNumber}" },
                text = message,
                from = "ESE",
                parts = "1",
                trsec = "1",
            };


            try
            {
                auditoriaSMS.ContenidoBody = JsonConvert.SerializeObject(body);

                var response = await _httpClient.PostAsJsonAsync($"rest/message", body);

                auditoriaSMS.FueEnviado = response.IsSuccessStatusCode;
                auditoriaSMS.StatusCode = response.StatusCode.ToString();
                auditoriaSMS.ContenidoRespuesta = await response.Content.ReadAsStringAsync();
                auditoriaSMS.UrlRequest = response.RequestMessage!.RequestUri!.ToString();
            }
            catch (Exception exe)
            {
                auditoriaSMS.FueEnviado = false;
                auditoriaSMS.Error = $"{exe.Message} {exe.InnerException}";
                _logger.LogError(exe, $"Error al enviar mensaje sms a {phoneNumber}");
            }

            await _auditoriaService.RegistrarAuditoriaEnvioSMS(auditoriaSMS);

            return auditoriaSMS.FueEnviado;
        }
    }


    public class BodySMSRequest
    {
        public string[]? to { get; set; }
        public string? text { get; set; }
        public string? from { get; set; }
        public string? parts { get; set; }
        public string? trsec { get; set; }
    }
}
