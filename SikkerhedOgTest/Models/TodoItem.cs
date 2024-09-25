using SikkerhedOgTest.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SikkerhedOgTest.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        public int CprId { get; set; }  // Foreign key linking to the Cpr table

        [ForeignKey("CprId")]
        public Cpr Cpr { get; set; }

        public string? TodoItemName { get; set; }

    }
}
