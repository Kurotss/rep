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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Pract_15_16
{
	/// <summary>
	/// Логика взаимодействия для InfoVacancies.xaml
	/// </summary>
	public partial class InfoVacancies : Window
	{
		public InfoVacancies()
		{
			InitializeComponent();
			FillDataGrid();
		}
		private void FillDataGrid()
		{
			try
			{
				using (SqlConnection constring = new SqlConnection(@"Data Source=DESKTOP-62MFGKR; Initial Catalog=StaffCollege;" + "Integrated Security=True;Connect Timeout=15;Encrypt=False;"
			+ "TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
				{
					constring.Open();
					SqlCommand command = new SqlCommand("SELECT * FROM dbo.Vacancies WHERE (Free_vacancies <> 0)", constring);
					SqlDataAdapter sda = new SqlDataAdapter(command);
					DataTable posts = new DataTable("Vacancies");
					sda.Fill(posts);
					info.ItemsSource = posts.DefaultView;
				}
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
