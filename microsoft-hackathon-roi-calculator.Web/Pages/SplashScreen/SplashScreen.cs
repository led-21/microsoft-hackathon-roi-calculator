using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace microsoft_hackathon_roi_calculator.Web.Pages.SplashScreen;
public static class DialogSplashScreen
{
    static public async Task ShowSplashScreen(IDialogService DialogService)
    {
        var parameters = new DialogParameters<SplashScreenContent>
        {
            Content = new SplashScreenContent
            {
                Title = "Loading...",
                SubTitle = "InovaROI: Soluções Inteligentes para Retorno Máximo",
                LoadingText = "Loading...",
                Message = (MarkupString)"<strong>Loading</strong> aguarde um pouco...",
                Logo = FluentSplashScreen.LOGO
            },
            Width = "640px",
            Height = "480px",
            Modal = true,
        };

        var dialog = await DialogService.ShowSplashScreenAsync(parameters);

        // Simulate a loading process
        await Task.Delay(3000);

        await dialog.CloseAsync();
    }
}