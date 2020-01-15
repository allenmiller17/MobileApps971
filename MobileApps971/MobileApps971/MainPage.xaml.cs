using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MobileApps971.Model;
using SQLite;
using System.Collections.ObjectModel;
using System.IO;
using Plugin.LocalNotifications;

namespace MobileApps971
{
    public partial class MainPage : ContentPage
    {

        private SQLiteAsyncConnection conn;
        private bool firstTime = true;
        public ObservableCollection<Terms> termList;
        public MainPage()
        {
            InitializeComponent();

            conn = DependencyService.Get<IMobileApps971_db>().GetConnection();
            termsListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Term_Clicked);
            Title = "WGU Home";
            
        }

        protected override async void OnAppearing()
        {
            await conn.CreateTableAsync<Terms>();
            await conn.CreateTableAsync<Courses>();
            await conn.CreateTableAsync<Assessments>();

            var termsList = await conn.Table<Terms>().ToListAsync();

            //Hardcoded Data

            #region Term Data
            if(!termsList.Any())
            {
            //Term Data
            var newTerm = new Terms();
            newTerm.TermName = "Term 1";
            newTerm.StartDate = new DateTime(2020, 01, 01);
            newTerm.EndDate = new DateTime(2020, 06, 30);
                
            await conn.InsertOrReplaceAsync(newTerm);
            termList.Add(newTerm); //Getting a Null Exception and not adding any data to the list
            #endregion

            #region Course Data

            
            
                //Course Data
                var newCourse = new Courses();
                newCourse.CourseId = 1;
                newCourse.CourseName = "C971 Mobile Apps";
                newCourse.Term = newTerm.Id;
                newCourse.StartDate = new DateTime(2020, 01, 01);
                newCourse.EndDate = new DateTime(2020, 01, 30);
                newCourse.CourseInstructorName = "Allen Miller";
                newCourse.CourseInstructorPhone = "8018829123";
                newCourse.CourseInstructorEmail = "amil133@wgu.edu";
                newCourse.CourseNotes = "Make sure to study how to add a new class to a mobile application.";
                newCourse.Notifications = 1;
                newCourse.Status = "In Progress";

                await conn.InsertAsync(newCourse);

                #endregion

                #region Assessment Data

                //Objective Assessment Data
                var newOA = new Assessments();
                newOA.AssessmentId = 1;
                newOA.AssessmentName = "Mobile Apps Objective Assessment";
                newOA.AssessmentType = "Objective Assessment";
                newOA.AssessmentStart = new DateTime(2020, 01, 06);
                newOA.AssessmentEnd = new DateTime(2020, 01, 12);
                newOA.CourseId = newCourse.CourseId;
                newOA.AssessmentNotifications = 1;
                await conn.InsertAsync(newOA);

                //Performance Assessment Data
                var newPA = new Assessments();
                newPA.AssessmentId = 1;
                newPA.AssessmentName = "Mobile Apps Performance Assessment";
                newPA.AssessmentType = "Performance Assessment";
                newPA.AssessmentStart = new DateTime(2020, 01, 13);
                newPA.AssessmentEnd = new DateTime(2020, 01, 19);
                newPA.CourseId = newCourse.CourseId;
                newPA.AssessmentNotifications = 1;

                await conn.InsertAsync(newPA);

                #endregion

            }
            var courseList = await conn.Table<Courses>().ToListAsync();
            var assessList = await conn.Table<Assessments>().ToListAsync();
            termsListView.ItemsSource = termsList;

            if (firstTime)
            {
                #region Courses Notifications
                firstTime = false;
                int courseId = 0;
                foreach (Courses courses in courseList)
                {
                    courseId++;
                    if (courses.Notifications == 1)
                    {
                        if (courses.StartDate == DateTime.Today)
                        {
                            CrossLocalNotifications.Current.Show("Reminder", $"{courses.CourseName} is starting today.", courseId);
                        }

                        if (courses.EndDate == DateTime.Today)
                        {
                            CrossLocalNotifications.Current.Show("Reminder", $"{courses.CourseName} is ending today.", courseId);
                        }
                    }
                }

                #endregion

                #region Assessments Notifications

                //Assessment Notifications
                int assessId = courseId;
                foreach (Assessments assessments in assessList)
                {
                    if (assessments.AssessmentStart == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Reminder", $"{assessments.AssessmentName} is ending today.", courseId);
                    }

                    if (assessments.AssessmentEnd == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Reminder", $"{assessments.AssessmentName} is ending today.", courseId);
                    }
                }

                #endregion
            }
            termList = new ObservableCollection<Terms>(termsList);
            termsListView.ItemsSource = termsList;
            base.OnAppearing();
        }

        private void AddTerm_Clicked(object sender, EventArgs e)
        {

        }

        async private void Term_Clicked(object sender, ItemTappedEventArgs e)
        {
            Terms term = (Terms)e.Item;
            await Navigation.PushAsync(new TermPage(term));
        }
    }
}
