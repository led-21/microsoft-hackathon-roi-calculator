using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using microsoft_hackathon_roi_calculator.Web;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
                            new HttpClient { BaseAddress = new Uri("https+http://functions") });

builder.Services.AddRadzenComponents();
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();