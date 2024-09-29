namespace VehicleRelocation.Api.Domain
{
    public class DatabaseConfig
	{
        public string Name { get; set; }
        public string Provider { get; set; }
        public required string ConnectionString { get; set; }
        public required string ConnectionType { get; set; }
        public bool EnsureDatabaseIsCreated { get; set; }
        public string? DbCreatorUser { get; set; }
        public string? DbCreatorPassword { get; set; }
        public bool Migrate { get; set; }
        public string? MigrationSchema { get; set; }
        public string? MigrationTable { get; set; }
        public string? MigrationAssembly { get; set; }
    }
}

