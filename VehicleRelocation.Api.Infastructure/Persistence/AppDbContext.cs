using System;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.EntityFrameworkCore;

namespace VehicleRelocation.Api.Infastructure.Persistence
{
	public class AppDbContext : DataConnection
	{
		public AppDbContext(DataOptions<AppDbContext> options)
			: base(options.Options)
		{

		}
	}

}

