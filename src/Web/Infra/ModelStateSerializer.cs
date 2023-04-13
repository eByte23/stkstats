using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace STKBC.Stats.Pages.Infra;

public static class ModelStateSerializer
{
    public class ModelStateTransferValue
    {
        public string? Key { get; set; }
        public string? AttemptedValue { get; set; }
        public object? RawValue { get; set; }
        public ICollection<string> ErrorMessages { get; set; } = new List<string>();
    }

    public static string Serialize(ModelStateDictionary modelState)
    {
        var errorList = modelState
            .Select(kvp => new ModelStateTransferValue
            {
                Key = kvp.Key,
                AttemptedValue = kvp.Value!.AttemptedValue,
                RawValue = kvp.Value!.RawValue,
                ErrorMessages = kvp.Value!.Errors.Select(err => err.ErrorMessage).ToList(),
            }).ToList();

        return System.Text.Json.JsonSerializer.Serialize(errorList);
    }

    public static ModelStateDictionary Deserialize(string serializedErrorList)
    {
        var errorList = System.Text.Json.JsonSerializer.Deserialize<List<ModelStateTransferValue>>(serializedErrorList);
        var modelState = new ModelStateDictionary();

        if (errorList == null)
            return modelState;

        foreach (var item in errorList)
        {
            modelState.SetModelValue(item.Key!, item.RawValue, item.AttemptedValue);
            foreach (var error in item.ErrorMessages)
                modelState.AddModelError(item.Key!, error);
        }
        return modelState;
    }

    public static (ModelStateDictionary, bool) GetModelState(this ITempDataDictionary tempData)
    {
        var modelState = new ModelStateDictionary();

        var serializedModelState = tempData[SerializeModelStateFilter.Key] as string;
        if (string.IsNullOrWhiteSpace(serializedModelState))
            return (modelState, false);

        modelState.Merge(ModelStateSerializer.Deserialize(serializedModelState));

        if (modelState.Count == 0)
            return (modelState, false);

        return (modelState, true);
    }

    public static T GetValue<T>(this ModelStateDictionary modelState, string key)
    {
        if (modelState.TryGetValue(key, out var value) && value.RawValue != null && value.AttemptedValue != null)
        {
            if (value.RawValue is T t)
            {
                return t;
            }
            else if (value.RawValue is System.Text.Json.JsonElement jsonEl)
            {
                var json = jsonEl.GetRawText();

                if (string.IsNullOrWhiteSpace(json) || string.IsNullOrEmpty(value.AttemptedValue))
                {
                    return default!;
                }

                return JsonSerializer.Deserialize<T>(json)!;
            }


            return (T)value.RawValue;
        }
        return default!;
    }
}