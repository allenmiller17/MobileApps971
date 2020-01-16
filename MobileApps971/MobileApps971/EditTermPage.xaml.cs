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
    public partial class EditTermPage : ContentPage
    {
        private SQLiteAsyncConnection conn;
        private Terms _currentTerm;
        public EditTermPage(Terms currentTerm)
        {
            InitializeComponent();

            _currentTerm = currentTerm;
            editTermHearderLabel.Text = "Edit " + _currentTerm.TermName;

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await conn.CreateTableAsync<Terms>();

            termName.Text = _currentTerm.TermName;
            startDatePicker.Date = _currentTerm.StartDate;
            endDatePicker.Date = _currentTerm.EndDate;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var updateTerm = _currentTerm;
            updateTerm.TermName = termName.Text;
            updateTerm.StartDate = startDatePicker.Date;
            updateTerm.EndDate = endDatePicker.Date;

            //Makes sure No Null Fields Exist
            if (!HelperClass.IsNull(termName.Text) && !HelperClass.IsNull(startDatePicker.Date.ToShortDateString()) && !HelperClass.IsNull(endDatePicker.Date.ToShortDateString()))
            {
                //Date Validtation
                if (updateTerm.StartDate <= updateTerm.EndDate)
                {

                    await conn.UpdateAsync(updateTerm);
                    await DisplayAlert("Notice", $"{_currentTerm.TermName}" + " Updated", "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Warning!", "Start date must be earlier than end date!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Warning!", "All fields are required. Please try again!", "Ok");
            }

        }
    }
}