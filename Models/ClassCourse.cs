using System;
using System.Collections.Generic;

namespace Lab1_SQL.Models;

public partial class ClassCourse
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public int CourseId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
