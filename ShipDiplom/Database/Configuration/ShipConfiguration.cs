﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Database.Configuration;

public class ShipConfiguration : BaseConfigurationWithId<Ship>
{
    public override void Configure(EntityTypeBuilder<Ship> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ShipType).IsRequired();
        builder.Property(x => x.SystemId).IsRequired();
        builder.Property(x => x.OwnerId).IsRequired();
        builder.Property(x => x.Length).IsRequired();
        builder.Property(x => x.Width).IsRequired();
    }
}