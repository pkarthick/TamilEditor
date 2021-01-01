using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamilLib;

namespace TamilExperiment
{
    public class Folder 
    {
        public string FolderPath { get; set; }

        public Folder(string path)
        {
            FolderPath = path;
            Album = new Album(path);
        }

        public string Name { get { return new DirectoryInfo(FolderPath).Name; } }

        public Album Album { get; set; }

        public List<Folder> SubFolders { get; set; } = new List<Folder>();
        
         
    }

    public class Song : BindableBase
    {

        private Album album;

        public Album Album
        {
            get { return album; }
            private set { SetProperty(ref album, value); }
        }
        
        public Song(string songFilePath, Album album)
        {
            Album = album;
            SongPath = songFilePath;
            NativeName = Path.GetFileNameWithoutExtension(songFilePath);
        }

        private string songPath;

        public string SongPath
        {
            get
            {
                return songPath;
            }
            set
            {
                SetProperty(ref songPath, value);
            }
        }
          
        
        private string englishName;

        public string EnglishName
        {
            get { return englishName; }
            set
            {
                SetProperty(ref englishName, value);
                NativeName = value;
            }
        }

        private string nativeName;

        public string NativeName
        {
            get { return nativeName; }
            set
            {

                if (!TamilProcessor.IsNative(value))
                {
                    string songFilePath = Path.Combine(Album.AlbumPath, songPath);
                    
                    SetProperty(ref nativeName, TamilProcessor.GetNative(value));

                    string newSongPath = Path.Combine(Path.GetDirectoryName(SongPath), nativeName + Path.GetExtension(SongPath));

                    if (File.Exists(songFilePath) && !File.Exists(newSongPath) && songFilePath != newSongPath)
                    {
                        File.Move(songFilePath, newSongPath);
                        SongPath = newSongPath;
                    }

                }
                else
                {
                    nativeName = value;
                }

                englishName = TamilProcessor.GetEnglish(nativeName);

            }
        }
        

    }

    public class Album : BindableBase
    {
        
        public ObservableCollection<Song> Songs { get; private set; } = new ObservableCollection<Song>();

        public Album(string albumPath)
        {
            AlbumPath = albumPath;
            NativeName = new DirectoryInfo(albumPath).Name;
        }

        private string albumPath;

        public string AlbumPath
        {
            get
            {
                
                return albumPath;
            }
            set
            {
               

                SetProperty(ref albumPath, value);
                
            }
        }
        
        private string englishName;

        public string EnglishName
        {
            get { return englishName; }
            set
            {
                SetProperty(ref englishName, value);
                NativeName = value;
            }
        }

        private string nativeName;

        public string NativeName
        {
            get { return nativeName; }
            set
            {

                if (!TamilProcessor.IsNative(value))
                {
                    
                    SetProperty(ref nativeName, TamilProcessor.GetNative(value));

                    if (!string.IsNullOrEmpty(AlbumPath))
                    {
                        var oldDirInfo = new DirectoryInfo(AlbumPath);

                        if (oldDirInfo.Exists && nativeName != oldDirInfo.Name)
                        {
                            AlbumPath = Path.Combine(oldDirInfo.Parent.FullName, nativeName);
                            oldDirInfo.MoveTo(AlbumPath);
                        }
                    }

                }
                else
                {
                    SetProperty(ref nativeName, value);
                }

                englishName = TamilProcessor.GetEnglish(nativeName);
                OnPropertyChanged("AlbumEnglishName");

            }
        }

    }
}
