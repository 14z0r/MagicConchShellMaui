using CommunityToolkit.Maui.Views;

namespace MagicConchShellMaui.Tools
{
    public class AudioPlayer
    {
        public AudioPlayer() 
        { 
            _audioPlayer = new MediaElement();
            _audioPlayer.IsVisible = false;
            Globals.MainGrid.Add(_audioPlayer);
        }

        public enum Audios
        {
            Ja, Nein, Neein, IchGlaubeEherNicht, FragDochEinfachNochmal, KeinsVonBeiden
        }

        private readonly MediaElement _audioPlayer;

        public void Play(Audios audio)
        {
            _audioPlayer.Source = MediaSource.FromResource(audio.ToString() + ".mp3");
            _audioPlayer.Play();
        }
    }
}
