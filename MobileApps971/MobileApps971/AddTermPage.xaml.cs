using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using MobileApps971.Model;
using System.Collections.ObjectModel;

namespace MobileApps971
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddTermPage : ContentPage
	{
        private SQLiteAsyncConnection conn;
        private MainPage mainPage;
        public AddTermPage (MainPage _mainPage)
		{
			InitializeComponent ();

            mainPage = _mainPage;

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();

            addTermHearderLabel.Text = "Add New Term";
		}

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var newTerm = new Terms();
            newTerm.TermName = termName.Text;
            newTerm.StartDate = startDatePicker.Date;
            newTerm.EndDate = endDatePicker.Date;

            //Date Validtation
            if (newTerm.StartDate <= newTerm.EndDate)
            {
                await conn.InsertAsync(newTerm);

                mainPage._termList.Add(newTerm);
                await DisplayAlert("Success!", $"{newTerm.TermName}" + " Added", "Ok");
                await Navigation.PopModalAsync(); 
            }
            else
            {
                await DisplayAlert("Warning!", "Start date must be earlier than end date!", "Ok");
            }
        }
    }
}