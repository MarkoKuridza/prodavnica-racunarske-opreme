using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Threading;
using Projekat_A_Prodavnica_racunarske_opreme.Util;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;

namespace Projekat_A_Prodavnica_racunarske_opreme
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //po default-u je engleski jezik i light tema
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LanguageController.Instance.ChangeLanguage("en");
            
        }

        
    }
}
