using System.Reflection;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VehicleRelocation.Api.Domain;
using VehicleRelocation.Api.Domain.Interfaces;
using VehicleRelocation.Api.Infastructure.Persistence;
using VehicleRelocation.Api.Infastructure.Repositories;

namespace VehicleRelocation.Api.Infastructure.Extensions
{
    public static class InfastructureExenstions
	{
		public static void ResgisterInfastructure(IServiceCollection services, IConfiguration configuration)
		{
			
			
			
			// services.AddDbContext<AppDbContext>(options =>
			// options.UseSqlServer(configuration.GetConnectionString("Data"))
			// );



			services.AddLinqToDBContext<AppDbContext>((provoider, options) =>
				options.UseSqlServer(configuration.GetSection("Data").Get<List<DatabaseConfig>>()[0].ConnectionString)
					
					.UseDefaultLogging(provoider));
			
			
			// Register Data Access Services
			//services.AddDataContextSupport<AppDbContext>(configuration, true, typeof(AppDbContext).Assembly);
			
			//services.AddScoped((Func<IServiceProvider, BulkSignatureDbContext>) (_ => new BulkSignatureDbContext()));
			
			services.AddLinqToDBContext<BulkSignatureDbContext>((provoider, options) =>
				options.UseSqlServer(configuration.GetSection("Data").Get<List<DatabaseConfig>>()[1].ConnectionString)
					.UseDefaultLogging(provoider));
		//services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
		//services.AddScoped<IUnitOfWork, UnitOfWork<BulkSignatureDbContext>>();
			//services.AddScoped((Func<IServiceProvider, BulkSignatureDbContext>)(_ => new BulkSignatureDbContext()));

			services.AddScoped<IUnitOfWork, UnitOfWork>();



		}
		
		private static void AddDataContextSupport<TDbContext>(this IServiceCollection services, IConfiguration configuration, bool migrateDatabases, params Assembly[] assemblies)
			where TDbContext : AppDbContext, new()
		{
			
			//services.(configuration, migrateDatabases, assemblies);
			
			/*
			services.AddDbContext<Persistence.AppDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("Data"))
			);*/

			
			//services.AddScoped(_ => new TDbContext());
            
			// Register Unit of Work
			//services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
		}

	}
}

