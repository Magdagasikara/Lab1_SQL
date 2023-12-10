using System;
using System.Collections.Generic;

namespace Lab1_SQL.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int ClassId { get; set; }

    public virtual Class Class { get; set; } = null!;
}
