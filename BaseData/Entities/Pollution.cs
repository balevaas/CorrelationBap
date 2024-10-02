using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData.Entities
{
    [PrimaryKey(nameof(Date), nameof(PointID))]
    public class Pollution
    {
        public DateTime Date { get; set; }
        [ForeignKey(nameof(PointID))]
        public int PointID {  get; set; }
        public decimal Concentration { get; set; }

        public Point Point { get; set; }
    }
}
