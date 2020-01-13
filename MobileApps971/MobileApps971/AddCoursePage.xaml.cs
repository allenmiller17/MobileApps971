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

        //protected override async void OnApearing()
        //{
        //    await conn.CreateTableAsync<Courses>();

        //    base.OnAppearing();
        //}

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var newCourse = new Courses();
            newCourse.CourseName = courseName.Text;
            newCourse.StartDate = startDatePicker.Date;
            newCourse.EndDate = endDatePicker.Date;
            newCourse.Status = (string)courseStatusPicker.SelectedItem;
            newCourse.CourseInstructorName = instructorNameEntry.Text;
            newCourse.CourseInstructorPhone = instructorPhoneEntry.Text;
            newCourse.CourseInstructorEmail = instructorEmailEntry.Text;
            newCourse.Term = _currentTerm.Id;
            //TODO add notifications

            await conn.InsertAsync(newCourse);
            await DisplayAlert("Notice", "New Course Created", "Ok");
            await Navigation.PopModalAsync();
        }
    }
}