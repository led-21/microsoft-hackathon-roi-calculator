namespace microsoft_hackathon_roi_calculator.Mobile.View;

public class Dashboard : ContentPage
{
    public Dashboard()
    {
        Content = new VerticalStackLayout
        {
            Children = {
                new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
                },
                new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Adriano"
                }
            }
        };
    }
}