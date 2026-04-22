using System;
using System.Collections.Generic;

namespace EF_MVC_CRUD.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int StateId { get; set; }

    public DateOnly Dob { get; set; }

    public string? ImagePath { get; set; }

    public string? Technologies { get; set; }

    public virtual State State { get; set; } = null!;
}
