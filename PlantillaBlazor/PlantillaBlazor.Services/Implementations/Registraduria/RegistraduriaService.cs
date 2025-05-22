using PlantillaBlazor.Domain.Common.ResultModels;
using PlantillaBlazor.Domain.DTO.Registraduria;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Auditoria;
using PlantillaBlazor.Persistence.Repositories.Interfaces.Registraduria;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace PlantillaBlazor.Services.Implementations.Registraduria
{
    public sealed class RegistraduriaService
    {
        private readonly HttpClient _client;
        private readonly IAuditoriaRepository _auditoriaRepository;
        private readonly IRegistraduriaRepository _registraduriaRepository;
        private readonly ILogger<RegistraduriaService> _logger;

        public RegistraduriaService
        (
            HttpClient client,
            IAuditoriaRepository auditoriaRepository,
            IRegistraduriaRepository registraduriaRepository,
            ILogger<RegistraduriaService> logger
        )
        {
            _client = client;
            _auditoriaRepository = auditoriaRepository;
            _registraduriaRepository = registraduriaRepository;
            _logger = logger;
        }

        public async Task<Result<ResponseRegistraduria>> ConsultarCedulaRegistraduria(RequestRegistraduria request)
        {
            Result<ResponseRegistraduria> resultFinal = default;

            var consulta = await _registraduriaRepository.ConsultarRegistroRegistraduria(request.Cedula);

            //Si existe una consulta en auditoria para la cédula consulta y ésta no supera los 30 dias de consultada, se devuelve la auditoria de base de datos
            if (consulta is not null)
            {
                var respuesta = new ResponseRegistraduria()
                {
                    Cedula = consulta.CedulaConsultada,
                    CodigoErrorCedula = consulta.CodigoErrorCedula,
                    DepartamentoExpedicionDocumento = consulta.DepartamentoExpedicionDocumento,
                    EstadoCedula = consulta.EstadoCedula,
                    MunicipioExpedicionDocumento = consulta.MunicipioExpedicionDocumento,
                    PrimerApellido = consulta.PrimerApellido,
                    PrimerNombre = consulta.PrimerNombre,
                    SegundoApellido = consulta.SegundoApellido,
                    SegundoNombre = consulta.SegundoNombre
                };

                DateTime fechaE = new DateTime();

                if (DateTime.TryParse(consulta.FechaExpedicionDocumento, out fechaE))
                {
                    respuesta.FechaExpedicionDocumento = fechaE;
                }

                if (respuesta.CodigoErrorCedula.Equals("OK – Candidato encontrado") && respuesta.EstadoCedula.Equals("Vigente"))
                {
                    resultFinal = Result<ResponseRegistraduria>.Success(respuesta);
                }
                else
                {
                    resultFinal = Result<ResponseRegistraduria>.Failure($"Cédula no válida: {respuesta.CodigoErrorCedula} - {respuesta.EstadoCedula}");
                }

                return resultFinal;
            }

            AuditoriaConsumoRegistraduria auditoria = new();
            auditoria.CedulaConsultada = request.Cedula;
            auditoria.IpConsulta = request.IpAccion;
            auditoria.UsuarioConsulta = request.Usuario;

            var body = new RegistraduriaBody()
            {
                Documento = request.Cedula
            };

            auditoria.FechaInicioConsulta = DateTime.Now;
            auditoria.BodyRequest = JsonConvert.SerializeObject(body);

            try
            {
                var response = await _client.PostAsJsonAsync($"api/ValidacionAni", body);

                auditoria.StatusCodeRespuesta = response.StatusCode.ToString();
                auditoria.FechaFinConsulta = DateTime.Now;
                auditoria.UrlRequest = response.RequestMessage!.RequestUri!.ToString();

                var content = await response.Content.ReadAsStringAsync();

                auditoria.JsonResponse = content;

                if (response.IsSuccessStatusCode)
                {
                    Root respuesta = JsonConvert.DeserializeObject<Root>(content);

                    ResponseRegistraduria respuestaConsulta = new();

                    switch (respuesta.Respuesta.CodigoErrorDatosCedula)
                    {
                        case "0":
                            respuestaConsulta.CodigoErrorCedula = "OK – Candidato encontrado";
                            break;
                        case "1":
                            respuestaConsulta.CodigoErrorCedula = "Candidato no encontrado";
                            break;
                        case "2":
                            respuestaConsulta.CodigoErrorCedula = "Campos de entrada con formato erróneo";
                            break;
                        case "3":
                            respuestaConsulta.CodigoErrorCedula = "Error interno del servicio";
                            break;
                    }

                    switch (respuesta.Respuesta.EstadoCedula)
                    {
                        case "0":
                            respuestaConsulta.EstadoCedula = "Vigente";
                            break;
                        case "1":
                            respuestaConsulta.EstadoCedula = "Vigente";
                            break;
                        case "12":
                            respuestaConsulta.EstadoCedula = "Baja por Pérdida o Suspensión de los Derechos Políticos";
                            break;
                        case "14":
                            respuestaConsulta.EstadoCedula = "Baja por Interdicción Judicial por Demencia";
                            break;
                        case "21":
                            respuestaConsulta.EstadoCedula = "Cancelada por Muerte";
                            break;
                        case "22":
                            respuestaConsulta.EstadoCedula = "Cancelada por Doble Cedulación";
                            break;
                        case "23":
                            respuestaConsulta.EstadoCedula = "Cancelada por Suplantación o Falsa Identidad";
                            break;
                        case "24":
                            respuestaConsulta.EstadoCedula = "Cancelada por Menoría de Edad";
                            break;
                        case "25":
                            respuestaConsulta.EstadoCedula = "Cancelada por Extranjería";
                            break;
                        case "26":
                            respuestaConsulta.EstadoCedula = "Cancelada por Mala Elaboración";
                            break;
                        case "27":
                            respuestaConsulta.EstadoCedula = "Cancelada por Reasignación o cambio de sexo";
                            break;
                        case "51":
                            respuestaConsulta.EstadoCedula = "Cancelada por Muerte Facultad Ley 1365 2009";
                            break;
                        case "52":
                            respuestaConsulta.EstadoCedula = "Cancelada por Intento de Doble Cedulación NO Expedida";
                            break;
                        case "53":
                            respuestaConsulta.EstadoCedula = "Cancelada por Falsa Identidad o Suplantación NO Expedida";
                            break;
                        case "54":
                            respuestaConsulta.EstadoCedula = "Cancelada por Menoría de Edad NO Expedida";
                            break;
                        case "55":
                            respuestaConsulta.EstadoCedula = "Cancelada por Extranjería NO Expedida";
                            break;
                        case "56":
                            respuestaConsulta.EstadoCedula = "Cancelada por Mala Elaboración No Expedida";
                            break;
                    }

                    DateTime fechaExp = new DateTime();

                    if (DateTime.TryParse(respuesta.Respuesta.FechaExpedicion, out fechaExp))
                    {
                        respuestaConsulta.FechaExpedicionDocumento = fechaExp;
                    }

                    respuestaConsulta.MunicipioExpedicionDocumento = respuesta.Respuesta.MunicipioExpedicion;
                    respuestaConsulta.DepartamentoExpedicionDocumento = respuesta.Respuesta.DepartamentoExpedicion;
                    respuestaConsulta.PrimerApellido = respuesta.Respuesta.PrimerApellido;
                    respuestaConsulta.SegundoApellido = respuesta.Respuesta.SegundoApellido;
                    respuestaConsulta.PrimerNombre = respuesta.Respuesta.PrimerNombre;
                    respuestaConsulta.SegundoNombre = respuesta.Respuesta.SegundoNombre;

                    auditoria.CodigoErrorCedula = respuestaConsulta.CodigoErrorCedula;
                    auditoria.EstadoCedula = respuestaConsulta.EstadoCedula;
                    auditoria.DepartamentoExpedicionDocumento = respuestaConsulta.DepartamentoExpedicionDocumento;
                    auditoria.FechaExpedicionDocumento = respuestaConsulta.FechaExpedicionDocumento.GetValueOrDefault().ToString();
                    auditoria.MunicipioExpedicionDocumento = respuestaConsulta.MunicipioExpedicionDocumento;
                    auditoria.PrimerNombre = respuestaConsulta.PrimerNombre;
                    auditoria.SegundoNombre = respuestaConsulta.SegundoNombre;
                    auditoria.PrimerApellido = respuestaConsulta.PrimerApellido;
                    auditoria.SegundoApellido = respuestaConsulta.SegundoApellido;

                    if (respuestaConsulta.CodigoErrorCedula.Equals("OK – Candidato encontrado") && respuestaConsulta.EstadoCedula.Equals("Vigente"))
                    {
                        resultFinal = Result<ResponseRegistraduria>.Success(respuestaConsulta);
                    }
                    else
                    {
                        resultFinal = Result<ResponseRegistraduria>.Failure($"Cédula no válida: {respuestaConsulta.CodigoErrorCedula} - {respuestaConsulta.EstadoCedula}");
                    }
                }
                else
                {
                    resultFinal = Result<ResponseRegistraduria>.Failure("Respuesta incorrecta de servicio de registraduría");
                }
            }
            catch (Exception exe)
            {
                _logger.LogError(exe, $"Error al consultar cédula {request.Cedula} en registraduría");
                auditoria.Error = $"{exe.Message} {exe.InnerException}";

                resultFinal = Result<ResponseRegistraduria>.Failure("Ocurrió un error en la consulta a registraduría");
            }

            await _auditoriaRepository.RegistrarAuditoriaConsumoRegistraduria(auditoria);

            return resultFinal;
        }
    }

    public class Respuesta
    {
        public string AnioResolucion { get; set; }
        public string CodigoErrorDatosCedula { get; set; }
        public string DepartamentoExpedicion { get; set; }
        public string EstadoCedula { get; set; }
        public DateTime? FechaDefuncion { get; set; }
        public string FechaExpedicion { get; set; }
        public string Informante { get; set; }
        public string MunicipioExpedicion { get; set; }
        public string NUIP { get; set; }
        public string NumeroResolucion { get; set; }
        public string Particula { get; set; }
        public string PrimerApellido { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoApellido { get; set; }
        public string SegundoNombre { get; set; }
        public string Serial { get; set; }
    }

    public class Root
    {
        public Respuesta Respuesta { get; set; }
        public string CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
    }

    public class RegistraduriaBody
    {
        public string CodigoAplicacion { get; set; } = "4FB1F7A7-4CE5-4F32-AAA6-77AEA695F259";
        public string Documento { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = "1";
    }
}
