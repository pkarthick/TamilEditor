using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using TamilLib;

namespace TamilExperiment
{

    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Song> Songs { get; set; } = new ObservableCollection<Song>();

        public MainWindow()
        {
            InitializeComponent();

            SongsGrid.DataContext = Songs;

            TamilProcessor.Initialize();

            Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<Folder> folders = ProcessTopLevel(new DirectoryInfo(ConfigurationManager.AppSettings["MusicFolderPath"])).ToList();

            List<Song> songs = new List<Song>();

            foreach (var folder in folders)
            {
                GetAllSongs(folder, songs);
            }

            foreach (var song in songs)
            {
                Songs.Add(song);
            }

        }


        private void GetAllSongs(Folder folder, List<Song> allSongs)
        {
            
            foreach (var sf in folder.SubFolders)
            {
                GetAllSongs(sf, allSongs);
            }

            allSongs.AddRange(folder.Album.Songs);

        }

        private IEnumerable<Folder> ProcessTopLevel(DirectoryInfo dirInfo)
        {
            foreach (var subDirInfo in dirInfo.GetDirectories())
            {
                yield return CreateFolder(subDirInfo);
            }
        }

        private Folder CreateFolder(DirectoryInfo dirInfo)
        {
            Folder folder = new Folder(dirInfo.FullName);

            var albumDirInfo = new DirectoryInfo(folder.Album.AlbumPath);

            foreach (var subDirInfo in albumDirInfo.GetDirectories())
            {
                folder.SubFolders.Add(CreateFolder(subDirInfo));
            }
            
            foreach (var songInfo in albumDirInfo.GetFiles("*.mp3", SearchOption.TopDirectoryOnly))
            {
                folder.Album.Songs.Add(new Song(songInfo.FullName, folder.Album));
            }

            return folder;

        }


    }
}
