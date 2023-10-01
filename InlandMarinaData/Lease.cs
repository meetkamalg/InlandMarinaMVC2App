using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    [Table("Lease")]
    public class Lease
    {
        public int ID { get; set; }
        public int SlipID { get; set; }
        public int CustomerID { get; set; }

        //navigation properties
        public virtual Customer Customer { get; set; }
        public virtual Slip Slip { get; set; }
    }
}
