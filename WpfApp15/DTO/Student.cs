using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp15.DTO
{
    public class Student : BaseDTO
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public DateTime Birthday { get; set; }
        public int GroupId { get; set; }
    }
}
