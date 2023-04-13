using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace STKBC.Stats.Pages.Infra;

public class SerializeModelStateFilter : IPageFilter
{
    public static readonly string Key = $"{nameof(SerializeModelStateFilter)}Data";
    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        // if (!(context.HandlerInstance is PageModel page))
        //     return;

        // var serializedModelState = page.TempData[Key] as string;
        // if (string.IsNullOrWhiteSpace(serializedModelState))
        //     return;

        // var modelState = ModelStateSerializer.Deserialize(serializedModelState);
        // page.ModelState.Merge(modelState);
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
    }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {

    }
}