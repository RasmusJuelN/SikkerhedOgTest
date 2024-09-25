using SikkerhedOgTest.Data;
using System.ComponentModel.DataAnnotations;

namespace SikkerhedOgTest.Models
{
        public class Cpr
        {
            [Key]
            public int Id { get; set; }

            public string Username { get; set; }
          
            public string CprNr { get; set; }
            public List<TodoItem> TodoItems { get; set; }
    }
}
