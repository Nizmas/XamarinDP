using App2.Models;
using App2.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class WorkPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public WorkPageViewModel()
        {
            //var modulesRes = await GetModulesAsync();
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        /*private async System.Threading.Tasks.Task<UserLogin> GetModulesAsync()
        {
        }*/
    }
}
