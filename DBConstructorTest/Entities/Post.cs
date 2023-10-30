using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConstructorTest.Entities
{
    internal class Post
    {
        [Key]
        public int Id { get; set; }

        public DateOnly Post_date { get; set; }

        public TimeOnly Post_time { get; set; }

        public string Post_content { get; set; }
    }
}
