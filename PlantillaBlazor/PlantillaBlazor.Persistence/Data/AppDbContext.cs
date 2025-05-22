using Microsoft.EntityFrameworkCore;
using PlantillaBlazor.Domain.Entities.Auditoria;
using PlantillaBlazor.Domain.Entities.GaiaCaporal;
using PlantillaBlazor.Domain.Entities.Parametrizacion;
using PlantillaBlazor.Domain.Entities.Perfilamiento;
using PlantillaBlazor.Persistence.Data.TablesConfigurations.Auditoria;
using PlantillaBlazor.Persistence.Data.TablesConfigurations.GaiaCaporal;
using PlantillaBlazor.Persistence.Data.TablesConfigurations.Parametrizacion;
using PlantillaBlazor.Persistence.Data.TablesConfigurations.Perfilamiento;

namespace PlantillaBlazor.Persistence.Data
{
    /// <summary>
    /// DbContext que gestiona todas las operaciones de base de datos del aplicativo
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        #region Perfilamiento

        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolModulo> RolModulo { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<SolicitudRecuperacionClave> SolicitudesRecuperacionClave { get; set; }
        public DbSet<AuditoriaLoginUsuario> AuditoriaLoginUsuario { get; set; }
        public DbSet<Session> Sessions { get; set; }

        #endregion

        #region Auditoria

        public DbSet<AuditoriaDescargaArchivo> AuditoriaDescargaArchivos { get; set; }
        public DbSet<AuditoriaEnvioEmail> AuditoriaEnvioEmail { get; set; }
        public DbSet<AuditoriaAdjuntoEmail> AuditoriaAdjuntosEmail { get; set; }
        public DbSet<AuditoriaNavegacion> AuditoriaNavegacion { get; set; }
        public DbSet<AuditoriaOtp> AuditoriaOtp { get; set; }
        public DbSet<AuditoriaEnvioSMS> AuditoriaEnvioSMS { get; set; }
        public DbSet<AuditoriaEnvioWpp> AuditoriaEnvioWpp { get; set; }
        public DbSet<AuditoriaEvento> AuditoriaEventos { get; set; }
        public DbSet<AuditoriaConsumoRegistraduria> AuditoriaConsumoRegistraduria { get; set; }

        #endregion

        #region Parametrizacion

        public DbSet<ParametroGeneral> ParametrosGenerales { get; set; }
        public DbSet<ParametroDetallado> ParametrosDetallados { get; set; }

        #endregion
        #region GaiaCaporal

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Perfilamiento

            modelBuilder.ApplyConfiguration(new ModuloConfig());
            modelBuilder.ApplyConfiguration(new RolConfig());
            modelBuilder.ApplyConfiguration(new RolModuloConfig());
            modelBuilder.ApplyConfiguration(new UsuarioConfig());
            modelBuilder.ApplyConfiguration(new SolicitudRecuperacionClaveConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaLoginUsuarioConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaEnvioSMSConfig());
            modelBuilder.ApplyConfiguration(new SessionsConfig());

            #endregion

            #region Auditoria

            modelBuilder.ApplyConfiguration(new AuditoriaDescargaArchivosConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaEnvioEmailConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaAdjuntoEmailConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaNavegacionConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaOtpConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaEnvioSMSConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaEnvioWppConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaEventosConfig());
            modelBuilder.ApplyConfiguration(new AuditoriaConsumoRegistraduriaConfig());

            #endregion

            #region Parametrizacion

            modelBuilder.ApplyConfiguration(new PGeneralConfig());
            modelBuilder.ApplyConfiguration(new PDetalladoConfig());

            #endregion

            #region GaiaCaporal

            modelBuilder.ApplyConfiguration(new ProductoConfig());
            modelBuilder.ApplyConfiguration(new CarritoConfig());
            modelBuilder.ApplyConfiguration(new DetallePedidoConfig());

            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
