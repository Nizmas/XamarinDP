using Xamarin.Forms;
using App2.ViewModels;

namespace App2.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new SignInViewModel() { Navigation = this.Navigation };
        }
    }
}
