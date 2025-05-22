using PlantillaBlazor.Services.Interfaces.Auditoria;
using PlantillaBlazor.Services.Interfaces.Messages;
using PlantillaBlazor.Domain.Entities.Auditoria;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Services.Implementations.Messages
{
    public class WhatsappMessageSender : IWhatsappMessageSender
    {
        private readonly ILogger<WhatsappMessageSender> _logger;
        private readonly IAuditoriaService _auditoriaService;
        private readonly HttpClient _httpClient;

        public WhatsappMessageSender
        (
            ILogger<WhatsappMessageSender> logger,
            IAuditoriaService auditoriaService,
            HttpClient httpClient
        )
        {
            _logger = logger;
            _auditoriaService = auditoriaService;
            _httpClient = httpClient;
        }

        public async Task<bool> SendWhatsappMessage(string phoneNumber, string message, string concepto, string identificacionProceso, string tipoProceso)
        {
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message))
            {
                return false;
            }

            AuditoriaEnvioWpp auditoriaWpp = new AuditoriaEnvioWpp()
            {
                CelularDestinatario = phoneNumber,
                Concepto = concepto,
                FechaEnvio = DateTime.Now,
                IdentificacionProceso = identificacionProceso,
                Mensaje = message
            };

            try
            {
                string body_content = @"{
                        ""messaging_product"": ""whatsapp"",
                        ""recipient_type"": ""individual"",
                        ""to"": ""57" + phoneNumber + @""",
                        ""type"": ""template"",
                        ""template"": {
                            ""name"": ""defaultnot"",
                            ""language"": {
                                ""code"": ""es""
                            },
                            ""components"": [
                                {
                                    ""type"": ""body"",
                                    ""parameters"": [
                                        {
                                            ""type"": ""text"",
                                            ""text"": """ + message + @"""
                                        }
                                    ]
                                }
                            ]
                        }
                    }";
                var content = new StringContent(body_content, null, "application/json");

                var response = await _httpClient.PostAsync("v18.0/111109975281154/messages", content);

                auditoriaWpp.FueEnviado = response.IsSuccessStatusCode;
                auditoriaWpp.StatusCode = response.StatusCode.ToString();
                auditoriaWpp.ContenidoRespuesta = await response.Content.ReadAsStringAsync();
                auditoriaWpp.UrlRequest = response.RequestMessage!.RequestUri!.ToString();
                auditoriaWpp.ContenidoBody = body_content;
            }
            catch (Exception exe)
            {
                auditoriaWpp.FueEnviado = false;
                auditoriaWpp.Error = $"{exe.Message} {exe.InnerException}";
                _logger.LogError(exe, $"Error al enviar mensaje vía whatsapp al número {phoneNumber}");
            }

            await _auditoriaService.RegistrarAuditoriaEnvioWpp(auditoriaWpp);

            return auditoriaWpp.FueEnviado;
        }

        public async Task<bool> SendWhatsappMessageOTP(string phoneNumber, string message, string concepto, string identificacionProceso, string tipoProceso)
        {
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message))
            {
                return false;
            }

            AuditoriaEnvioWpp auditoriaWpp = new AuditoriaEnvioWpp()
            {
                CelularDestinatario = phoneNumber,
                Concepto = concepto,
                FechaEnvio = DateTime.Now,
                IdentificacionProceso = identificacionProceso,
                Mensaje = message
            };

            try
            {
                string body_content = @"{
                            ""messaging_product"": ""whatsapp"",
                            ""recipient_type"": ""individual"",
                            ""to"": ""57" + phoneNumber + @""",
                            ""type"": ""template"",
                            ""template"": {
                                ""name"": ""codigootp"",
                                ""language"": {
                                    ""code"": ""es""
                                },
                                ""components"": [
                                    {
                                        ""type"": ""body"",
                                        ""parameters"": [
                                            {
                                                ""type"": ""text"",
                                                ""text"": """ + message + @"""
                                            }
                                        ]
                                    },
                                    {
                                        ""type"": ""button"",
                                        ""sub_type"": ""url"",
                                        ""index"": ""0"",
                                        ""parameters"": [
                                            {
                                                ""type"": ""text"",
                                                ""text"": """ + message + @"""
                                            }
                                        ]
                                    }
                                ]
                            }
                        }";
                var content = new StringContent(body_content, null, "application/json");

                var response = await _httpClient.PostAsync("v18.0/111109975281154/messages", content);

                auditoriaWpp.FueEnviado = response.IsSuccessStatusCode;
                auditoriaWpp.StatusCode = response.StatusCode.ToString();
                auditoriaWpp.ContenidoRespuesta = await response.Content.ReadAsStringAsync();
                auditoriaWpp.UrlRequest = response.RequestMessage!.RequestUri!.ToString();
                auditoriaWpp.ContenidoBody = body_content;
            }
            catch (Exception exe)
            {
                auditoriaWpp.FueEnviado = false;
                auditoriaWpp.Error = $"{exe.Message} {exe.InnerException}";
                _logger.LogError(exe, $"Error al enviar mensaje vía whatsapp al número {phoneNumber}");
            }

            await _auditoriaService.RegistrarAuditoriaEnvioWpp(auditoriaWpp);

            return auditoriaWpp.FueEnviado;
        }
    }
}
