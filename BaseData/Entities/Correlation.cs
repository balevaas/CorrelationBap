using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Entities
{
    [PrimaryKey(nameof(ID))]
    public class Correlation
    {
        public int ID { get; set; }
        [ForeignKey(nameof(PointID))]
        public int PointID { get; set; }
        public decimal ResultCorrelation { get; set; }
        public DateTime Year { get; set; }

        public Point Point { get; set; }
    }
}
