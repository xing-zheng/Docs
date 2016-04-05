using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;

public class CountryViewModelEmpty
{
    public string Country { get; set; }

    public List<SelectListItem> Countries { get; private set; }

    public CountryViewModelEmpty()
    {         
        Countries = new List<SelectListItem>
           {
               new SelectListItem {Value = "",   Text = "<none>" },
               new SelectListItem {Value = "MX", Text = "Mexico" },
               new SelectListItem {Value = "CA", Text = "Canada" },
               new SelectListItem {Value = "US", Text = "USA"    }
           };
    }
}

