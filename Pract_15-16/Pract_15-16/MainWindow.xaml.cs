using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;

namespace Pract_15_16
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static string vacancy;
		List <string> numberPosts;
		public MainWindow()
		{
			InitializeComponent();
			try
			{
				using (SqlConnection constring = new SqlConnection(@"Data Source=DESKTOP-62MFGKR; Initial Catalog=StaffCollege;" + "Integrated Security=True;Connect Timeout=15;Encrypt=False;"
			+ "TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
				{
					constring.Open();
					numberPosts = new List<string>();
					SqlCommand command = new SqlCommand("SELECT * FROM dbo.Vacancies WHERE (Free_vacancies <> 0)", constring);
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						numberPosts.Add(reader[0].ToString());
						Vacancies.Items.Add(reader[2]);
					}
				}
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		private void ChoiceVacancy(object sender, SelectionChangedEventArgs e)
		{
			vacancy = numberPosts[Vacancies.SelectedIndex];
			MainFrame.Navigate(new Add_vacancy());
		}
		public static void Clear()
		{
			System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
			Application.Current.Shutdown();
		}

		private void OpenInfo(object sender, RoutedEventArgs e)
		{
			InfoVacancies window = new InfoVacancies();
			window.Show();
		}
	}
}