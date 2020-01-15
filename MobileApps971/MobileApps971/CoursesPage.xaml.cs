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


	public partial class CoursesPage : ContentPage
	{

        private SQLiteAsyncConnection conn;
        private ObservableCollection<Assessments> assessmentsList;
        private Courses currentCourse;

        public CoursesPage (Courses course)
		{
			InitializeComponent ();

            currentCourse = course;
            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            courseHearderLabel.Text = $"{currentCourse.CourseName}";
            courseStatusLabel.Text = "Status: " + $"{currentCourse.Status}";
            startDateLabel.Text = "Start Date: " + $"{currentCourse.CourseStartDate.ToShortDateString()}";
            endDateLabel.Text = "End Date: " + $"{currentCourse.CourseEndDate.ToShortDateString()}";
            instructorNameLabel.Text = "Course Instructor Name: " + $"{currentCourse.CourseInstructorName}";
            instructorPhoneLabel.Text = "Course Instructor Phone: " + $"{currentCourse.CourseInstructorPhone}";
            instructorEmailLabel.Text = "CourseInstructor Email: " + $"{currentCourse.CourseInstructorEmail}";


            await conn.CreateTableAsync<Assessments>();
            var _assessmentsList = await conn.QueryAsync<Assessments>($"SELECT * FROM Assessments WHERE CourseId = '{currentCourse.CourseId}'");
            assessmentsList = new ObservableCollection<Assessments>(_assessmentsList);



            base.OnAppearing();
        }

        private async void AddAssessmentsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentsPage(currentCourse));
        }

        private async void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            var deleteResponse = await DisplayAlert("WARNING", "You are about to drop this course! Do you wish to continue?", "Yes", "No");
            if (deleteResponse)
            {
                await conn.DeleteAsync(currentCourse);
                await Navigation.PopAsync();
            }
        }

        private async void EditCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditCoursePage(currentCourse));
        }

        private async void AddCourseNotesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CourseNotesPage(currentCourse));
        }
    }
}