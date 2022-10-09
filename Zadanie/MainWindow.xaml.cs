using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Zadanie
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int a = 0;
        int b = 1;
        int result = 0;
        int sum;
        public MainWindow()
        {
            InitializeComponent();
            inputNum.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteCommand));
        }

        private void PasteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            //Запрет вставки текста
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (inputNum.Text != string.Empty)
            {
                int i = Int32.Parse(inputNum.Text);
                if (e.Key == Key.Up)
                {
                    i++;
                    inputNum.Text = $"{i}";
                }
                if (e.Key == Key.Down)
                {
                    i--;
                    inputNum.Text = $"{i}";
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
            else
            {
                if(inputNum.Text != string.Empty)
                {
                    if (Int32.Parse(inputNum.Text.First().ToString()) == 0) inputNum.Text = "1";
                    if (Int32.Parse(inputNum.Text.First().ToString()) == 1 && e.Text == "0")
                    {
                        inputNum.Text = "10";
                    }
                    else
                        inputNum.Text = e.Text;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (inputNum.Text == "Введите любое число от 1 до 10") inputNum.Text = "1";
        }

        private void inputNum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(inputNum.Text)) inputNum.Text = "Введите любое число от 1 до 10";
        }

        private void inputNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Int32.TryParse(inputNum.Text,out int val))
            {
                if (Convert.ToInt32(inputNum.Text) >= 10)
                {
                    inputNum.Text = "10";
                }

                if (inputNum.Text != string.Empty)
                {
                    if (Int32.Parse(inputNum.Text.First().ToString()) == 0) inputNum.Text = "1";
                }
            }
        }

        private void inputNum_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int i = Int32.Parse(inputNum.Text);
            if (e.Delta == 120)
            {
                i++;
                inputNum.Text = $"{i}";
            }
            if (e.Delta == -120)
            {
                i--;
                inputNum.Text = $"{i}";
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int val;
            a = 0;
            b = 1;
            Int32.TryParse(inputNum.Text, out val);
            if (val == 1)
            {
                fibo.Text = $"{a}, {b}";
                sum = a + b;
                fiboSum.Text = $"{sum}";
            }
            if (val > 1)
            {
                fibo.Text = $"{a}, {b}";
                for (int i = 2; i <= val; i++)
                {
                    sum = a + b;
                    fibo.Text += $", {sum}";
                    a = b;
                    b = sum;
                }
            }
            result = 0;
            foreach(var s in fibo.Text.Split(','))
            {
                result += Int32.Parse(s);
            }
            fiboSum.Text = result.ToString();

        }

        private void inputMaxWordLength_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
            else
            {
                if (inputMaxWordLength.Text != string.Empty)
                {
                    if (Int32.Parse(inputMaxWordLength.Text.First().ToString()) == 0) inputMaxWordLength.Text = "1";
                }
            }
        }

        private void inputMaxWordLength_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(inputMaxWordLength.Text)) inputMaxWordLength.Text = "Введите максимальную длину слова";
        }

        private void inputMaxWordLength_GotFocus(object sender, RoutedEventArgs e)
        {
            if (inputMaxWordLength.Text == "Введите максимальную длину слова") inputMaxWordLength.Text = "5";
        }

        private void inputMaxWordLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(inputMaxWordLength.Text != "Введите максимальную длину слова")
            {
                foundWords.Items.Clear();
                if (Int32.TryParse(inputMaxWordLength.Text, out int val))
                {
                    string[] separators = new string[] { "\r\n", " " };
                    string allText = new TextRange(richText.Document.ContentStart, richText.Document.ContentEnd).Text;
                    string[] Words = allText.Split(separators,StringSplitOptions.None);
                    foreach (var s in Words)
                    {
                        var temp = s.Trim().Replace("?", "").Replace("!", "").Replace(".", "").Replace(",", "").Replace(":","")
                            .Replace("(","").Replace(")","").Replace(";","");
                        
                        if (temp.Length == val)
                        {
                            foundWords.Items.Add(temp);
                        }
                    }
                }
            }
            
        }

        private void searchEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchEmail.Text != "Поиск E-mail адресов" && searchEmail.Text != string.Empty)
            {
                string allText = new TextRange(richText.Document.ContentStart, richText.Document.ContentEnd).Text;
                Regex emails = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",RegexOptions.IgnoreCase);
                MatchCollection emailsMatches = emails.Matches(allText);

                foreach (Match email in emailsMatches)
                {
                    if(email.Value.Contains(searchEmail.Text))
                    {
                        foundWords.Items.Clear();
                        foundWords.Items.Add(email.Value);
                    }
                }
            }
        }

        private void searchEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchEmail.Text == "Поиск E-mail адресов") searchEmail.Text = "";
        }

        private void searchEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(searchEmail.Text)) searchEmail.Text = "Поиск E-mail адресов";
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if(foundWords.Items.Count >= 1)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "TXT | *.txt";
                saveFile.DefaultExt = "TXT | *.txt";
                saveFile.FileName = "result";
                if(saveFile.ShowDialog() == true)
                {
                    StreamWriter writer = new StreamWriter(saveFile.FileName);
                    foreach (string line in foundWords.Items)
                    {
                        writer.WriteLine(line);
                    }
                    writer.Close();
                    MessageBox.Show("Файл сохранён.","Сохранение",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Отсутсвуют данные!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Text File | *.txt";
            openFile.FileName = "";
            if(openFile.ShowDialog() == true)
            {
                StreamReader reader = new StreamReader(openFile.FileName);
                richText.Document.Blocks.Clear();
                richText.Document.Blocks.Add(new Paragraph(new Run(reader.ReadToEnd())));
                reader.Close();
            }
        }

        private void inputMaxWordLength_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int i = Int32.Parse(inputMaxWordLength.Text);
            if (e.Delta == 120)
            {
                i++;
                inputMaxWordLength.Text = $"{i}";
            }
            if (e.Delta == -120)
            {
                i--;
                inputMaxWordLength.Text = $"{i}";
            }
        }
    }
}
