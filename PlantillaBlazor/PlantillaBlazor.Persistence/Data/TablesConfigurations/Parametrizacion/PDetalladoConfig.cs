using PlantillaBlazor.Domain.Entities.Parametrizacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantillaBlazor.Persistence.Data.TablesConfigurations.Parametrizacion
{
    internal class PDetalladoConfig : IEntityTypeConfiguration<ParametroDetallado>
    {
        public void Configure(EntityTypeBuilder<ParametroDetallado> builder)
        {
            builder.HasOne(p => p.ParametroGeneral)
                .WithMany(pg => pg.ListaParametrosDetallados)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(p => p.IdParametroGeneral);

            builder.HasData(Build());
        }

        private List<ParametroDetallado> Build()
        {
            return new List<ParametroDetallado>
            {
                new ParametroDetallado()
                {
                    Id = 1,
                    Nombre = "Si",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1),
                    IdParametroGeneral = 1
                },
                new ParametroDetallado()
                {
                    Id = 2,
                    Nombre = "No",
                    IdUsuarioAdiciono = 1,
                    FechaAdicion = new DateTime(2024,5,1),
                    IdParametroGeneral = 1
                }
            };
        }
    }
}
