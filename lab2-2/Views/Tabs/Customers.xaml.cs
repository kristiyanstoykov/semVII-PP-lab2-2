using lab2_2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace lab2_2.Views.Tabs
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : UserControl
    {
        private readonly ObservableCollection<Customer> CustomerList;
        private const int TotalCustomers = 10000; // Constant for total customers
        private const int RandomSearchCount = 100; // Constant for random search IDs
        private const int DelayMin = 100; // Constant for random search IDs
        private const int DelayMax = 100; // Constant for random search IDs

        public Customers()
        {
            InitializeComponent();
            DataContext = this;

            CustomerList = GenerateRandomCustomers(TotalCustomers);
        }
        private ObservableCollection<Customer> GenerateRandomCustomers(int count)
        {
            var customers = new ObservableCollection<Customer>();
            var random = new Random();

            string[] firstNames = { "John", "Jane", "Alice", "Bob", "Charlie", "David", "Emma", "Fiona", "George", "Hannah" };
            string[] lastNames = { "Doe", "Smith", "Johnson", "Brown", "Taylor", "Wilson", "Evans", "Thomas", "Roberts", "Walker" };
            string[] streets = { "Main St", "Elm St", "Maple St", "Oak St", "Pine St", "Cedar St", "Birch St", "Spruce St", "Willow St", "Ash St" };

            int id = 1;
            for (int i = 0; i < count; i++)
            {
                string name = $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
                string address = $"{random.Next(1, 1000)} {streets[random.Next(streets.Length)]}";
                int orders = random.Next(1, 20);
                customers.Add(new Customer(id++, name, address, orders));
            }

            return customers;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string[] ids = IdsTextBox.Text.Split(',');
            List<int> idList = ids.Select(int.Parse).ToList();

            List<Customer> results = await FindCustomersByIdsAsync(idList);

            ResultsListBox.ItemsSource = results;
            // Update results count
            ResultsCountTextBlock.Text = $"Found: {results.Count}";
        }

        private void NotAsyncSearchButton_Click(object sender, RoutedEventArgs e)
        {
            string[] ids = IdsTextBox.Text.Split(',');
            List<int> idList = ids.Select(int.Parse).ToList();

            List<Customer> results = FindCustomersByIdsWithDelay(idList);

            ResultsListBox.ItemsSource = results;
            // Update results count
            ResultsCountTextBlock.Text = $"Found: {results.Count}";
        }

        private void RandomSearchButton_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            var ids = Enumerable.Range(1, TotalCustomers)
                                 .OrderBy(x => random.Next())
                                 .Take(RandomSearchCount)
                                 .ToList();

            IdsTextBox.Text = string.Join(",", ids);
        }

        private Task<Customer?> FindCustomerByIdAsync(int id)
        {
            return Task.Run(async () =>
            {
                // Simulate delay
                await Task.Delay(new Random().Next(DelayMin, DelayMax));
                return CustomerList.FirstOrDefault(c => c.Id == id);
            });
        }

        private async Task<List<Customer>> FindCustomersByIdsAsync(List<int> ids)
        {
            var tasks = ids.Select(id => FindCustomerByIdAsync(id)).ToArray();
            Customer?[] results = await Task.WhenAll(tasks);
            return results.Where(r => r != null).Cast<Customer>().ToList();
        }

        private List<Customer> FindCustomersByIdsWithDelay(List<int> ids)
        {
            var results = new List<Customer>();
            var random = new Random();

            foreach (var id in ids)
            {
                Thread.Sleep(random.Next(DelayMin, DelayMax));
                var customer = CustomerList.FirstOrDefault(c => c.Id == id);
                if (customer != null)
                {
                    results.Add(customer);
                }
            }

            return results;
        }
    }
}
