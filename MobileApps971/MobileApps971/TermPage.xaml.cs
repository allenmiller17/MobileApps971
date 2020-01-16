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
    public partial class TermPage : ContentPage
    {
        private SQLiteAsyncConnection connection;
        private ObservableCollection<Courses> courseList;
        private Terms currentTerm;

        public TermPage(Terms term)
        {
            InitializeComponent();
            
            currentTerm = term;
            connection = DependencyService.Get<IMobileApps971_db>().GetConnection();
            courseListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Course_Tapped);
        }

        protected override async void OnAppearing()
        {
            termHearderLabel.Text = currentTerm.TermName;
            startDateLabel.Text = currentTerm.StartDate.ToShortDateString();
            endDateLabel.Text = currentTerm.EndDate.ToShortDateString();

            await connection.CreateTableAsync<Courses>();
            var _courseList = await connection.QueryAsync<Courses>($"SELECT * FROM Courses WHERE Term = '{currentTerm.Id}'");
            courseList = new ObservableCollection<Courses>(_courseList);

            courseListView.ItemsSource = courseList;

            

            base.OnAppearing();
        }

        private async void EditTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditTermPage(currentTerm));
        }

        private async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            var deleteResponse = await DisplayAlert("WARNING", "You are about to drop the current term! Do you wish to continue?", "Yes", "No");
            if (deleteResponse)
            {
                await connection.DeleteAsync(currentTerm);
                await Navigation.PopToRootAsync();
            }
        }

        private async void AddCourseButton_Clicked(object sender, EventArgs e)
        {
            if (courseList.Count<Courses>() >= 6)
            {
                await DisplayAlert("Warning", "A maximum of 6 courses are allowed per term.", "Ok");
            }
            else
            {
                await Navigation.PushModalAsync(new AddCoursePage(currentTerm)); 
            }
        }

        private async void Course_Tapped(object sender, ItemTappedEventArgs e)
        {
            Courses course = (Courses)e.Item;
            await Navigation.PushAsync(new CoursesPage(course));
        }
    }
}