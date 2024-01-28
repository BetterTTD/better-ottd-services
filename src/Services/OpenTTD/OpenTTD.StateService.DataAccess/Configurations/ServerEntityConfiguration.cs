using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenTTD.StateService.DataAccess.Entities;

namespace OpenTTD.StateService.DataAccess.Configurations;

public sealed class ServerEntityConfiguration : IEntityTypeConfiguration<ServerEntity>
{
    public void Configure(EntityTypeBuilder<ServerEntity> builder)
    {
        builder
            .ToTable("Server", "dbo")
            .HasKey(p => p.Id)
            .HasName("Id");

        builder
            .Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(100);

        builder
            .Property(p => p.AdminName)
            .HasColumnName("AdminName")
            .HasMaxLength(100);

        builder
            .Property(p => p.IpAddress)
            .HasConversion(
                p => p.ToString(),
                p => IPAddress.Parse(p))
            .HasColumnName("IpAddress")
            .HasMaxLength(39);

        builder
            .Property(p => p.Port)
            .HasColumnName("Port")
            .HasColumnType("smallint");

        builder
            .Property(p => p.Password)
            .HasColumnName("Password")
            .HasMaxLength(256);
    }
}