using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.Json;
using System;
using System.Linq;
using Napier_Bank_Message_Filtering_Service_NEW.Properties;
using Napier_Bank_Message_Filtering_Service_NEW.Resources;

namespace Napier_Bank_Message_Filtering_Service_NEW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SMS SMS = new SMS();
        EMAIL EMAIL = new EMAIL();
        TWEET TWEET = new TWEET();
        SIR SIRReport = new SIR();
        Random rand = new Random();
        Dictionary<String, String> textwords = new Dictionary<string, string>();
        Dictionary<String, String> country_codes = new Dictionary<string, string>();
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
            //General string checks
            if (CheckBoxErrorMessage(txt_sender) == false)
                return;
            
            //Check what type of message it is and open elements accordingly
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
                txt_twitterID.Text = "@" + txt_sender.Text;
            }
        }

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

            //C
            string[] data = Data.textwords.Split("\n");

            foreach (var line in data)
            {
                if (string.IsNullOrEmpty(line)) continue;
                //string currentSplit = line);
                string abbreviation = line.Substring(0, line.IndexOf(","));
                string phrase = line.Substring(line.IndexOf(",") + 1);
                Console.WriteLine("" +  abbreviation + " " +  phrase);

                textwords.Add(abbreviation, phrase);
            }
        }

        public void ComponentsInit()
        {
            //Phone prefix combobox initialisation
            string[] country_codes_array = Data.country_codes.Split("\n");
            foreach (string code in country_codes_array)
            {
                cb_prefix.Items.Add(code.ToString());
                country_codes.Add(code.Substring(0, code.IndexOf("   ")).Trim(), code.Substring(code.IndexOf("   ")).Trim());
            }
            cb_prefix.SelectedItem = cb_prefix.Items.GetItemAt(0);

            //Nature of accident combobox initialisation
            String[] NOI = { "Theft", "Staff Attack", "ATM Theft", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Intelligence", "Cash Loss" };
            for (int i = 0; i < NOI.Length; i++)
            {
                cb_SIR_NOI.Items.Add(NOI[i]);
            }

            //Check if input file exists and create it
            if (!File.Exists("input.txt"))
                File.Create("input.txt");
        }

        public void Filter(String type, bool send = false)
        {
            if(!send)
                txt_output.Text = "";

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
                        if (send)
                        {
                            quarantined_URLS.Add(x);
                            //if (check_SIR.IsChecked == true)
                            //    SIRReport = (new SIR('R', "R" + Create_ID(), txt_email.Text, txt_output.Text, txt_subject.Text, SIR_date.DisplayDate, txt_SIRSortCode.Text, cb_SIR_NOI.SelectedItem.ToString()));
                        }
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
                    if (send)
                    {
                        if (Regex.IsMatch(x, "^#", RegexOptions.IgnoreCase))
                            if (!hashtags.ContainsKey(x))
                                hashtags.Add(x, 1);
                            else
                                hashtags[x]++;

                        //Check for mentions and add to list
                        if (Regex.IsMatch(x, "^@", RegexOptions.IgnoreCase))
                            mentions.Add(x);
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
            check_SIR.Visibility = Visibility.Visible;

            txt_subject.MaxLength = 20;
            txt_messageEmail.MaxLength = 1028;
        }

        //Layout for Tweet message
        public void Tweet_layout()
        {
            //Make appropriate elements visible
            Reset_layout();
            lbl_twitterID.Visibility = txt_twitterID.Visibility = lbl_messageTwitter.Visibility = txt_messageTwitter.Visibility = Visibility.Visible;

            //Set max txt lenghts
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
            //Populate statistics lists
            Filter("E", true); Filter("T", true);

            //Clear lists
            lb_mentionList.Items.Clear();
            lb_QuarantinedList.Items.Clear();
            lb_TrendList.Items.Clear();

            foreach (String x in quarantined_URLS)
                lb_QuarantinedList.Items.Add(x);

            foreach (String x in mentions)
                lb_mentionList.Items.Add(x);

            foreach (KeyValuePair<string, int> x in hashtags)
                lb_TrendList.Items.Add(x);

            if(SIRReport != null)
                lb_SIRList.Items.Add("SIR ID: " + SIRReport.Id + "\n\t" + "Sender: " + SIRReport.Sender
                                                                + "\n\t" + "Date: " + SIRReport.Sir_date
                                                                + "\n\t" + "Sort code: " + SIRReport.Sort_code
                                                                + "\n\t" + "Nature: " + SIRReport.NOI
                                                                + "\n\t" + "Message: " + SIRReport.Text);



            //Message object setup for JSON serialization
            if (txt_messageSMS.Visibility == Visibility.Visible)
            {
                SMS = new SMS('S', "S" + Create_ID(), cb_prefix.Text.Substring(cb_prefix.Text.IndexOf("+")) + txt_phonenumber.Text, txt_output.Text);
                txt_output.Text = "";
                JsonOutput(SMS.Type);
            }
            else if (txt_SIRSortCode.Visibility == Visibility.Visible)
            {
                SIRReport = new SIR('R', "R" + Create_ID(), txt_email.Text, txt_output.Text, txt_subject.Text, SIR_date.DisplayDate, txt_SIRSortCode.Text, cb_SIR_NOI.SelectedItem.ToString());
                JsonOutput(SIRReport.Type);
            }
            else if (txt_messageEmail.Visibility == Visibility.Visible)
            {
                EMAIL = new EMAIL('E', "E" + Create_ID(), txt_email.Text, txt_output.Text, txt_subject.Text);
                txt_output.Text = "";
                JsonOutput(EMAIL.Type);
            }
            else if (txt_messageTwitter.Visibility == Visibility.Visible)
            {
                TWEET = new TWEET('T', "T" + Create_ID(), txt_twitterID.Text, txt_output.Text);
                txt_output.Text = "";
                JsonOutput(TWEET.Type);
            }
        }

        public String Create_ID()
        {
            //Create message id
            string id = "";
            for (int i = 0; i < 9; i++)
            {
                id += rand.Next(10);
            }
            return id;
        }

        private void txt_sender_TextChanged(object sender, TextChangedEventArgs e)
        {
            check_SIR.IsChecked = false;
        }

        public void JsonOutput(char type)
        {
            //Create JSON file
            if (!File.Exists("messages.json"))
                File.Create("messages.json");

            //Encoder options
            var encoder_options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            //Add message object to JSON file
            if(type == 'S')
                File.AppendAllText("messages.json", JsonSerializer.Serialize(SMS, encoder_options));
            else if (type == 'E')
                File.AppendAllText("messages.json", JsonSerializer.Serialize(EMAIL, encoder_options));
            else if (type == 'R')
                File.AppendAllText("messages.json", JsonSerializer.Serialize(SIRReport, encoder_options));
            else if (type == 'T')
                File.AppendAllText("messages.json", JsonSerializer.Serialize(TWEET, encoder_options));



        }

        private void btn_ReadFile_Click(object sender, RoutedEventArgs e)
        {
            //Check if there is data in file
            if (File.ReadAllText("input.txt") == "")
            {
                MessageBox.Show("File is empty");
                return;
            }

            //If there are messages in the file check correct format and load
            String[] text = File.ReadAllLines("input.txt");

            foreach (String entry in text)
            {
                String[] line = entry.Split(';');

                if (entry == "")
                    return;

                if (line[0].Contains('@') && line[0].Contains('.'))
                {
                    txt_messageEmail.Text = line[1];
                    txt_subject.Text = line[2];
                    Filter("E", true);
                    EMAIL = new EMAIL('E', "E" + Create_ID(), line[0], txt_output.Text, line[2]);
                    txt_output.Text = "";
                    JsonOutput(EMAIL.Type);
                }
                else if (Int64.TryParse(line[0].Substring(1), out long value))
                {
                    SMS = new SMS('S', "S" + Create_ID(), line[0], line[1]);
                    JsonOutput(SMS.Type);
                }
                else
                {
                    TWEET = new TWEET('T', "T" + Create_ID(), line[0], line[1]);
                    JsonOutput(TWEET.Type);
                }
            }
        }

        //General string checks abd custom error message
        public bool CheckBoxErrorMessage(TextBox textbox, string text = "")
        {
            //Custom message
            if (text != "")
            {
                MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //General string checks
            if (textbox.Text == "")
            {
                MessageBox.Show(textbox.Name + " cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (Regex.Match(textbox.Text, @"\s").Success)
            {
                MessageBox.Show(textbox.Name + " cannot contain spaces", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
            
        }

        private void txt_messageSMS_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter("S");
        }

        private void txt_messageEmail_TextChanged(object sender, TextChangedEventArgs e) 
        {
            Filter("E");
        }

        private void txt_messageTwitter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter("T");
        }
    }
}
