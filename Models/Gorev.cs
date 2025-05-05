using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTasksApp.Models
{
    public class Gorev
    {
        public int Id { get; set; }
        public string? Baslik { get; set; }
        public string? Aciklama { get; set; }
        public DateTime SonTarih { get; set; }
        public bool TamamlandiMi { get; set; }

        public int AssignedUserId { get; set; }
        [ForeignKey("AssignedUserId")]
        public User? AssignedUser { get; set; }

        public int? CompletedByUserId { get; set; }
        [ForeignKey("CompletedByUserId")]
        public User? CompletedByUser { get; set; }
    }
}
