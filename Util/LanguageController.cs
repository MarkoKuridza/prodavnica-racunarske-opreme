using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Projekat_A_Prodavnica_racunarske_opreme.Util
{
    public class LanguageController : INotifyPropertyChanged
    {
        private static readonly LanguageController instance = new LanguageController();

        static LanguageController()
        {

        }
        private LanguageController()
        {

        }
        public static LanguageController Instance
        {
            get
            {
                return instance;
            }
        }


        private ResourceManager resourceManager = Resources.Language_en.ResourceManager;

        public ResourceManager ResourceManager
        {
            get
            {
                return resourceManager;
            }
            set
            {
                resourceManager = value;
                OnPropertyChanged(nameof(ResourceManager));
            }
        }

        public void ChangeLanguage(string langCode)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(langCode);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);

            if (langCode == "en")
            {
                ResourceManager = Resources.Language_en.ResourceManager;
            }
            else if (langCode == "sr")
            {
                ResourceManager = Resources.Language_sr.ResourceManager;
            }

            foreach (Window window in Application.Current.Windows)
            {
                if (window is ILocalizable localizable)
                {
                    localizable.ApplyInternationalization();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
