using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using microsoft_hackathon_roi_calculator.Web;
using microsoft_hackathon_roi_calculator.Web.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Default HttpClient points to Blazor WASM host origin so relative assets load reliably
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
    Timeout = TimeSpan.FromMinutes(2)
});

builder.Services.AddScoped<AppStateService>();
builder.Services.AddScoped<RoiApiClient>();
builder.Services.AddScoped<MarkdownService>();

builder.Services.AddRadzenComponents();
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();