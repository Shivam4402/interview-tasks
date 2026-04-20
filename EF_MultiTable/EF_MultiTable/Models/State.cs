using System;
using System.Collections.Generic;

namespace EF_MultiTable.Models;

public partial class State
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
