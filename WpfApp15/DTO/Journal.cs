using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp15.Tools;

namespace WpfApp15.DTO
{
    [Table("journal")]
    public class Journal : BaseDTO
    {
        [Column("day")]
        public DateTime Day { get; set; } = DateTime.Now;
        [Column("value")]
        public string Value { get; set; } = "";

        [Column("discipline_id")]
        public int DisciplineId { get; set; }
        [Column("student_id")]
        public int StudentId { get; set; }

        public Discipline Discipline { get; set; }
        public Student Student { get; set; }
    }
}
