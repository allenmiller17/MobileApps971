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
	public partial class EditCoursePage : ContentPage
	{
        private SQLiteAsyncConnection conn;
        private Courses _currentCourse;

		public EditCoursePage (Courses currentCourse)
		{
			InitializeComponent ();

            editCourseHearderLabel.Text = "Edit "+currentCourse.CourseName;

            _currentCourse = currentCourse;

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();
		}

        protected override async void OnAppearing()
        {
            await conn.CreateTableAsync<Courses>();

            courseName.Text = _currentCourse.CourseName;
            startDatePicker.Date = _currentCourse.CourseStartDate;
            endDatePicker.Date = _currentCourse.CourseEndDate;
            courseStatusPicker.SelectedItem = _currentCourse.Status;
            instructorNameEntry.Text = _currentCourse.CourseInstructorName;
            instructorPhoneEntry.Text = _currentCourse.CourseInstructorPhone;
            instructorEmailEntry.Text = _currentCourse.CourseInstructorEmail;

            base.OnAppearing();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var updatedCourse = _currentCourse;
            updatedCourse.CourseName = courseName.Text;
            updatedCourse.CourseStartDate = startDatePicker.Date;
            updatedCourse.CourseEndDate = endDatePicker.Date;
            updatedCourse.Status = (string)courseStatusPicker.SelectedItem;
            updatedCourse.CourseInstructorName = instructorNameEntry.Text;
            updatedCourse.CourseInstructorPhone = instructorPhoneEntry.Text;
            updatedCourse.CourseInstructorEmail = instructorEmailEntry.Text;

            if (!HelperClass.IsNull(courseName.Text) && !HelperClass.IsNull(startDatePicker.Date.ToShortDateString()) &&
                !HelperClass.IsNull(endDatePicker.Date.ToShortDateString()) &&
                !HelperClass.IsNull((string)courseStatusPicker.SelectedItem) && !HelperClass.IsNull(instructorNameEntry.Text) &&
                !HelperClass.IsNull(instructorPhoneEntry.Text) && !HelperClass.IsNull(instructorEmailEntry.Text))
            {
                if (HelperClass.EmailIsValid(instructorEmailEntry.Text))
                {
                    if (HelperClass.PhoneIsValid(instructorPhoneEntry.Text))
                    {
                        if (updatedCourse.CourseStartDate <= updatedCourse.CourseEndDate)
                        {
                            await conn.UpdateAsync(updatedCourse);
                            await DisplayAlert("Notice", $"{_currentCourse.CourseName}" + " Updated", "Ok");
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            await DisplayAlert("Warning!", "Start date must be earlier than end date!", "Ok");
                        } 
                    }
                    else
                    {
                        await DisplayAlert("Warning!", "Course instructor phone must only contain 10 digits!", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Warning!", "Course instructor must have a valid @WGU.edu email!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Warning!", "All fields are required. Please try again!", "Ok");
            }
        }
    }
}