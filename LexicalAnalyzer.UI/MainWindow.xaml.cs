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
        public MainWindow()
        {
            InitializeComponent();
            var language = Languages.Pascal;
            var filePath = "pascalDefinition.xml";
            language.Save(filePath);

            var file = @"TestData\test_code.pas";
            var fsm = new StateMachine(filePath);

            var result = fsm.Process(file);

            using (var text = File.OpenRead(file))
            {
                using (var reader = new StreamReader(file))
                {
                    testFragment.Text = reader.ReadToEnd();
                }
            }

            parsingResult.ItemsSource = result.CommonSymbolTable;
            keywords.ItemsSource = result.Keywords;
            identifiers.ItemsSource = result.Identifiers;
            delimiters.ItemsSource = result.Delimiters;
            delimiters.ItemsSource = result.Delimiters;
            decimalNumbers.ItemsSource = result.DecimalNumbers;
            stringConstants.ItemsSource = result.Strings;
            languageDelimiters.ItemsSource = fsm.Language.Delimiters;
            languageDoubleDelimiters.ItemsSource = fsm.Language.ComplexDelimiters;
            languageKeywords.ItemsSource = fsm.Language.Keywords;
            languageDelimiters.ItemsSource = fsm.Language.Delimiters;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
