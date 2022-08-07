namespace OpenTTD.DataAccess.Seeder;

public interface IDbSeeder
{
    Task SeedAsync(CancellationToken ct = default);
}