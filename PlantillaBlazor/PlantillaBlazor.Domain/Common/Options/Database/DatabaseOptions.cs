namespace PlantillaBlazor.Domain.Common.Options.Database
{
    /// <summary>
    /// Opciones de configuración para conexiones de base de datos realizadas para EF Core
    /// </summary>
    public class DatabaseOptions
    {
        /// <summary>
        /// Cadena de conexión
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;
        /// <summary>
        /// Cantidad máxima de reintentos cuando una operación de base de datos falle
        /// </summary>
        public int MaxRetryCount { get; set; }
        /// <summary>
        /// Tiempo en segundo que se debe esperar a que una operación de base de datos termine antes de mostrar un error
        /// </summary>
        public int CommandTimeout { get; set; }
        /// <summary>
        /// Indica si EF Core debe mostrar errores detallados
        /// </summary>
        public bool EnableDetailedErrors { get; set; }
        /// <summary>
        /// Indica si EF Core debe mostrar información sensible mediante Logs
        /// </summary>
        public bool EnableSensitiveDataLogging { get; set; }
    }
}
