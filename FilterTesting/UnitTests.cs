using System;
using Xunit;
using Napier_Bank_Message_Filtering_Service_NEW;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.Factory;
using System.Threading;
using TestStack.White.UIItems.Finders;

namespace FilterTesting
{
    public class UnitTests
    {
        [Fact]
        public void Test1()
        {
            var app = Application.Launch("Napier Bank Message Filtering Service NEW.exe");
            
            //Finds the main window (this and above line should be in [TestInitialize])
            var window = app.GetWindow("MainWindow", InitializeOption.NoCache);


            //Thread.Sleep(5000);
            //Finds the button (see other Get...() methods for options)
            var btnMyButton = (Button) window.Get<Button>(SearchCriteria.ByAutomationId("btn_create"));
            var sendertextbox = (TextBox) window.Get(SearchCriteria.ByAutomationId("txt_sender"));

            sendertextbox.Text = "34";
            /*
            var messagetextbox = window.Get(SearchCriteria.ByAutomationId("txt_messageSMS"));
            
            sendertextbox.Enter("12345678");

            //Simulate clicking
            btnMyButton.Click();

            //Gets the result text box 
            //Note: TextBox/Button is in TestStack.White.UIItems namespace
            //var txtMyTextBox = window.Get<TextBox>("txtMyTextBox");

            //Check for the result
            Assert.True(messagetextbox.Visible);*/

            //Close the main window and the app (preferably in [TestCleanup])
            app.Close();

        }
    }
}
