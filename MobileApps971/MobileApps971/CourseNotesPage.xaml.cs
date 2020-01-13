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
	public partial class CourseNotesPage : ContentPage
	{
		public CourseNotesPage (Courses currentCourse)
		{
			InitializeComponent ();
		}
	}
}