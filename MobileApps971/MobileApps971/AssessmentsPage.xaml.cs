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
	public partial class AssessmentsPage : ContentPage
	{

        private Courses _course;
        private SQLiteAsyncConnection conn;
        private ObservableCollection<Assessments> assessmentsList;

		public AssessmentsPage (Courses courses)
		{
			InitializeComponent ();

            _course = courses;
            assessmentsPageHeader.Text = $"{_course.CourseName}" + "'s Assessments";

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();

            assessmentsListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Assessment_Tapped);
        }

        protected override async void OnAppearing()
        {
            await conn.CreateTableAsync<Assessments>();
            var assessmentsList = await conn.QueryAsync<Assessments>($"SELECT * FROM Assessments WHERE CourseId = '{_course.CourseId}'");
            assessmentsListView.ItemsSource = assessmentsList;

            

            base.OnAppearing();
        }

        private async void AddAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddAssessmentsPage(_course));
        }

        private async void Assessment_Tapped(object sender, ItemTappedEventArgs e)
        {
            Assessments _assessment = (Assessments)e.Item;
            await Navigation.PushModalAsync(new EditAssessmentsPage(_assessment));
        }
    }
}