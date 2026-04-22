using System;
using System.Collections.Generic;

namespace EF_MVC_CRUD.Models;

public partial class Technology
{
    public int TechnologyId { get; set; }

    public string TechnologyName { get; set; } = null!;
}
