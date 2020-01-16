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

        public EditAssessmentsPage (Assessments _assessment)
		{
			InitializeComponent ();

            _currentAssessment = _assessment;

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();

		}

        protected override async void OnAppearing()
        {
            await conn.CreateTableAsync<Assessments>();
            

            assessmentName.Text = _currentAssessment.AssessmentName;
            assessmentTypePicker.SelectedItem = _currentAssessment.AssessmentType;
            assessStartDatePicker.Date = _currentAssessment.AssessmentStart;
            assessEndDatePicker.Date = _currentAssessment.AssessmentEnd;

            base.OnAppearing();
        }

        private async void SaveAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await conn.CreateTableAsync<Assessments>();
            var assessList =
                await conn.QueryAsync<Assessments>($"SELECT Type " +
                $"FROM Assessments " +
                $"WHERE CourseId = '{_currentAssessment.CourseId}'");
            var updatedAssessment = _currentAssessment;
            updatedAssessment.AssessmentName = assessmentName.Text;
            updatedAssessment.AssessmentType = (string)assessmentTypePicker.SelectedItem;
            updatedAssessment.AssessmentStart = assessStartDatePicker.Date;
            updatedAssessment.AssessmentEnd = assessEndDatePicker.Date;

            foreach (Assessments assessments in assessList)
            {
                if (updatedAssessment.AssessmentType == assessments.AssessmentType)
                {
                    await DisplayAlert("Warning",
                        "There may only be one of each assessment type per course." +
                        " Please try again", "Ok");
                }
                else
                {
                    await conn.UpdateAsync(updatedAssessment);
                    await DisplayAlert("Notice", "Assessessment Successfully Updated", "Ok");
                    await Navigation.PopModalAsync();
                }
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
    }
}