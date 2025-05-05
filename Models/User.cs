using System.Collections.Generic;

namespace HomeTasksApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public bool IsHouseholdAdmin { get; set; }

        public int HouseholdId { get; set; }
        public Household? Household { get; set; }
        public bool IsAdmin { get; set; }


        public ICollection<Gorev>? AssignedTasks { get; set; } = new List<Gorev>();
        public ICollection<Gorev>? CompletedTasks { get; set; } = new List<Gorev>();


    }
}
