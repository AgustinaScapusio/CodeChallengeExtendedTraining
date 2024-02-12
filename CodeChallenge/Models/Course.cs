using System;

public class Course
{

		public int Id {  get; set; }

	    public string Name { get; set; } = string.Empty;

	    public string City { get; set; } = string.Empty;

	    public DateTime FromDate { get; set; }

	    public DateTime ToDate { get; set; }

	    public ICollection<Student>? Students { get; set; }

	    public ICollection<CourseModule>? CourseModules { get; set; }

	}
