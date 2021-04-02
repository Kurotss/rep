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
using System.Data.SqlClient;

namespace Pract_15_16
{
	/// <summary>
	/// Логика взаимодействия для Add_vacancy.xaml
	/// </summary>
	public partial class Add_vacancy : Page
	{
		public string paulR;
		public Add_vacancy()
		{
			InitializeComponent();
		}
		private void addImage(int row, int column)
		{
			Image image = new Image();
			image.Source = BitmapFrame.Create(new Uri("Resources/point.png", UriKind.Relative));
			image.SetValue(Grid.RowProperty, row);
			image.SetValue(Grid.ColumnProperty, column + 1);
			grid.Children.Add(image);
		}
		private void InputForm(object sender, RoutedEventArgs e)
		{
			var children = grid.Children.OfType<UIElement>().ToList();
			foreach (FrameworkElement img in children)
			{
				if (img is Image) grid.Children.Remove(img);
			}
			Head.Text = "Заполните форму";
			bool AllFill = true;
			int notSelectedRadioButton = 0;
			children = grid.Children.OfType<UIElement>().ToList();
			foreach (FrameworkElement content in children)
			{
				if (content is TextBox)
				{
					TextBox txt = (TextBox)content;
					if (String.IsNullOrWhiteSpace(txt.Text))
					{
						AllFill = false;
						txt.Style = (Style)FindResource("IsNull");
					}
					else txt.Style = (Style)FindResource("Textbox_style");
				}
				else if (content is ComboBox)
				{
					ComboBox txt = (ComboBox)content;
					if (txt.SelectedIndex == -1)
					{
						AllFill = false;
						addImage(Grid.GetRow(txt), Grid.GetColumn(txt));
					}
				}
				else if (content is RadioButton)
				{
					RadioButton txt = (RadioButton)content;
					if (txt.IsChecked == false) notSelectedRadioButton++;
				}
				else if (content is DatePicker)
				{
					DatePicker txt = (DatePicker)content;
					if (txt.SelectedDate == null)
					{
						AllFill = false;
						addImage(Grid.GetRow(txt), Grid.GetColumn(txt));
					}
				}
			}
			if (notSelectedRadioButton == 2)
			{
				AllFill = false;
				addImage(7, 4);
			}
			if (AllFill)
			{
				try
				{
					using (SqlConnection constring = new SqlConnection(@"Data Source=DESKTOP-62MFGKR; Initial Catalog=StaffCollege;" + "Integrated Security=True;Connect Timeout=15;Encrypt=False;"
				+ "TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
					{
						constring.Open();
						SqlCommand command = new SqlCommand("INSERT INTO dbo.Staffers (Surname, First_name, Middlename, Date_of_birth, Telephon, Education, Paul) " +
							"VALUES (@newSurname, @newFirst_name, @newMiddlename, @newDate_of_birth, @newTelephon, @newEducation, @newPaul)", constring);
						command.Parameters.AddWithValue("@newSurname", Surname.Text);
						command.Parameters.AddWithValue("@newFirst_name", Name.Text);
						command.Parameters.AddWithValue("@newMiddlename", Middlename.Text);
						command.Parameters.AddWithValue("@newDate_of_birth", Date_of_birth.SelectedDate);
						command.Parameters.AddWithValue("@newTelephon", Telephon.Text);
						command.Parameters.AddWithValue("@newEducation", Education.Text);
						command.Parameters.AddWithValue("@newPaul", paulR);
						int number = command.ExecuteNonQuery();
						command = new SqlCommand("SELECT * FROM dbo.LastStaffer", constring);
						SqlDataReader reader = command.ExecuteReader();
						reader.Read();
						int numberNewStaffer = int.Parse(reader[0].ToString());
						reader.Close();
						command = new SqlCommand("INSERT INTO dbo.History (Number_staffer, Number_post, Date_of_dismissal, Date_of_admission) " +
							"VALUES (@newStaffer, @numberPost, NULL, @today)", constring);
						command.Parameters.AddWithValue("@newStaffer", numberNewStaffer);
						command.Parameters.AddWithValue("@numberPost", MainWindow.vacancy);
						command.Parameters.AddWithValue("@today", DateTime.Now);
						number = command.ExecuteNonQuery();
					}
				}
				catch (SqlException ex)
				{
					MessageBox.Show(ex.ToString());
				}
				MainWindow.Clear();
			}
			else Head.Text = "Заполните пустые поля";
		}
		private void Radiobutton_checked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton item)
			{
				paulR = item.Content.ToString();
			}
		}

		private void OnlyLetters(object sender, TextCompositionEventArgs e)
		{
			if (e.Text[0] > '0' && e.Text[0] < '9')
			{
				e.Handled = true;
			}
		}
		private void OnlyNumbers(object sender, TextCompositionEventArgs e)
		{
			if (e.Text[0] < '0' || e.Text[0] > '9')
			{
				e.Handled = true;
			}
		}
	}
}