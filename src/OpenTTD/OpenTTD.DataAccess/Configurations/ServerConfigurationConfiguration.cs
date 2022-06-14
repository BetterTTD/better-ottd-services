using System.Net;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenTTD.DataAccess.Metadata;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.DataAccess.Configurations;

public sealed class ServerConfigurationConfiguration : IEntityTypeConfiguration<ServerConfiguration>
{
    public void Configure(EntityTypeBuilder<ServerConfiguration> builder)
    {
        builder
            .ToTable(Tables.ServerConfiguration, Schemas.Dbo)
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                guid => new ServerId(guid))
            .HasColumnName("ServerConfigurationID")
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();

        builder
            .Property(p => p.IpAddress)
            .HasConversion(
                ip => ip.ToString(), 
                str => IPAddress.Parse(str))
            .HasColumnType("nvarchar(15)")
            .IsRequired();
        
        builder
            .Property(p => p.Password)
            .HasColumnName("ServerPassword")
            .HasColumnType("nvarchar(50)")
            .IsRequired();

        builder
            .Property(p => p.Name)
            .HasColumnName("BotName")
            .HasColumnType("nvarchar(50)")
            .IsRequired();
        
        builder
            .Property(p => p.Version)
            .HasColumnName("BotVersion")
            .HasColumnType("nvarchar(10)")
            .IsRequired();
    }
}