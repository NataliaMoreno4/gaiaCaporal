using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlantillaBlazor.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Aud");

            migrationBuilder.EnsureSchema(
                name: "Seg");

            migrationBuilder.EnsureSchema(
                name: "Par");

            migrationBuilder.CreateTable(
                name: "AuditoriaConsumoRegistraduria",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicioConsulta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFinConsulta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusCodeRespuesta = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BodyRequest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CedulaConsultada = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IpConsulta = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UsuarioConsulta = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UrlRequest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoErrorCedula = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstadoCedula = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartamentoExpedicionDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaExpedicionDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MunicipioExpedicionDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimerNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SegundoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimerApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SegundoApellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaConsumoRegistraduria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaDescargaArchivos",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RutaOriginal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaDescargada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtensionArchivo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PesoArchivo = table.Column<long>(type: "bigint", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaDescargaArchivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaEnvioEmail",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailDestinatario = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailCC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBCC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Asunto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailEmisor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MensajeHTML = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FueEnviado = table.Column<bool>(type: "bit", nullable: false),
                    DescripcionError = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pantalla = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroIdentificacionProceso = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Puerto = table.Column<int>(type: "int", nullable: true),
                    SslEnabled = table.Column<bool>(type: "bit", nullable: true),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaEnvioEmail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaEnvioSMS",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CelularDestinatario = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FueEnviado = table.Column<bool>(type: "bit", nullable: false),
                    IdentificacionProceso = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Pantalla = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContenidoRespuesta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContenidoBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaEnvioSMS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaEnvioWpp",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CelularDestinatario = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FueEnviado = table.Column<bool>(type: "bit", nullable: false),
                    IdentificacionProceso = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Pantalla = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContenidoRespuesta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContenidoBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaEnvioWpp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaNavegacion",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Navegador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionNavegador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlataformaNavegador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlActual = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Idioma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CookiesHabilitadas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnchoPantalla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AltoPantalla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfundidadColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreSO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionSO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdUsuarioAccion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLocationPermitted = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaNavegacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaOtp",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaValidacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdentificacionProceso = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TipoProceso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MetodosEnvio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaOtp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modulo",
                schema: "Seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreModulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoModulo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nivel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParametroGeneral",
                schema: "Par",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametroGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                schema: "Seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaAdjuntoEmail",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RutaAbsolutaAdjunto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NombreAdjunto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAuditoriaEnvioEmail = table.Column<long>(type: "bigint", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaAdjuntoEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriaAdjuntoEmail_AuditoriaEnvioEmail_IdAuditoriaEnvioEmail",
                        column: x => x.IdAuditoriaEnvioEmail,
                        principalSchema: "Aud",
                        principalTable: "AuditoriaEnvioEmail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParametroDetallado",
                schema: "Par",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IdParametroGeneral = table.Column<long>(type: "bigint", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametroDetallado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParametroDetallado_ParametroGeneral_IdParametroGeneral",
                        column: x => x.IdParametroGeneral,
                        principalSchema: "Par",
                        principalTable: "ParametroGeneral",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolModulo",
                schema: "Seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRol = table.Column<long>(type: "bigint", nullable: false),
                    IdModulo = table.Column<long>(type: "bigint", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolModulo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolModulo_Modulo_IdModulo",
                        column: x => x.IdModulo,
                        principalSchema: "Seg",
                        principalTable: "Modulo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolModulo_Rol_IdRol",
                        column: x => x.IdRol,
                        principalSchema: "Seg",
                        principalTable: "Rol",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "Seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdRol = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FechaUltimoCambioContraseña = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MustChangePassword = table.Column<bool>(type: "bit", nullable: false),
                    Celular = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsTwoFAEnabled = table.Column<bool>(type: "bit", nullable: false),
                    FechaUltimoIngreso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_IdRol",
                        column: x => x.IdRol,
                        principalSchema: "Seg",
                        principalTable: "Rol",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaEventos",
                schema: "Aud",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Accion = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdUsuarioAccion = table.Column<long>(type: "bigint", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdentificadorProceso = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaEventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriaEventos_Usuario_IdUsuarioAccion",
                        column: x => x.IdUsuarioAccion,
                        principalSchema: "Seg",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditoriaLoginUsuario",
                schema: "Seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    FechaLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IpLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCierreSesion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpCierreSesion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MotivoCierreSesion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaLoginUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditoriaLoginUsuario_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "Seg",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Session",
                schema: "Seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RememberMe = table.Column<bool>(type: "bit", nullable: false),
                    FechaUltimoIngreso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaInactivacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdAuditoriaLogin = table.Column<long>(type: "bigint", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "Seg",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SolicitudRecuperacionClave",
                schema: "Seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAccionInicial = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IpAccionFinal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MotivoCambioContraseña = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaAdicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAdiciono = table.Column<long>(type: "bigint", nullable: true),
                    IdUsuarioUltimaActualizacion = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudRecuperacionClave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitudRecuperacionClave_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "Seg",
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Seg",
                table: "Modulo",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "Nivel", "NombreModulo", "TipoModulo" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "1", "Configuración", "Módulo" },
                    { 2L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "1.1", "Perfilamiento", "Submódulo" },
                    { 3L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "1.1.1", "Usuarios", "Submódulo" },
                    { 4L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "1.1.2", "Roles", "Submódulo" },
                    { 5L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "1.1.3", "Módulos", "Submódulo" },
                    { 6L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "1.2", "Parametrización", "Submódulo" }
                });

            migrationBuilder.InsertData(
                schema: "Par",
                table: "ParametroGeneral",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "Nombre" },
                values: new object[] { 1L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, "SiNo" });

            migrationBuilder.InsertData(
                schema: "Seg",
                table: "Rol",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "IsActive", "Nombre" },
                values: new object[] { 1L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, null, true, "Administrador" });

            migrationBuilder.InsertData(
                schema: "Par",
                table: "ParametroDetallado",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdParametroGeneral", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "Nombre" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, 1L, null, "Si" },
                    { 2L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, 1L, null, "No" }
                });

            migrationBuilder.InsertData(
                schema: "Seg",
                table: "RolModulo",
                columns: new[] { "Id", "FechaAdicion", "FechaUltimaActualizacion", "IdModulo", "IdRol", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, 1L, 1L, null },
                    { 2L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2L, 1L, 1L, null },
                    { 3L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3L, 1L, 1L, null },
                    { 4L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4L, 1L, 1L, null },
                    { 5L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5L, 1L, 1L, null },
                    { 6L, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 6L, 1L, 1L, null }
                });

            migrationBuilder.InsertData(
                schema: "Seg",
                table: "Usuario",
                columns: new[] { "Id", "Apellidos", "Celular", "Clave", "Email", "FechaAdicion", "FechaUltimaActualizacion", "FechaUltimoCambioContraseña", "FechaUltimoIngreso", "IdRol", "IdUsuarioAdiciono", "IdUsuarioUltimaActualizacion", "IsActive", "IsTwoFAEnabled", "MustChangePassword", "NombreUsuario", "Nombres" },
                values: new object[] { 1L, "Administrador", "", "$2a$10$shiRv6MY8eRdGrMd./ISYOSdhkxcfEEulDJQeVzF8JTGUJi/jK1Pq", "leonardo.arias@excellentiam.co", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, 1L, null, null, true, false, true, "Administrador", "Administrador" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaAdjuntoEmail_IdAuditoriaEnvioEmail",
                schema: "Aud",
                table: "AuditoriaAdjuntoEmail",
                column: "IdAuditoriaEnvioEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaAdjuntoEmail_RutaAbsolutaAdjunto",
                schema: "Aud",
                table: "AuditoriaAdjuntoEmail",
                column: "RutaAbsolutaAdjunto");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaConsumoRegistraduria_CedulaConsultada",
                schema: "Aud",
                table: "AuditoriaConsumoRegistraduria",
                column: "CedulaConsultada");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaConsumoRegistraduria_CodigoErrorCedula",
                schema: "Aud",
                table: "AuditoriaConsumoRegistraduria",
                column: "CodigoErrorCedula");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaConsumoRegistraduria_EstadoCedula",
                schema: "Aud",
                table: "AuditoriaConsumoRegistraduria",
                column: "EstadoCedula");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaConsumoRegistraduria_StatusCodeRespuesta",
                schema: "Aud",
                table: "AuditoriaConsumoRegistraduria",
                column: "StatusCodeRespuesta");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaConsumoRegistraduria_UsuarioConsulta",
                schema: "Aud",
                table: "AuditoriaConsumoRegistraduria",
                column: "UsuarioConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioEmail_Concepto",
                schema: "Aud",
                table: "AuditoriaEnvioEmail",
                column: "Concepto");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioEmail_EmailDestinatario",
                schema: "Aud",
                table: "AuditoriaEnvioEmail",
                column: "EmailDestinatario");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioEmail_FueEnviado",
                schema: "Aud",
                table: "AuditoriaEnvioEmail",
                column: "FueEnviado");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioEmail_NumeroIdentificacionProceso",
                schema: "Aud",
                table: "AuditoriaEnvioEmail",
                column: "NumeroIdentificacionProceso");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioSMS_CelularDestinatario",
                schema: "Aud",
                table: "AuditoriaEnvioSMS",
                column: "CelularDestinatario");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioSMS_FechaEnvio",
                schema: "Aud",
                table: "AuditoriaEnvioSMS",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioSMS_FueEnviado",
                schema: "Aud",
                table: "AuditoriaEnvioSMS",
                column: "FueEnviado");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioSMS_IdentificacionProceso",
                schema: "Aud",
                table: "AuditoriaEnvioSMS",
                column: "IdentificacionProceso");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioSMS_Pantalla",
                schema: "Aud",
                table: "AuditoriaEnvioSMS",
                column: "Pantalla");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioWpp_CelularDestinatario",
                schema: "Aud",
                table: "AuditoriaEnvioWpp",
                column: "CelularDestinatario");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioWpp_FechaEnvio",
                schema: "Aud",
                table: "AuditoriaEnvioWpp",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioWpp_FueEnviado",
                schema: "Aud",
                table: "AuditoriaEnvioWpp",
                column: "FueEnviado");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioWpp_IdentificacionProceso",
                schema: "Aud",
                table: "AuditoriaEnvioWpp",
                column: "IdentificacionProceso");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEnvioWpp_Pantalla",
                schema: "Aud",
                table: "AuditoriaEnvioWpp",
                column: "Pantalla");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEventos_Accion",
                schema: "Aud",
                table: "AuditoriaEventos",
                column: "Accion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEventos_Concepto",
                schema: "Aud",
                table: "AuditoriaEventos",
                column: "Concepto");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEventos_IdentificadorProceso",
                schema: "Aud",
                table: "AuditoriaEventos",
                column: "IdentificadorProceso");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaEventos_IdUsuarioAccion",
                schema: "Aud",
                table: "AuditoriaEventos",
                column: "IdUsuarioAccion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLoginUsuario_Descripcion",
                schema: "Seg",
                table: "AuditoriaLoginUsuario",
                column: "Descripcion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLoginUsuario_FechaAdicion",
                schema: "Seg",
                table: "AuditoriaLoginUsuario",
                column: "FechaAdicion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLoginUsuario_IdUsuario",
                schema: "Seg",
                table: "AuditoriaLoginUsuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaNavegacion_IdUsuarioAccion",
                schema: "Aud",
                table: "AuditoriaNavegacion",
                column: "IdUsuarioAccion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaNavegacion_IpAddress",
                schema: "Aud",
                table: "AuditoriaNavegacion",
                column: "IpAddress");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaNavegacion_UrlActual",
                schema: "Aud",
                table: "AuditoriaNavegacion",
                column: "UrlActual");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaOtp_Codigo",
                schema: "Aud",
                table: "AuditoriaOtp",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaOtp_Estado",
                schema: "Aud",
                table: "AuditoriaOtp",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaOtp_FechaAdicion",
                schema: "Aud",
                table: "AuditoriaOtp",
                column: "FechaAdicion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaOtp_FechaValidacion",
                schema: "Aud",
                table: "AuditoriaOtp",
                column: "FechaValidacion");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaOtp_IdentificacionProceso",
                schema: "Aud",
                table: "AuditoriaOtp",
                column: "IdentificacionProceso");

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaOtp_TipoProceso",
                schema: "Aud",
                table: "AuditoriaOtp",
                column: "TipoProceso");

            migrationBuilder.CreateIndex(
                name: "IX_ParametroDetallado_IdParametroGeneral",
                schema: "Par",
                table: "ParametroDetallado",
                column: "IdParametroGeneral");

            migrationBuilder.CreateIndex(
                name: "IX_RolModulo_IdModulo",
                schema: "Seg",
                table: "RolModulo",
                column: "IdModulo");

            migrationBuilder.CreateIndex(
                name: "IX_RolModulo_IdRol",
                schema: "Seg",
                table: "RolModulo",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Session_FechaUltimoIngreso",
                schema: "Seg",
                table: "Session",
                column: "FechaUltimoIngreso");

            migrationBuilder.CreateIndex(
                name: "IX_Session_IdAuditoriaLogin",
                schema: "Seg",
                table: "Session",
                column: "IdAuditoriaLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Session_IdUsuario",
                schema: "Seg",
                table: "Session",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Session_IsActive",
                schema: "Seg",
                table: "Session",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudRecuperacionClave_Estado",
                schema: "Seg",
                table: "SolicitudRecuperacionClave",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudRecuperacionClave_IdUsuario",
                schema: "Seg",
                table: "SolicitudRecuperacionClave",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudRecuperacionClave_MotivoCambioContraseña",
                schema: "Seg",
                table: "SolicitudRecuperacionClave",
                column: "MotivoCambioContraseña");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                schema: "Seg",
                table: "Usuario",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdRol",
                schema: "Seg",
                table: "Usuario",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IsActive",
                schema: "Seg",
                table: "Usuario",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NombreUsuario",
                schema: "Seg",
                table: "Usuario",
                column: "NombreUsuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaAdjuntoEmail",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "AuditoriaConsumoRegistraduria",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "AuditoriaDescargaArchivos",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "AuditoriaEnvioSMS",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "AuditoriaEnvioWpp",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "AuditoriaEventos",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "AuditoriaLoginUsuario",
                schema: "Seg");

            migrationBuilder.DropTable(
                name: "AuditoriaNavegacion",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "AuditoriaOtp",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "ParametroDetallado",
                schema: "Par");

            migrationBuilder.DropTable(
                name: "RolModulo",
                schema: "Seg");

            migrationBuilder.DropTable(
                name: "Session",
                schema: "Seg");

            migrationBuilder.DropTable(
                name: "SolicitudRecuperacionClave",
                schema: "Seg");

            migrationBuilder.DropTable(
                name: "AuditoriaEnvioEmail",
                schema: "Aud");

            migrationBuilder.DropTable(
                name: "ParametroGeneral",
                schema: "Par");

            migrationBuilder.DropTable(
                name: "Modulo",
                schema: "Seg");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "Seg");

            migrationBuilder.DropTable(
                name: "Rol",
                schema: "Seg");
        }
    }
}
