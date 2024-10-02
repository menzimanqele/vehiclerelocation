using System;
using LinqToDB;
using Microsoft.EntityFrameworkCore;

namespace VehicleRelocation.Api.Infastructure.Persistence
{
	public class AppDbContext : LinqToDB.Data.DataConnection
	{
		public AppDbContext(DataOptions<AppDbContext> options)
			: base(options.Options)
		{

		}
	}

}

