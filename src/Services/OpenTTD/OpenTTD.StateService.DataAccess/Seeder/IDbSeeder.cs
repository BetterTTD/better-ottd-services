namespace OpenTTD.StateService.DataAccess.Seeder;

public interface IDbSeeder
{
    Task SeedDataAsync();
}