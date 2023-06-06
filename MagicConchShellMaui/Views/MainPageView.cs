using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicConchShellMaui.Tools;
using System.Diagnostics;
using System.Globalization;

namespace MagicConchShellMaui.Views
{
    public partial class MainPageView : ObservableObject
    {
        private readonly ISpeechToText speechToText;
        private readonly DecideAnswer decideAnswer;
        [ObservableProperty]
        private string question;

        public MainPageView(ISpeechToText speechToText)
        {
            this.speechToText = speechToText;
            decideAnswer = new DecideAnswer();
        }

        [RelayCommand]
        async Task Evaluate()
        {
            await decideAnswer.Evaluate(Question);
            Question = string.Empty;
        }

        [RelayCommand(IncludeCancelCommand = true)]
        async Task Listen(CancellationToken cancellationToken)
        {
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName != "de")
            {
                await Toast.Make("Sorry, aber die App unterstüzt nur die deutsche Sprachen. :/").Show(CancellationToken.None);
                return;
            }

            var isGranted = await speechToText.RequestPermissions(cancellationToken);
            if (!isGranted)
            {
                await Toast.Make("Sie müssen den Mikrofonzugriff zulassen, sonst geht das hier nicht.").Show(CancellationToken.None);
                return;
            }

            try
            {
                var recognitionResult = await speechToText.ListenAsync(CultureInfo.CurrentCulture,
                                                new Progress<string>(), cancellationToken);

                recognitionResult.EnsureSuccess();

                Question = recognitionResult.Text;

                Evaluate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Toast.Make($"Fehler mit der Spracherkennung: {ex.Message}").Show(cancellationToken);
            }

            Question = string.Empty;
        }
    }
}