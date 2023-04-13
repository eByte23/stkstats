using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace STKBC.Stats.Pages;

public static class HtmlUtils
{
    public static SelectListItem MapToSelectItem(string label, string value, bool? selected = null, bool? disabled = null)
    {
        return new SelectListItem(label, value, selected ?? false, disabled ?? false);
    }

}