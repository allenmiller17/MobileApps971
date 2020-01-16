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
    public partial class EditAssessmentsPage : ContentPage
    {

        private SQLiteAsyncConnection conn;
        private Assessments _currentAssessment;

        public EditAssessmentsPage(Assessments _assessment)
        {
            InitializeComponent();

            _currentAssessment = _assessment;

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();

        }

        protected override async void OnAppearing()
        {
            await conn.CreateTableAsync<Assessments>();


            assessmentName.Text = _currentAssessment.AssessmentName;
            assessmentTypePicker.Text = "Type: "+_currentAssessment.AssessmentType;
            assessStartDatePicker.Date = _currentAssessment.AssessmentStart;
            assessEndDatePicker.Date = _currentAssessment.AssessmentEnd;
            notificationsSwitch.IsToggled = _currentAssessment.AssessmentNotifications == 1 ? true : false;

            base.OnAppearing();
        }

        private async void SaveAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await conn.CreateTableAsync<Assessments>();

            var updatedAssessment = _currentAssessment;
            updatedAssessment.AssessmentName = assessmentName.Text;
            updatedAssessment.AssessmentStart = assessStartDatePicker.Date;
            updatedAssessment.AssessmentEnd = assessEndDatePicker.Date;
            updatedAssessment.AssessmentNotifications = notificationsSwitch.IsToggled == true ? 1 : 0;

            //Makes sure no Null fields exist
            if (!HelperClass.IsNull(assessmentName.Text) && !HelperClass.IsNull(assessStartDatePicker.Date.ToShortDateString())
                && !HelperClass.IsNull(assessEndDatePicker.Date.ToShortDateString()))
            {
                //Date Validation
                if (updatedAssessment.AssessmentStart <= updatedAssessment.AssessmentEnd)
                {
                    await conn.UpdateAsync(updatedAssessment);
                    await DisplayAlert("Notice", "Assessessment Successfully Updated", "Ok");
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

        private async void DeleteAssessmentButton_Clicked(object sender, EventArgs e)
        {
            var deleteResponse = await DisplayAlert("Warning!", "Your are about to delete " + $"{_currentAssessment.AssessmentName}" + "! Do you wish to continue?", "Yes", "No");
            if (deleteResponse)
            {
                await conn.DeleteAsync(_currentAssessment);
                await Navigation.PopModalAsync();
            }
        }

        private void NotificationsSwitch_Toggled(object sender, ToggledEventArgs e)
        {

        }
    }
}