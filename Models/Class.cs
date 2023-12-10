using System;
using System.Collections.Generic;

namespace Lab1_SQL.Models;

public partial class Class
{
    public int Id { get; set; }

    public string ClassName { get; set; } = null!;

    public virtual ICollection<ClassCourse> ClassCourses { get; set; } = new List<ClassCourse>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
