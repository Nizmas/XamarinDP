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
    public class SignInViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SignInCommand { protected set; get; }
        public UserLogin userLogin { get; private set; }
        public INavigation Navigation { get; set; }
        public SignInViewModel()
        {
            SignInCommand = new Command(async () => await SignInAsync());
            userLogin = new UserLogin();
        }

        public string Login
        {
            get { return userLogin.Login; }
            set
            {
                if (userLogin.Login != value)
                {
                    userLogin.Login = value;
                    OnPropertyChanged("Login");
                }
            }
        }
        public string Password
        {
            get { return userLogin.Password; }
            set
            {
                if (userLogin.Password != value)
                {
                    userLogin.Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async System.Threading.Tasks.Task SignInAsync()
        {
            var adresses = new List<string> { "http://172.18.81.17:5000/", "http://192.168.0.105:5000/" };

            var client = new HttpClient();
            client.BaseAddress = new Uri(adresses[0]);
            client.Timeout = TimeSpan.FromSeconds(5);

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("username", Login));
            postData.Add(new KeyValuePair<string, string>("password", Password));

            var res = new HttpResponseMessage();
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, adresses[1] + "token") { Content = new FormUrlEncodedContent(postData) };
                res = await client.SendAsync(req);
            }
            catch
            {
                try
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, adresses[0] + "token") { Content = new FormUrlEncodedContent(postData) };
                    res = await client.SendAsync(req);
                }
                catch
                {
                    Android.Widget.Toast.MakeText(Android.App.Application.Context, "Нет подключения к серверу!", Android.Widget.ToastLength.Long).Show();
                }
                finally
                {
                    Android.Widget.Toast.MakeText(Android.App.Application.Context, "Всё плохо...", Android.Widget.ToastLength.Long).Show();
                }
            }
            finally
            {
                //Добавить лоадер

                string result = await res.Content.ReadAsStringAsync();

                if (res.IsSuccessStatusCode)
                {
                    var userData = JsonConvert.DeserializeObject<AccessToken>(result);
                    Application.Current.Properties["token"] = userData.access_token;

                    await Navigation.PushAsync(new WorkPage());
                }
                else 
                {
                    string tosteMessage = "";
                    if (result == "-3")
                    {
                        tosteMessage = "Вроде, надо сменить пароль";
                    }
                    else if (result == "-4")
                    {
                        tosteMessage = "Вроде, надо сменить пароль";
                    }
                    else if (result == "-6")
                    {
                        tosteMessage = "Пользователь временно заблокирован";
                    }
                    else if (result == "-7")
                    {
                        tosteMessage = "Выполнен вход под одним логином с разных ip";
                    }
                    else if (result == "-8")
                    {
                        tosteMessage = "Выполнен вход под разными логинами с одного IP-адреса";
                    }
                    else if (result == "-9")
                    {
                        tosteMessage = "Пользователь заблокирован администратором";
                    }
                    else
                        tosteMessage = "Неправильный логин или пароль";

                    Android.Widget.Toast.MakeText(Android.App.Application.Context, tosteMessage, Android.Widget.ToastLength.Long).Show();
                }
            }
        }
    }

}
