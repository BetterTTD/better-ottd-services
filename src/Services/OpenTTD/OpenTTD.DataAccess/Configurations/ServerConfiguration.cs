using System.Net;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenTTD.DataAccess.Metadata;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.DataAccess.Configurations;

public sealed class ServerConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.ToTable(Tables.Server, Schemas.Dbo);

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("ServerID");

        builder
            .Property(p => p.Name)
            .HasConversion(name => name.Value, str => new ServerName(str));
        
        builder
            .Property(p => p.Password)
            .HasConversion(pass => pass.Value, str => new ServerPassword(str));

        builder
            .Property(p => p.IpAddress)
            .HasConversion(ip => ip.ToString(), str => IPAddress.Parse(str))
            .HasColumnName("IP_Address");
        
        builder
            .Property(p => p.Port)
            .HasConversion(port => port.Value, @int => new ServerPort(@int));
        
        builder
            .Property(p => p.Version)
            .HasConversion(ver => ver.Value, str => new ServerVersion(str));
    }
}