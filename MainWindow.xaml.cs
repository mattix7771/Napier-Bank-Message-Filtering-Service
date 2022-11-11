using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Napier_Bank_Message_Filtering_Service_NEW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        Dictionary<String, String> textwords = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeDictionary();
            txt_output.IsReadOnly = true;
            ComboboxInit();
            Reset_layout();
        }


        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            if (txt_sender.Text.Contains("@"))
                Email_layout();
            else if (int.TryParse(txt_sender.Text, out int value))
                Sms_layout();
            else
                Tweet_layout();
        }


        private void txt_message_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            //Create message id
            string id = "";
            for (int i = 0; i < 9; i++)
            {
                id += rand.Next(10);
            }

            //Output text
            if (txt_messageSMS.Visibility == Visibility.Visible)
            {
                txt_output.Text = "S" + id + "\n" + "Sender: " + cb_prefix.Text.Substring(cb_prefix.Text.IndexOf('+') - 1) + " " + txt_phonenumber.Text + "\n";
                Filter("S");
            }
            else if (txt_messageEmail.Visibility == Visibility.Visible)
            {
                txt_output.Text = "E" + id + "\n";
                Filter("E");
            }
            else if (txt_messageTwitter.Visibility == Visibility.Visible)
            {
                txt_output.Text = "T" + id + "\n";
                Filter("T");
            }
        }

        public void InitializeDictionary()
        {
            //Extract abbreviations from file
            String[] temp_text = File.ReadAllLines("textwords.csv");
            List<String> text = new List<string>();

            //Devide text into individual items
            for (int i = 0; i < temp_text.Length; i++)
            {
                String line = temp_text[i];
                if (line.Contains("\""))
                {
                    String[] temp = line.Split('"');
                    text.Add(temp[0]);
                    text.Add(temp[1]);
                }
                else
                {
                    String[] temp = line.Split(',');
                    text.Add(temp[0]);
                    text.Add(temp[1]);
                }
            }

            //Add items to dictionary
            for (int i = 0; i < text.Count; i += 2)
            {
                textwords.Add(text[i], text[i + 1]);
            }
        }

        public void ComboboxInit()
        {
            String[] country_codes = File.ReadAllLines("country codes.txt");
            foreach (string code in country_codes)
            {
                cb_prefix.Items.Add(code);
            }
            cb_prefix.SelectedItem = cb_prefix.Items.GetItemAt(0);
        }

        public void Filter(String type)
        {
            if (type == "S" || type == "T")
            {
                String body = "";

                if (type == "S")
                    body = txt_messageSMS.Text;
                else if (type == "T")
                    body = txt_messageTwitter.Text;

                foreach (KeyValuePair<string, string> entry in textwords)
                {
                    if (Regex.Match(body, @"\b" + entry.Key + @"\b", RegexOptions.IgnoreCase).Success)
                    {
                        body = Regex.Replace(body, @"\b" + entry.Key + @"\b", entry.Key + "<" + entry.Value + ">", RegexOptions.IgnoreCase);
                    }
                }
                txt_output.Text += body;
            }
        }

        public void Sms_layout()
        {
            Reset_layout();
            lbl_phonenumber.Visibility = Visibility.Visible;
            txt_phonenumber.Visibility = Visibility.Visible;
            lbl_messageSMS.Visibility = Visibility.Visible;
            txt_messageSMS.Visibility = Visibility.Visible;
            cb_prefix.Visibility = Visibility.Visible;

            txt_phonenumber.MaxLength = 20;
            txt_messageSMS.MaxLength = 140;
        }

        public void Email_layout()
        {
            Reset_layout();
            lbl_email.Visibility = Visibility.Visible;
            txt_email.Visibility = Visibility.Visible;
            lbl_subject.Visibility = Visibility.Visible;
            txt_subject.Visibility = Visibility.Visible;
            lbl_messageEmail.Visibility = Visibility.Visible;
            txt_messageEmail.Visibility = Visibility.Visible;

            txt_subject.MaxLength = 20;
            txt_messageEmail.MaxLength = 1028;
        }

        public void Tweet_layout()
        {
            Reset_layout();
            lbl_twitterID.Visibility = Visibility.Visible;
            txt_twitterID.Visibility = Visibility.Visible;
            lbl_messageTwitter.Visibility = Visibility.Visible;
            txt_messageTwitter.Visibility = Visibility.Visible;


            txt_twitterID.Text = "@";
            txt_twitterID.MaxLength = 16;
            txt_messageTwitter.MaxLength = 140;
        }

        public void Reset_layout()
        {
            lbl_phonenumber.Visibility = Visibility.Hidden;
            txt_phonenumber.Visibility = Visibility.Hidden;
            lbl_messageSMS.Visibility = Visibility.Hidden;
            txt_messageSMS.Visibility = Visibility.Hidden;
            lbl_email.Visibility = Visibility.Hidden;
            txt_email.Visibility = Visibility.Hidden;
            lbl_subject.Visibility = Visibility.Hidden;
            txt_subject.Visibility = Visibility.Hidden;
            lbl_messageEmail.Visibility = Visibility.Hidden;
            txt_messageEmail.Visibility = Visibility.Hidden;
            lbl_twitterID.Visibility = Visibility.Hidden;
            txt_twitterID.Visibility = Visibility.Hidden;
            lbl_messageTwitter.Visibility = Visibility.Hidden;
            txt_messageTwitter.Visibility = Visibility.Hidden;
            cb_prefix.Visibility = Visibility.Hidden;
        }

        private void txt_sender_KeyDown(object sender, KeyEventArgs e)
        {
            if (Regex.IsMatch(e.Key.ToString(), "[0-9]"))
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
