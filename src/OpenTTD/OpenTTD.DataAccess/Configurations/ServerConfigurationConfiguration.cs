using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenTTD.DataAccess.Metadata;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.DataAccess.Configurations;

public sealed class ServerConfigurationConfiguration : IEntityTypeConfiguration<ServerConfiguration>
{
    public void Configure(EntityTypeBuilder<ServerConfiguration> builder)
    {
        builder.ToTable(Tables.ServerConfiguration, Schemas.Dbo);
        
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.IpAddress)
            .HasConversion(
                ip => ip.ToString(), 
                str => IPAddress.Parse(str))
            .HasColumnType("nvarchar(15)");
        
        builder
            .Property(p => p.Password)
            .HasColumnName("ServerPassword")
            .HasColumnType("nvarchar(50)");

        builder
            .Property(p => p.Name)
            .HasColumnName("BotName")
            .HasColumnType("nvarchar(50)");
        
        builder
            .Property(p => p.Version)
            .HasColumnName("BotVersion")
            .HasColumnType("nvarchar(10)");
    }
}