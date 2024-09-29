using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using VehicleRelocation.Api.Domain;
using VehicleRelocation.Api.Domain.Interfaces.Repositories;
using VehicleRelocation.Api.Infastructure.Extensions;
using VehicleRelocation.Api.Infastructure.Repositories;
 

const string MyCorsPolicy = "AllowedCors";
var builder = WebApplication.CreateBuilder(args);
RegisterRepos(builder.Services);

// Add services to the container.

builder.Services.AddControllers();
// Learn more abou  t configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseConfig>(
    builder.Configuration.GetSection("Data"));


var keyVaultUrl = "https://secret-sauce00058.vault.azure.net";//builder.Configuration.GetSection("KeyVault:keyVaultUrl");
var clientId = "d551e4b4-eb5e-447f-b981-85e9034d077b"; //builder.Configuration.GetSection("KeyVault:clientId");
var clientSecret = "Cp18Q~2pmGwFu.jvnTErSC6A-VrqtmZS8Wmt6dip"; //builder.Configuration.GetSection("KeyVault:clientSecret");
var directoryId = "3f043d0a-742f-40da-83f3-cc56b9476bd5";// builder.Configuration.GetSection("KeyVault:directoryId");
var credential = new ClientSecretCredential(directoryId.ToString(), clientId.ToString(), clientSecret.ToString());

/*
builder.Configuration.AddAzureKeyVault(keyVaultUrl.ToString(), clientId.ToString(), clientSecret.ToString(), new DefaultKeyVaultSecretManager());
var client = new SecretClient(new Uri(keyVaultUrl.ToString()), credential);

var dbConnection = client.GetSecret("dbConnection").Value.Value.ToString();*/

// Register the CORS policies
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCorsPolicy,
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

InfastructureExenstions.ResgisterInfastructure(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

 void RegisterRepos( IServiceCollection services)
{
    services.AddScoped<IManufactureRepository, ManufactureRepository>();
}

