using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Entities
{
    [PrimaryKey(nameof(ID), nameof(StationID))]
    public class Point
    {
        
        public int ID { get; set; }
        [ForeignKey(nameof(StationID))]
        public int StationID { get; set; }
        public string? Name { get; set; }

        public Station Station { get; set; }    
    }
}
