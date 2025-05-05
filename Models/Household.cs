using System.Collections.Generic;

namespace HomeTasksApp.Models
{
    public class Household
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
