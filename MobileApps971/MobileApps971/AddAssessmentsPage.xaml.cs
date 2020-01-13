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

		public AddAssessmentsPage (Courses currentCourse)
		{
			InitializeComponent ();

            addAssessmentsHearderLabel.Text = "Add Assessments";

            _currentCourse = currentCourse;
            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();
		}

        private async void SaveAssessmentButton_Clicked(object sender, EventArgs e)
        {
            var newAssessment = new Assessments();
            newAssessment.AssessmentName = assessmentName.Text;
            newAssessment.AssessmentType = (string)assessmentTypePicker.SelectedItem;
            newAssessment.AssessmentStart = assessStartDatePicker.Date;
            newAssessment.AssessmentEnd = assessEndDatePicker.Date;
            //TODO add notifications

            await conn.InsertAsync(newAssessment);
            await DisplayAlert("Notice", "Assessment Added to Course", "Ok");
            await Navigation.PopModalAsync();
        }
    }
}