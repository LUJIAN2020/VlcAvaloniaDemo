using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using LibVLCSharp.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace VlcAvaloniaDemo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //Linux使用方法
        //Ubuntu下运行一下这两个命令安装包
        //sudo apt install libvlc-dev
        //sudo apt install vlc

        string mrl = "";


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

        public async Task SelectVideoAsyncCommand()
        {
            if (App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
                if (topLevel is not null)
                {
                    var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                    {
                        Title = "Open Video File",
                        AllowMultiple = false
                    });

                    if (files.Count >= 1)
                    {
                        mrl = files[0].Path.LocalPath;
                    }
                }
            }
        }
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
                            if (mrl is not null)
                            {
                                using var media = new Media(_libVlc, mrl);
                                MyMediaPlayer?.Play(media);
                            }
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
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = default)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetProperty<T>(ref T field, T value, string? propertyName = default)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}