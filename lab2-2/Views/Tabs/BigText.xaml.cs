using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace lab2_2.Views.Tabs
{
    /// <summary>
    /// Interaction logic for BigText.xaml
    /// </summary>
    public partial class BigText : UserControl
    {
        public BigText()
        {
            InitializeComponent();
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "bigText.txt");
            string deploymentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "bigText.txt");

            string filePath = File.Exists(deploymentPath) ? deploymentPath : projectPath;

            // MessageBox.Show($"Checking Paths:\nProject Path: {projectPath}\nDeployment Path: {deploymentPath}\nSelected Path: {filePath}");

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"File not found! File path: {filePath}");
                return;
            }

            try
            {
                // Process file asynchronously
                var wordCounts = await CountWordsAsync(filePath);

                // Populate ListBox
                ResultsListBox.ItemsSource = wordCounts
                                             .OrderByDescending(w => w.Value) // Sort by occurrences
                                             .Select(w => $"{w.Key}: {w.Value}");

                // Show total unique words
                ResultsCountTextBlock.Text = $"Total Unique Words: {wordCounts.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task<Dictionary<string, int>> CountWordsAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                // Read all text from file
                string text = File.ReadAllText(filePath);

                // Match words using Regex
                var matches = Regex.Matches(text, @"\w+");

                // Count word occurrences using LINQ
                return matches
                        .Cast<Match>()
                        .Select(m => m.Value.ToLower())
                        .GroupBy(word => word)
                        .ToDictionary(group => group.Key, group => group.Count());
            });
        }
    }
}
