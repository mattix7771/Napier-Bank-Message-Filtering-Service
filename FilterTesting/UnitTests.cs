using System;
using Xunit;
using Napier_Bank_Message_Filtering_Service_NEW;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.Factory;
using System.Threading;

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
            var btnMyButton = window.Get<Button>("btn_create");
            var sendertextbox = window.Get<TextBox>("txt_sender");
            var messagetextbox = window.Get<TextBox>("txt_messageSMS");

            sendertextbox.Enter("12345678");

            //Simulate clicking
            btnMyButton.Click();

            //Gets the result text box 
            //Note: TextBox/Button is in TestStack.White.UIItems namespace
            //var txtMyTextBox = window.Get<TextBox>("txtMyTextBox");

            //Check for the result
            Assert.True(messagetextbox.Visible);

            //Close the main window and the app (preferably in [TestCleanup])
            app.Close();

        }
    }
}
