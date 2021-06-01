using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using VK_Images.API;
using VK_Images.Commands;
using VK_Images.Enums;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;

namespace VK_Images.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        private readonly IDataService _wall;
        private readonly Album _album;

        private readonly string DefaultAvatar =
            "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/bae20f47-fe9e-4ab7-87c0-c461fc3b9d83/d45jvvb-ed95931a-0f66-459f-9214-8e23209f4b3d.png/v1/fill/w_900,h_851,q_75,strp/cirno_shrug__lol_youmu_edit_by_yukirumo990-d45jvvb.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwic3ViIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsImF1ZCI6WyJ1cm46c2VydmljZTppbWFnZS5vcGVyYXRpb25zIl0sIm9iaiI6W1t7InBhdGgiOiIvZi9iYWUyMGY0Ny1mZTllLTRhYjctODdjMC1jNDYxZmMzYjlkODMvZDQ1anZ2Yi1lZDk1OTMxYS0wZjY2LTQ1OWYtOTIxNC04ZTIzMjA5ZjRiM2QucG5nIiwid2lkdGgiOiI8PTkwMCIsImhlaWdodCI6Ijw9ODUxIn1dXX0.Ypbbc6xO96BTc1L2Hpl_SL7cKCVod5iwFHHVnkRDgak";

        public ShellViewModel(IDataService wall, Album album)
        {
            UserData = new UserData();

            _userData.AvatarUrl = DefaultAvatar;
            UserData.Name = "???";
            UserData.id = 11111;

            _wall = wall;
            _album = album;
        }


        private UserData _userData;

        public UserData UserData
        {
            get => _userData;
            set
            {
                _userData = value;
                OnPropertyChanged(nameof(UserData));
            }
        }


        private string _accessToken;

        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                OnPropertyChanged(nameof(AccessToken));

                if (OwnerId != null && _option == Options.Album)
                {
                    AlbumSetInfo();

                }
                else if (OwnerId != null && _option == Options.Wall)
                {
                    WallSetInfo();
                }
            }
        }

        private string _ownerId;

        public string OwnerId
        {
            get => _ownerId;
            set
            {
                _ownerId = value;
                OnPropertyChanged(nameof(OwnerId));

                if (AccessToken != null && _option == Options.Album)
                {
                    AlbumSetInfo();
                }
                else if (AccessToken != null && _option == Options.Wall)
                {
                    WallSetInfo();

                }
            }
        }

        private int _imagesCount;

        public int ImagesCount
        {
            get => _imagesCount;
            set
            {
                _imagesCount = value;
                OnPropertyChanged(nameof(ImagesCount));
            }
        }

        private int _offset;

        public int Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                OnPropertyChanged(nameof(Offset));
            }
        }

        private RelayCommand _optionCommand;

        public RelayCommand OptionCommand
        {
            get
            {
                return _optionCommand ??= new RelayCommand(obj =>
                {
                    if (obj != null)
                    {
                        _option = (Options)obj;

                        if (AccessToken != null && OwnerId != null && _option == Options.Album)
                        {
                            AlbumSetInfo();
                        }

                        else if (AccessToken != null && OwnerId != null && _option == Options.Wall)
                        {
                            WallSetInfo();
                        }
                    }
                });
            }

        }
                                                           
        private AsyncRelayCommand _downloadImages;

        public AsyncRelayCommand DownloadImages
        {
            get
            {
                return _downloadImages ??= new AsyncRelayCommand(async obj =>
                {
                    if (_option == Options.Album)
                    {
                        var images = await _album.GetImagesAsync(AccessToken, OwnerId, ImagesCount, Offset);
                        await DownloadData(images);
                        MessageBox.Show("Your images were successfully downloaded to the images folder", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (_option == Options.Wall)
                    {
                        var images = await _wall.GetImagesAsync(AccessToken, OwnerId, ImagesCount, Offset);
                        await DownloadData(images);
                        MessageBox.Show("Your images were successfully downloaded to the images folder", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }, obj => UserData.id != 11111 && ImagesCount > 0 && Offset >= 0);
            }
        }

        private RelayCommand _openLink;

        public RelayCommand OpenLink
        {
            get
            {
                return _openLink ??= new RelayCommand(obj =>
                    { 
                        var url = "https://devman.org/qna/63/kak-poluchit-token-polzovatelja-dlja-vkontakte/";
                        try
                        {

                            Process.Start(url);
                        }
                        catch
                        {
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            {
                                url = url.Replace("&", "^&");
                                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") {CreateNoWindow = true});
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            {
                                Process.Start("xdg-open", url);
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                            {
                                Process.Start("open", url);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    });
            }
        }

        

        public void WallSetInfo()
        {
            _wall.GetInfoAsync(AccessToken, OwnerId).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    if (task.Result.AvatarUrl == null)
                        UserData = new UserData
                        {
                            AvatarUrl = DefaultAvatar,
                            id = 11111,
                            Name = "Owner ID/Access Token is invalid"
                        };
                    else
                        UserData = task.Result;
                }
            });
        }

        public void AlbumSetInfo()
        {
            _album.GetInfoAsync(AccessToken, OwnerId).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    if (task.Result.AvatarUrl == null)
                        UserData = new UserData
                        {
                            AvatarUrl = DefaultAvatar,
                            id = 11111,
                            Name = "Owner ID/Access Token is invalid"
                        };
                    else
                        UserData = task.Result;
                }
            });
        }
        public async Task DownloadData(IEnumerable<string> data)
        {
            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }

            using (WebClient client = new WebClient())
            {
                foreach (var item in data)
                {
                    if (!client.IsBusy)
                        await client.DownloadFileTaskAsync(new Uri(item),$"Images/{Path.GetFileName(item).Split('?')[0]}");
                }
            }
        }

        private Options _option = Options.Album;
    }
}
