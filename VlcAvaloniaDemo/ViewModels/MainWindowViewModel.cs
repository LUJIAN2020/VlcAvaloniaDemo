using CommunityToolkit.Mvvm.Input;
using LibVLCSharp.Shared;

namespace VlcAvaloniaDemo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        //Linux使用方法
        //Ubuntu下运行一下这两个命令安装包
        //sudo apt install libvlc-dev
        //sudo apt install vlc

        private readonly LibVLC _libVlc = new LibVLC();
        public MainWindowViewModel()
        {
            MyMediaPlayer = new MediaPlayer(_libVlc);
        }

        private MediaPlayer? myMediaPlayer;
        public MediaPlayer? MyMediaPlayer
        {
            get { return myMediaPlayer; }
            set { SetProperty(ref myMediaPlayer, value); }
        }


        [RelayCommand]
        public void OptionsCommand(string content)
        {
            switch (content)
            {
                case "播放":
                    {
                        if (MyMediaPlayer?.State == VLCState.Paused)
                        {
                            MyMediaPlayer?.Play();
                        }
                        else
                        {
                            using var media = new Media(_libVlc, "Avalonia XPF.mp4");
                            MyMediaPlayer?.Play(media);
                        }
                    }
                    break;
                case "停止":
                    MyMediaPlayer?.Stop();
                    break;
                case "暂停":
                    MyMediaPlayer?.Pause();
                    break;
                default:
                    break;
            }
        }






    }
}