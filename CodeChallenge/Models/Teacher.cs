﻿using System;

public class Teacher
{
		public int Id {  get; set; }
	    
	    public string Name { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

	    public ICollection<TeacherModule>? TeacherModules { get; set; }
	
}
