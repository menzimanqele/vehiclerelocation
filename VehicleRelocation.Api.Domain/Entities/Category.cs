using System.ComponentModel.DataAnnotations.Schema;
using VehicleRelocation.Api.Domain.Entities.Base;

namespace VehicleRelocation.Api.Domain.Entities;

[Table("Categories")]
public class Category : BaseEntity<Int32>
{
    public string Name { get; set; }
    public int  DisplayOrder { get; set; }
}