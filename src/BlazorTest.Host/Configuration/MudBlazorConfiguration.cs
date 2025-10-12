using MudBlazor;

namespace BlazorTest.Host.Configuration;

/// <summary>
/// MudBlazor theme configuration
/// </summary>
public static class MudBlazorConfiguration
{
    /// <summary>
    /// Gets the default MudBlazor theme
    /// </summary>
    public static MudTheme DefaultTheme => new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#594AE2",
            Secondary = "#FF4081",
            Tertiary = "#1EC8A5",
            AppbarBackground = "#594AE2",
            AppbarText = "#FFFFFF",
            Background = "#F5F5F5",
            Surface = "#FFFFFF",
            DrawerBackground = "#FFFFFF",
            DrawerText = "rgba(0,0,0, 0.87)",
            Success = "#4CAF50",
            Info = "#2196F3",
            Warning = "#FF9800",
            Error = "#F44336",
            Dark = "#424242"
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#776BE7",
            Secondary = "#FF4081",
            Tertiary = "#1EC8A5",
            AppbarBackground = "#1E1E1E",
            AppbarText = "#FFFFFF",
            Background = "#121212",
            Surface = "#1E1E1E",
            DrawerBackground = "#1E1E1E",
            DrawerText = "#FFFFFF",
            Success = "#4CAF50",
            Info = "#2196F3",
            Warning = "#FF9800",
            Error = "#F44336"
        },
        LayoutProperties = new LayoutProperties
        {
            DrawerWidthLeft = "260px",
            DrawerWidthRight = "300px",
            AppbarHeight = "64px"
        },
        Typography = new Typography
        {
            Default = new Default
            {
                FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                FontSize = "0.875rem",
                FontWeight = 400,
                LineHeight = 1.43,
                LetterSpacing = "0.01071em"
            },
            H1 = new H1
            {
                FontSize = "6rem",
                FontWeight = 300,
                LineHeight = 1.167,
                LetterSpacing = "-0.01562em"
            },
            H2 = new H2
            {
                FontSize = "3.75rem",
                FontWeight = 300,
                LineHeight = 1.2,
                LetterSpacing = "-0.00833em"
            },
            H3 = new H3
            {
                FontSize = "3rem",
                FontWeight = 400,
                LineHeight = 1.167,
                LetterSpacing = "0em"
            },
            H4 = new H4
            {
                FontSize = "2.125rem",
                FontWeight = 400,
                LineHeight = 1.235,
                LetterSpacing = "0.00735em"
            },
            H5 = new H5
            {
                FontSize = "1.5rem",
                FontWeight = 400,
                LineHeight = 1.334,
                LetterSpacing = "0em"
            },
            H6 = new H6
            {
                FontSize = "1.25rem",
                FontWeight = 500,
                LineHeight = 1.6,
                LetterSpacing = "0.0075em"
            },
            Button = new Button
            {
                FontSize = "0.875rem",
                FontWeight = 500,
                LineHeight = 1.75,
                LetterSpacing = "0.02857em",
                TextTransform = "uppercase"
            }
        }
    };
}

