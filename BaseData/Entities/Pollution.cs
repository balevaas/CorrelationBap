using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Entities
{
    [PrimaryKey(nameof(Date), nameof(PointID))]
    public class Pollution
    {
        public DateTime Date { get; set; }
        [ForeignKey(nameof(PointID))]
        public int PointID { get; set; }
        public decimal Concentration { get; set; }

        public Point? Point { get; set; }
    }
}
