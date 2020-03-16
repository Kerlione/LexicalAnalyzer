using LexicalAnalyzer.BL.FSM;
using LexicalAnalyzer.Tests.TestData;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LexicalAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string _languageDefinitionPath = "pascalDefinition.xml";
        private static readonly string _testCodeFragment = @"TestData\test_code.pas";
        private static StateMachine fsm;
        public MainWindow()
        {
            InitializeComponent();
            InitStateMachine();

            languageDelimiters.ItemsSource = fsm.Language.Delimiters;
            languageDoubleDelimiters.ItemsSource = fsm.Language.ComplexDelimiters;
            languageKeywords.ItemsSource = fsm.Language.Keywords;
            languageDelimiters.ItemsSource = fsm.Language.Delimiters;
            languageSymbols.ItemsSource = fsm.Language.AllowedSymbols;

            LoadTestFragment();
            Process();
        }

        private void InitStateMachine()
        {
            fsm = new StateMachine(_languageDefinitionPath);
        }

        private void LoadTestFragment()
        {
            using (var text = File.OpenRead(_testCodeFragment))
            {
                using (var reader = new StreamReader(_testCodeFragment))
                {
                    testFragment.Text = reader.ReadToEnd();
                    reader.Close();
                }
                text.Close();
            }
        }

        private void Process()
        {
            try
            {
                var result = fsm.Process(_testCodeFragment);
                parsingResult.ItemsSource = result.CommonSymbolTable;
                keywords.ItemsSource = result.Keywords;
                identifiers.ItemsSource = result.Identifiers;
                delimiters.ItemsSource = result.Delimiters;
                delimiters.ItemsSource = result.Delimiters;
                decimalNumbers.ItemsSource = result.DecimalNumbers;
                stringConstants.ItemsSource = result.Strings;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(_testCodeFragment, testFragment.Text);
            LoadTestFragment();
            Process();
        }
    }
}
