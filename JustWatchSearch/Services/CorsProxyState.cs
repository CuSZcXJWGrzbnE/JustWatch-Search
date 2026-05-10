using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

public class CorsProxyState
{
    public bool UseCorsProxy { get; private set; } = true;
    public event Action? OnChange;

    public async Task InitializeAsync(IJSRuntime js)
    {
        var val = await js.InvokeAsync<string>("localStorage.getItem", "useCorsProxy");
        if (!string.IsNullOrEmpty(val))
        {
            UseCorsProxy = val == "true";
        }
        NotifyStateChanged();
    }

    public async Task SetAsync(bool use, IJSRuntime? js = null)
    {
        UseCorsProxy = use;
        if (js is not null)
        {
            await js.InvokeVoidAsync("localStorage.setItem", "useCorsProxy", use ? "true" : "false");
        }
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
