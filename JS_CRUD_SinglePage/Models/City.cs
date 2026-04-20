using System;
using System.Collections.Generic;

namespace JS_CRUD_SinglePage.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public int? StateId { get; set; }

    public virtual State? State { get; set; }
}
