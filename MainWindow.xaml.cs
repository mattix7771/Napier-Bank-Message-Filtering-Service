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
using System.Text.Json;

namespace Napier_Bank_Message_Filtering_Service_NEW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Message message;
        Random rand = new Random();
        Dictionary<String, String> textwords = new Dictionary<string, string>();
        List<String> quarantined_URLS = new List<string>();
        Dictionary<string, int> hashtags = new Dictionary<string, int>();
        List<string> mentions = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Color.FromRgb(171, 171, 171));
            InitializeDictionary();
            txt_output.IsReadOnly = true;
            ComponentsInit();
            Reset_layout();
        }

        private void btn_create_Click(object sender, RoutedEventArgs e)
        {
            if (txt_sender.Text.Contains("@"))
            {
                Email_layout();
                txt_email.Text = txt_sender.Text;
            }
            else if (int.TryParse(txt_sender.Text, out int value))
            {
                Sms_layout();
                txt_phonenumber.Text = txt_sender.Text;
            }
            else
            {
                Tweet_layout();
                txt_twitterID.Text = txt_sender.Text;
            }
                
        }


        private void txt_message_TextChanged(object sender, TextChangedEventArgs e){}

        private void btn_preview_Click(object sender, RoutedEventArgs e)
        {
            txt_output.Text = "";

            //Text preview
            if (txt_messageSMS.Visibility == Visibility.Visible)
            {
                txt_output.Text = "Sender: " + cb_prefix.Text.Substring(cb_prefix.Text.IndexOf('+') - 1) + " " + txt_phonenumber.Text + "\n";
                Filter("S");
            }
            else if (txt_messageEmail.Visibility == Visibility.Visible)
            {
                //Check that email contains "@" to assure it's an email address
                if (!txt_email.Text.Contains("@"))
                {
                    MessageBox.Show("Invalid email, try again");
                    return;
                }

                Filter("E");
            }
            else if (txt_messageTwitter.Visibility == Visibility.Visible)
            {
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

        public void ComponentsInit()
        {
            //Phone prefix combobox initialisation
            String[] country_codes = File.ReadAllLines("country codes.txt");
            foreach (string code in country_codes)
            {
                cb_prefix.Items.Add(code);
            }
            cb_prefix.SelectedItem = cb_prefix.Items.GetItemAt(0);

            //Nature of accident combobox initialisation
            String[] NOI = { "Theft", "Staff Attack", "ATM Theft", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Intelligence", "Cash Loss" };
            for (int i = 0; i < NOI.Length; i++)
            {
                cb_SIR_NOI.Items.Add(NOI[i]);
            }
        }

        public void Filter(String type)
        {
            if (type == "S")
            {
                String body = "";

                body = txt_messageSMS.Text;

                foreach (KeyValuePair<string, string> entry in textwords)
                {
                    if (Regex.Match(body, @"\b" + entry.Key + @"\b", RegexOptions.IgnoreCase).Success)
                    {
                        body = Regex.Replace(body, @"\b" + entry.Key + @"\b", entry.Key + "<" + entry.Value + ">", RegexOptions.IgnoreCase);
                    }
                }
                txt_output.Text += body;
            }
            else if(type == "E")
            {
                String body = txt_messageEmail.Text;

                foreach (String x in body.Split(' '))
                    if (Regex.IsMatch(x, @"\b\w*www|http|https\w*\b", RegexOptions.IgnoreCase))
                    {
                        body = Regex.Replace(body, x, "<URL Quarantined>");
                        quarantined_URLS.Add(x);
                    }
                txt_output.Text += body;
            }
            else if(type == "T")
            {
                //Get text from textbox
                String body = "";
                body = txt_messageTwitter.Text;

                //Check for abbreviations
                foreach (KeyValuePair<string, string> entry in textwords)
                {
                    if (Regex.Match(body, @"\b" + entry.Key + @"\b", RegexOptions.IgnoreCase).Success)
                    {
                        body = Regex.Replace(body, @"\b" + entry.Key + @"\b", entry.Key + "<" + entry.Value + ">", RegexOptions.IgnoreCase);
                    }
                }

                //Check for hashtags and mentions
                foreach (String x in body.Split(' '))
                {
                    //Check for hashtags and add to dictionary
                    if (Regex.IsMatch(x, "^#", RegexOptions.IgnoreCase))
                        if (!hashtags.ContainsKey(x))
                            hashtags.Add(x, 1);
                        else
                            hashtags[x]++;
                    //Check for mentions and add to list
                    if (Regex.IsMatch(x, "^@", RegexOptions.IgnoreCase))
                        mentions.Add(x);
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
            check_SIR.Visibility = Visibility.Visible;

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
            check_SIR.Visibility = Visibility.Hidden;
        }

        private void txt_sender_KeyDown(object sender, KeyEventArgs e)
        {
            if (Regex.IsMatch(e.Key.ToString(), "[0-9]"))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txt_subject_KeyDown(object sender, KeyEventArgs e)
        {
            if (check_SIR.IsChecked == true)
            {
                if (Regex.IsMatch(e.Key.ToString(), "[0-9]"))
                    e.Handled = false;
                else
                    e.Handled = true;
            }
        }

        private void check_SIR_Checked(object sender, RoutedEventArgs e)
        {
            SIR_date.Visibility = Visibility.Visible;
            lbl_SIRSortCode.Visibility = Visibility.Visible;
            txt_SIRSortCode.Visibility = Visibility.Visible;
            lbl_SIR_NOI.Visibility = Visibility.Visible;
            cb_SIR_NOI.Visibility = Visibility.Visible;
            txt_messageEmail.Margin = new Thickness(txt_messageEmail.Margin.Left, txt_messageEmail.Margin.Top + 80, txt_messageEmail.Margin.Right, txt_messageEmail.Margin.Bottom);
            lbl_messageEmail.Margin = new Thickness(lbl_messageEmail.Margin.Left, lbl_messageEmail.Margin.Top + 80, lbl_messageEmail.Margin.Right, lbl_messageEmail.Margin.Bottom);
            txt_subject.Width = 40;
            txt_subject.IsReadOnly = true;
            txt_subject.Text = "SIR ";
        }

        private void check_SIR_Unchecked(object sender, RoutedEventArgs e)
        {
            SIR_date.Visibility = Visibility.Hidden;
            lbl_SIRSortCode.Visibility = Visibility.Hidden;
            txt_SIRSortCode.Visibility = Visibility.Hidden;
            lbl_SIR_NOI.Visibility = Visibility.Hidden;
            cb_SIR_NOI.Visibility = Visibility.Hidden;
            txt_messageEmail.Margin = new Thickness(txt_messageEmail.Margin.Left, txt_messageEmail.Margin.Top - 80, txt_messageEmail.Margin.Right, txt_messageEmail.Margin.Bottom);
            lbl_messageEmail.Margin = new Thickness(lbl_messageEmail.Margin.Left, lbl_messageEmail.Margin.Top - 80, lbl_messageEmail.Margin.Right, lbl_messageEmail.Margin.Bottom);
            txt_subject.Width = 240;
            txt_subject.IsReadOnly = false;
            txt_subject.Text = "";
        }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            //Create JSON file
            if (!File.Exists("messages.json"))
                File.Create("messages.json");

            //Create message id
            string id = "";
            for (int i = 0; i < 9; i++)
            {
                id += rand.Next(10);
            }

            //Message object setup for JSON serialization
            if (txt_messageSMS.Visibility == Visibility.Visible)
            {
                message = new Message('S', "S" + id, txt_phonenumber.Text, txt_output.Text);
            }
            else if (txt_messageEmail.Visibility == Visibility.Visible)
            {
                message = new Message('E', "E" + id, txt_email.Text, txt_output.Text);
                message.Subject = txt_subject.Text;
                message.Sir_date = SIR_date.DisplayDate;
                message.Sort_code = txt_SIRSortCode.Text;
                message.NOI = cb_SIR_NOI.Text;
            }
            else if (txt_messageTwitter.Visibility == Visibility.Visible)
            {
                message = new Message('S', "S" + id, txt_twitterID.Text, txt_output.Text);
            }

            //Add message object to JSON file
            File.AppendAllText("messages.json", JsonSerializer.Serialize(message).ToString());
        }
    }
}
