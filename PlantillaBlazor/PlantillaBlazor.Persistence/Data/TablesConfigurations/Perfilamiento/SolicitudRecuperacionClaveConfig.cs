using PlantillaBlazor.Domain.Entities.Perfilamiento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Perfilamiento
{
    public class SolicitudRecuperacionClaveConfig : IEntityTypeConfiguration<SolicitudRecuperacionClave>
    {
        public void Configure(EntityTypeBuilder<SolicitudRecuperacionClave> builder)
        {
            builder.HasOne(s => s.Usuario)
                .WithMany(u => u.ListaSolicitudesRecuperacionClaves)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(s => s.IdUsuario);


            builder.HasIndex(s => s.Estado);
            builder.HasIndex(s => s.MotivoCambioContraseña);

            //Data inicial
            builder.HasData(
                Build()
            );
        }

        private List<SolicitudRecuperacionClave> Build()
        {
            return new List<SolicitudRecuperacionClave>()
            {

            };
        }
    }
}
