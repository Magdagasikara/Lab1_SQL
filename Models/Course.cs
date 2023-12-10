using System;
using System.Collections.Generic;

namespace Lab1_SQL.Models;

public partial class Course
{
    public int Id { get; set; }

    public string CourseName { get; set; } = null!;

    public virtual ICollection<ClassCourse> ClassCourses { get; set; } = new List<ClassCourse>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
