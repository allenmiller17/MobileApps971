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
	public partial class AddAssessmentsPage : ContentPage
	{

        private SQLiteAsyncConnection conn;
        private Courses _currentCourse;

		public AddAssessmentsPage (Courses _course)
		{
			InitializeComponent ();

            addAssessmentsHearderLabel.Text = "Add Assessments";

            _currentCourse = _course;
            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();
		}

        protected override async void OnAppearing()
        {
            await conn.CreateTableAsync<Assessments>();
            var assessList = 
                await conn.QueryAsync<Assessments>($"SELECT Type " +
                $"FROM Assessments " +
                $"WHERE CourseId = '{_currentCourse.CourseId}'");

            foreach (Assessments assessments in assessList)
            {
                if (String.IsNullOrEmpty(assessments.AssessmentType))
                {
                    assessmentTypePicker.Items.Add("Objective Assessment");
                    assessmentTypePicker.Items.Add("Performance Assessment");
                }
                else if (assessments.AssessmentType == "Objective Assessment")
                {
                    assessmentTypePicker.Items.Add("Performance Assessment");
                }
                else
                {
                    assessmentTypePicker.Items.Add("Objective Assessment");
                }
            }
        }

        private async void SaveAssessmentButton_Clicked(object sender, EventArgs e)
        {
            var newAssessment = new Assessments();
            newAssessment.AssessmentName = assessmentName.Text;
            newAssessment.AssessmentType = (string)assessmentTypePicker.SelectedItem;
            newAssessment.AssessmentStart = assessStartDatePicker.Date;
            newAssessment.AssessmentEnd = assessEndDatePicker.Date;
            newAssessment.CourseId = _currentCourse.CourseId;
            //TODO add notifications

            await conn.InsertAsync(newAssessment);
            await DisplayAlert("Notice", "Assessment Added to Course", "Ok");
            await Navigation.PopModalAsync();
        }
    }
}