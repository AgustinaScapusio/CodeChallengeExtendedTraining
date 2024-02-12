using CodeChallenge.Models;
using System;

public class Student
{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public bool HasExperience { get; set; }

	    public int Age { get; set; }

	    public int CourseId { get; set; }

	    public Course? Course { get; set; }

		public ICollection<CheckpointStudent>? CheckpointStudents { get; set; }

	}

