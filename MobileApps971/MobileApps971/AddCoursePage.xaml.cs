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
	public partial class AddCoursePage : ContentPage
	{

        private SQLiteAsyncConnection conn;
        private Terms _currentTerm;

		public AddCoursePage (Terms currentTerm)
		{
			InitializeComponent ();

            addCourseHearderLabel.Text = "Add New Course";

            _currentTerm = currentTerm;
            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var newCourse = new Courses();
            newCourse.CourseName = courseName.Text;
            newCourse.CourseStartDate = startDatePicker.Date;
            newCourse.CourseEndDate = endDatePicker.Date;
            newCourse.Status = (string)courseStatusPicker.SelectedItem;
            newCourse.CourseInstructorName = instructorNameEntry.Text;
            newCourse.CourseInstructorPhone = instructorPhoneEntry.Text;
            newCourse.CourseInstructorEmail = instructorEmailEntry.Text;
            newCourse.Term = _currentTerm.Id;

            //Makes sure no Null Fields Exist
            if (!HelperClass.IsNull(courseName.Text) && !HelperClass.IsNull(startDatePicker.Date.ToShortDateString()) &&
                            !HelperClass.IsNull(endDatePicker.Date.ToShortDateString()) &&
                            !HelperClass.IsNull((string)courseStatusPicker.SelectedItem) && !HelperClass.IsNull(instructorNameEntry.Text) &&
                            !HelperClass.IsNull(instructorPhoneEntry.Text) && !HelperClass.IsNull(instructorEmailEntry.Text))
            {
                //Checks for valid @wgu.edu email
                if (HelperClass.EmailIsValid(instructorEmailEntry.Text))
                {
                    //Checks for 10 digits and no other characters in phone field
                    if (HelperClass.PhoneIsValid(instructorPhoneEntry.Text))
                    {
                        //Date Validation
                        if (newCourse.CourseStartDate <= newCourse.CourseEndDate)
                        {
                            await conn.InsertAsync(newCourse);
                            await DisplayAlert("Notice", "New Course Created", "Ok");
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