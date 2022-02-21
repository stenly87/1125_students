using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp15.Tools;

namespace WpfApp15.DTO
{
    [Table("group")]
    public class Group : BaseDTO
    {
        [Column("title")]
        public string Title { get; set; }
        [Column("year")]
        public int Year { get; set; }
    }
}
