using System;
using System.Collections.Generic;

namespace EF_MVC_CRUD.Models;

public partial class State
{
    public int StateId { get; set; }

    public string StateName { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
