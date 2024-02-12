using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models
{
    public class Checkpoint
    {
        public int Id { get; set; }

        [Range(0,10)]
        public int Score { get; set; }

        public int ModuleId { get; set; }

        public Module? Module { get; set; }

        public ICollection<CheckpointStudent>? CheckpointStudents {  get; set; }

    }
}
