using System;
using System.Collections.Generic;

namespace JS_CRUD_SinglePage.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public int? StateId { get; set; }

    public int? CityId { get; set; }

    public string? Technologies { get; set; }

    public string? ProfileImage { get; set; }
}
