using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TamilLib;

namespace TamilEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            TamilProcessor.Initialize();
            this.DataContext = new MainWindowViewModel();
        }

        //private string getNative(string s) {
        //    if (string.IsNullOrEmpty(s)) return " ";
        //    else return (s.StartsWith("[") && s.EndsWith("]")) ? s.Substring(1, s.Length - 2) + " " : TamilProcessor.GetNative(s) + " ";
        //}

        private void EnglishTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TamilRTB.Text = GetNative(this.EnglishTB.Text);
            Clipboard.SetText(this.TamilRTB.Text);
        }

        private string GetNative(string englishText)
        {
            string [] lines = englishText.Split(new char[] { '\n' }).Select(s => s.TrimEnd()).ToArray();

            return string.Join("\r\n", lines.Select(f => string.Join(" ", GetNativeFragments(f))));
        }

        private IEnumerable<string> GetNativeFragments(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text)) yield return text;

            if (text.Length > 0)
            {
                if (text[0] == '[')
                {
                    int endIndex = text.IndexOf(']');
                    if (endIndex == -1)
                        yield return text;
                    else
                    {
                        yield return text.Substring(1, endIndex-1);
                        if (endIndex < text.Length - 1)
                            foreach (string s in GetNativeFragments(text.Substring(endIndex + 1)))
                                yield return s;
                    }
                }
                else
                {
                    int startIndex = text.IndexOf('[');
                    if (startIndex == -1)
                        yield return TamilProcessor.GetNative(text);
                    else
                    {
                        if (startIndex > 0)
                            foreach (string s in GetNativeFragments(text.Substring(0, startIndex)))
                                yield return s;

                        if (startIndex < text.Length - 1)
                            foreach (string s in GetNativeFragments(text.Substring(startIndex)))
                                yield return s;
                    }
                }
            }
        }

    }
}
