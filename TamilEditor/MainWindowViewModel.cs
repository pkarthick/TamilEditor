using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamilLib;

namespace TamilEditor
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string englishText;

        public string EnglishText
        {
            get { return englishText; }
            set
            {
                englishText = value;
                //HindiText = TamilProcessor.GetNative(englishText);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EnglishText"));
                    
                }
            }
        }

        private string hindiText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string HindiText
        {
            get { return hindiText; }
            set
            {
                hindiText = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("HindiText"));
                }
            }
        }


    }
}
