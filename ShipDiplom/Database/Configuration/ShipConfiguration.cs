using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Database.Configuration;

public class ShipConfiguration : BaseConfigurationWithId<Ship>
{
    public override void Configure(EntityTypeBuilder<Ship> builder)
    {
        base.Configure(builder);

        builder.
    }
}