using System;

public class CourseModule
{
	
		public int ID {  get; set; }

	    public int ModuleID { get; set; }

	    public int CourseID { get; set; }

	    public Course? Course { get; set; }

	    public Module? Module { get; set; }
}
