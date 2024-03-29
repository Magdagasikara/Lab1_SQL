﻿using System;
using System.Collections.Generic;

namespace Lab1_SQL.Models;

public partial class Grade
{
    public int Id { get; set; }

    public int Grade1 { get; set; } // varför blev heter Grade1 här efter scaffold men det ska va Grade?

    public DateOnly GradeDate { get; set; }

    public int CourseId { get; set; }

    public int StudentId { get; set; }

    public virtual Course Course { get; set; } = null!;
}
