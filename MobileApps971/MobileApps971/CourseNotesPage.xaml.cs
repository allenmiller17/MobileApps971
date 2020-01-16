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
using Xamarin.Essentials;

namespace MobileApps971
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CourseNotesPage : ContentPage
	{

        private SQLiteAsyncConnection conn;
        private Courses _currentCourse;

		public CourseNotesPage (Courses currentCourse)
		{
			InitializeComponent ();

            _currentCourse = currentCourse;

            courseNotesHeader.Text = $"{_currentCourse.CourseName}" + "'s Notes";

            courseNotesEntry.Text = $"{_currentCourse.CourseNotes}";

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();

        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            _currentCourse.CourseNotes = courseNotesEntry.Text;
            await conn.UpdateAsync(_currentCourse);
            await DisplayAlert("Success", "Course Notes have been updated", "ok");
            await Navigation.PopModalAsync();
        }

        private void ShareButton_Clicked(object sender, EventArgs e)
        {
            Share.RequestAsync(new ShareTextRequest { Text = courseNotesEntry.Text, Title = "Share Course Notes" });
        }
    }
}