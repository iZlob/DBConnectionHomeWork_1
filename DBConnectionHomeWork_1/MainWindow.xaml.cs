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
using MySqlBuilder;
using MySqlConnector;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace DBConnectionHomeWork_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "";
        public MainWindow()
        {
            InitializeComponent();

            var ConnectionStringBuilder = new MySqlConnectionStringBuilder();
            ConnectionStringBuilder.Database = "zlobindatabase";
            ConnectionStringBuilder.Server = "db4free.net";
            ConnectionStringBuilder.Port = 3306;
            ConnectionStringBuilder.UserID = "i_zlobin";
            ConnectionStringBuilder.Password = "Myrtlebeach2007";
            //ConnectionStringBuilder.SslMode = MySqlSslMode.Required;

            connectionString = ConnectionStringBuilder.ConnectionString;

            tableName.Items.Add("Workers");
            tableName.Items.Add("Cars");
            tableName.Items.Add("Orders");
        }

        private void tableName_GotFocus(object sender, RoutedEventArgs e)
        {
            tableName.IsDropDownOpen = true;
        }

        private void tableName_LostFocus(object sender, RoutedEventArgs e)
        {
            tableName.IsDropDownOpen = false;
        }

        private void btn_show_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.GetColumn(grid.Children[grid.Children.Count - 1]) == 0 && Grid.GetRow(grid.Children[grid.Children.Count - 1]) == 2)
                grid.Children.RemoveAt(grid.Children.Count - 1);

            DataGrid table = new DataGrid();
            table.AlternationCount = 2;
            table.AlternatingRowBackground = new SolidColorBrush(Colors.LightBlue);
            table.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
            table.IsReadOnly = true;
            table.VerticalAlignment = VerticalAlignment.Top;
            table.HorizontalAlignment = HorizontalAlignment.Left;
            table.VerticalContentAlignment = VerticalAlignment.Center;
            table.HorizontalContentAlignment = HorizontalAlignment.Center;

            Grid.SetColumn(table, 0);
            Grid.SetRow(table, 2);
            grid.Children.Add(table);

            using (var MySqlConnection = new MySqlConnection(connectionString))
            {               
                while (MySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    MySqlConnection.Open();
                }
                if (tableName.SelectedItem != null)
                {
                    switch (tableName.SelectedItem.ToString())
                    {
                        case "Workers":
                            {
                                using (var command = MySqlConnection.CreateCommand())//создает объект DbCommand
                                {
                                    command.CommandType = System.Data.CommandType.Text;
                                    command.CommandText = @"SELECT * FROM `workers`";

                                    using (var rez = command.ExecuteReader())// метод возвращает DbDataReader для получения результата запроса DB
                                    {
                                        var Workers_ = new List<Workers>();
                                        var isNextDataAvalible = rez.HasRows;
                                        rez.Read();
                                        while (isNextDataAvalible)
                                        {
                                            Workers w = new Workers();
                                            for (int i = 0; i < rez.FieldCount; i += 2)
                                            {
                                                w.Id = Convert.ToInt32(rez.GetValue(i));
                                                w.Name = rez.GetValue(i + 1).ToString();
                                            }
                                            Workers_.Add(w);
                                            isNextDataAvalible = rez.Read();
                                        }
                                        table.ItemsSource = Workers_;
                                    }
                                }
                                break;
                            }
                        case "Cars":
                            {
                                using (var command = MySqlConnection.CreateCommand())//создает объект DbCommand
                                {
                                    command.CommandType = System.Data.CommandType.Text;
                                    command.CommandText = @"SELECT * FROM `cars`";

                                    using (var rez = command.ExecuteReader())// метод возвращает DbDataReader для получения результата запроса DB
                                    {
                                        var Cars_ = new List<Cars>();
                                        var isNextDataAvalible = rez.HasRows;
                                        rez.Read();
                                        while (isNextDataAvalible)
                                        {
                                            Cars c = new Cars();
                                            for (int i = 0; i < rez.FieldCount; i += 3)
                                            {
                                                c.Id = Convert.ToInt32(rez.GetValue(i));
                                                c.Manufacturer = rez.GetValue(i + 1).ToString();
                                                c.Model = rez.GetValue(i + 2).ToString();
                                            }
                                            Cars_.Add(c);
                                            isNextDataAvalible = rez.Read();
                                        }
                                        table.ItemsSource = Cars_;
                                    }
                                }
                                break;
                            }
                        case "Orders":
                            {
                                using (var command = MySqlConnection.CreateCommand())//создает объект DbCommand
                                {
                                    command.CommandType = System.Data.CommandType.Text;
                                    command.CommandText = @"SELECT orders.id,ACEPT.Name, REP.Name, cars.manufacturer, cars.model, orders.defect, orders.fixed 
                                                            FROM `cars`, `workers` AS ACEPT, `workers` AS REP, `orders`
                                                            WHERE ACEPT.id=orders.accepcerId AND REP.id=orders.repairerId AND cars.id=orders.carId";
                                    
                                    using (var rez = command.ExecuteReader())// метод возвращает DbDataReader для получения результата запроса DB
                                    {
                                        var Orders_ = new List<Orders>();
                                        var isNextDataAvalible = rez.HasRows;
                                        rez.Read();
                                        while (isNextDataAvalible)
                                        {
                                            Orders o = new Orders();
                                            for (int i = 0; i < rez.FieldCount; i += 7)
                                            {
                                                o.Id = Convert.ToInt32(rez.GetValue(i));
                                                o.Acceptor = rez.GetValue(i + 1).ToString();
                                                o.Repairer = rez.GetValue(i + 2).ToString();
                                                o.Manufacturer = rez.GetValue(i + 3).ToString();
                                                o.Model = rez.GetValue(i + 4).ToString();
                                                o.Defect = rez.GetValue(i + 5).ToString();
                                                o.Fixed = rez.GetValue(i + 6).ToString();
                                            }
                                            Orders_.Add(o);
                                            isNextDataAvalible = rez.Read();
                                        }
                                        table.ItemsSource = Orders_;
                                    }
                                }
                                break;
                            }
                        default: break;
                    }
                }
            }
        }
    }
}
