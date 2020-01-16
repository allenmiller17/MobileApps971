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
        private ObservableCollection<Assessments> _assessmentsList;

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
            _assessmentsList = new ObservableCollection<Assessments>(assessmentsList);
            assessmentsListView.ItemsSource = _assessmentsList;

            

            base.OnAppearing();
        }

        private async void AddAssessmentButton_Clicked(object sender, EventArgs e)
        {

            if (_assessmentsList.Count<Assessments>() >= 2)
            {
                await DisplayAlert("Warning!", "You cannot add more than 2 assessments for a course.", "Ok");
            }
            else
            {
                await Navigation.PushModalAsync(new AddAssessmentsPage(_course));
            }
        }

        private async void Assessment_Tapped(object sender, ItemTappedEventArgs e)
        {
            Assessments _assessment = (Assessments)e.Item;
            await Navigation.PushModalAsync(new EditAssessmentsPage(_assessment));
        }
    }
}