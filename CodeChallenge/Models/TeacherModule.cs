using System;

public class TeacherModule
{
		public int ID {  get; set; }
	    
	    public int ModuleID { get; set; }

	    public int TeacherID { get; set; }

	    public Module? Module { get; set; }

	   public Teacher? Teacher { get; set; }
}

