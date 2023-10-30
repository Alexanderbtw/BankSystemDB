using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConstructorTest.Entities
{
    internal class User
    {
        public int Id { get; set; }

        [Column("FullName")]
        public string? Name { get; set; }

        public int? Age { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
