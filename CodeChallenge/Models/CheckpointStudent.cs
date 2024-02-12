namespace CodeChallenge.Models
{
    public class CheckpointStudent
    {
       public int Id { get; set; }

       public int StudentID { get; set; }

       public int CheckpointID { get; set; }

       public Student? Student { get; set; }

       public Checkpoint? Checkpoint { get; set; }
    }
}
