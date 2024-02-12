using CodeChallenge.Models;
using System;

public class Module
{
		public int ID {  get; set; }

	    public string Title { get; set; } = string.Empty;
	    
	    public ICollection<TeacherModule>? TeacherModules { get; set; }

		public ICollection<CourseModule>? CourseModule { get; set; }

	    public Checkpoint? Checkpoint { get; set; }

}
