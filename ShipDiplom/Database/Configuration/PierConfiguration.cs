using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Database.Configuration;

public class PierConfiguration: BaseConfigurationWithId<Pier>
{
    public override void Configure(EntityTypeBuilder<Pier> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.TrackingNumber).IsRequired();
        builder.Property(x => x.Location).IsRequired();
        builder.Property(x => x.Length).IsRequired();
        builder.Property(x => x.Width).IsRequired();
    }
}
