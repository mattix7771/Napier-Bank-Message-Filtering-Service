using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.Conditions;
using FlaUI.Core.AutomationElements;
using System;
using System.Threading;
using Napier_Bank_Message_Filtering_Service_NEW;
using System.Runtime.Intrinsics.X86;
using System.Windows.Forms;
using Application = FlaUI.Core.Application;
using System.Windows.Interop;

namespace NBM_Testing
{

    /*
lbl_sender
txt_sender
btn_create
lbl_preview
txt_output
btn_preview
btn_send
lbl_phonenumber
cb_prefix
txt_phonenumber
lbl_messageSMS
txt_messageSMS
lbl_email
txt_email
lbl_subject
txt_subject
lbl_messageEmail
txt_messageEmail
lbl_twitterID
txt_twitterID
lbl_messageTwitter
txt_messageTwitter
check_SIR
SIR_date
txt_SIRSortCode
lbl_SIRSortCode
lbl_SIR_NOI
cb_SIR_NOI
btn_ReadFile
lb_TrendList
lb_mentionList
lb_SIRList
lb_QuarantinedList
lbl_TrendingList
lbl_QuarantinedURLs
lbl_SIRList
lbl_MentionsList
*/
    //UI TESTING
    [TestClass]
    public class UI
    {
        //Test initiation
        static Application application = Application.Launch("Napier Bank Message Filtering Service NEW.exe");
        static UIA3Automation auto = new UIA3Automation();
        Window mainWindow = application.GetMainWindow(auto);
        ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

        [TestMethod]
        public void SMSTypeTest()
        {
            //Get wanted elements
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();

            //Enter text and click button
            txt_sender.Enter("123456789");
            btn_create.Click();

            //Check that correct textbox appears
            var txt_messageSMS = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageSMS")).AsTextBox();
            Assert.IsTrue(txt_messageSMS.IsAvailable);
        }

        [TestMethod]
        public void SMSBodyLenghtTest()
        {
            //Get wanted elements
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();

            //Enter text and click button
            txt_sender.Enter("123456789");
            btn_create.Click();

            //Check that user may only write 140 characters
            var txt_messageSMS = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageSMS")).AsTextBox();
            txt_messageSMS.Enter("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            Thread.Sleep(3000);
            Assert.AreEqual(txt_messageSMS.Text.Length, 140);
        }

        [TestMethod]
        public void EmailTypeTest()
        {
            //Get wanted elements
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();

            //Enter text and click button
            txt_sender.Enter("40442647@live.napier.co.uk");
            btn_create.Click();

            //Check that correct textbox appears
            var txt_messageEmail = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageEmail")).AsTextBox();
            Assert.IsTrue(txt_messageEmail.IsAvailable);
        }

        [TestMethod]
        public void EmailSubjectLenghtTest()
        {
            //Check that user may only write 20 characters
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            txt_sender.Enter("40442647@live.napier.co.uk");
            btn_create.Click();
            var txt_subject = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_subject")).AsTextBox();
            txt_subject.Enter("Lorem ipsum dolor sit amet, consectetur");
            Thread.Sleep(3000);
            Assert.AreEqual(txt_subject.Text.Length, 20);
        }

        [TestMethod]
        public void EmailBodyLenghtTest()
        {
            //Check that user may only write 1028 characters
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            txt_sender.Enter("40442647@live.napier.co.uk");
            btn_create.Click();
            var txt_messageEmail = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageEmail")).AsTextBox();
            txt_messageEmail.Enter("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            Thread.Sleep(10000);
            Assert.AreEqual(txt_messageEmail.Text.Length, 1028);
        }

        [TestMethod]
        public void SIRCheckBoxTest()
        {
            //Check that SIR checkbox works properly
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            txt_sender.Enter("40442647@live.napier.co.uk");
            btn_create.Click();
            var check_SIR = mainWindow.FindFirstDescendant(cf.ByAutomationId("check_SIR")).AsCheckBox();
            check_SIR.Toggle();
            var txt_SIRSortCode = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_SIRSortCode")).AsTextBox();
            Assert.IsTrue(txt_SIRSortCode.IsAvailable);
        }

        [TestMethod]
        public void TweetTypeTest()
        {
            //Get wanted elements
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();

            //Enter text and click button
            txt_sender.Enter("JohnSmith");
            btn_create.Click();

            //Check that correct textbox appears
            var txt_messageTwitter = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageTwitter")).AsTextBox();
            Assert.IsTrue(txt_messageTwitter.IsAvailable);
        }

        [TestMethod]
        public void TwitterIDLenghtTest()
        {
            //Check that user may only write 16 characters
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            txt_sender.Enter("JohnSmith");
            btn_create.Click();
            var txt_twitterID = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_twitterID")).AsTextBox();
            txt_twitterID.Enter("Lorem ipsum dolor sit amet");
            Thread.Sleep(3000);
            Assert.AreEqual(txt_twitterID.Text.Length, 16);
        }

        [TestMethod]
        public void TweetBodyLenghtTest()
        {
            //Check that user may only write 140 characters
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            txt_sender.Enter("JohnSmith");
            btn_create.Click();
            var txt_messageTwitter = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageTwitter")).AsTextBox();
            txt_messageTwitter.Enter("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            Thread.Sleep(3000);
            Assert.AreEqual(txt_messageTwitter.Text.Length, 140);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PreviewReadOnlyTest()
        {
            //Check that the preview box is readonly
            var txt_output = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_output")).AsTextBox();
            txt_output.Enter("hello");
        }

        [TestMethod]
        public void TabSelectionTest()
        {
            //Check that tabs work properly
            var tab_statistics = mainWindow.FindFirstDescendant(cf.ByAutomationId("tab_statistics")).AsTabItem();
            tab_statistics.Click();
            var lb_TrendList = mainWindow.FindFirstDescendant(cf.ByAutomationId("lb_TrendList")).AsListBox();
            Assert.IsTrue(lb_TrendList.IsAvailable);
        }

    }

    [TestClass]
    public class Messages
    {
        //Build messages
        SMS msg = new SMS('S', "123456789", "+44123456789", "Hello, I hope to see you asap");
        EMAIL email = new EMAIL('E', "123445", "40442647@live.napier.ac.uk", "Hello, here's the link you asked for www.google.com", "Link");
        TWEET tweet = new TWEET('T', "123445", "@Johnsmith", "Hello, I hope to see you asap");
        SIR sir = new SIR('R', "4567890", "SIR", "Hello, here's the link you asked for www.google.com", "Link", DateTime.Now, "44-44-44", "Incident");

        //Check for correct values
        [TestMethod]
        public void SMSTest()
        {
            Assert.AreEqual(msg.Type, 'S');
            Assert.AreEqual(msg.Id, "123456789");
            Assert.AreEqual(msg.Sender, "+44123456789");
            Assert.AreEqual(msg.Text, "Hello, I hope to see you asap");
        }

        [TestMethod]
        public void EMAILTest()
        {
            Assert.AreEqual(email.Type, 'E');
            Assert.AreEqual(email.Id, "123445");
            Assert.AreEqual(email.Sender, "40442647@live.napier.ac.uk");
            Assert.AreEqual(email.Text, "Hello, here's the link you asked for www.google.com");
            Assert.AreEqual(email.Subject, "Link");
        }

        [TestMethod]
        public void TWEETTest()
        {
            Assert.AreEqual(tweet.Type, 'T');
            Assert.AreEqual(tweet.Id, "123445");
            Assert.AreEqual(tweet.Sender, "@Johnsmith");
            Assert.AreEqual(tweet.Text, "Hello, I hope to see you asap");
        }

        [TestMethod]
        public void SIRTest()
        {
            Assert.AreEqual(sir.Type, 'R');
            Assert.AreEqual(sir.Id, "4567890");
            Assert.AreEqual(sir.Sender, "SIR");
            Assert.AreEqual(sir.Text, "Hello, here's the link you asked for www.google.com");
            Assert.AreEqual(sir.Subject, "Link");
            Assert.AreEqual(sir.Sort_code, "44-44-44");
            Assert.AreEqual(sir.NOI, "Incident");

        }
    }

    [TestClass]
    public class Filtering
    {
        //Initialise UI
        static Application application = Application.Launch("Napier Bank Message Filtering Service NEW.exe");
        static UIA3Automation auto = new UIA3Automation();
        Window mainWindow = application.GetMainWindow(auto);
        ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

        [TestMethod]
        public void EMAILFilter()
        {
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            var txt_output = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_output")).AsTextBox();
            txt_sender.Enter("JohnSmith@gmail.com");
            btn_create.Click();
            var txt_messageEmail = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageEmail")).AsTextBox();
            txt_messageEmail.Text = "hello, here's the link you wanted www.google.com";
            Assert.AreEqual(txt_output.Text, "hello, here's the link you wanted <URL Quarantined>");
        }

        [TestMethod]
        public void SMSFilter()
        {
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            var txt_output = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_output")).AsTextBox();
            txt_sender.Enter("67890");
            btn_create.Click();
            var txt_messageSMS = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageSMS")).AsTextBox();
            txt_messageSMS.Text = "hello, pls come AAP";
            Assert.AreEqual(txt_output.Text, "hello, pls come AAP<Always a pleasure>");
        }

        [TestMethod]
        public void TWEETFilter()
        {
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();
            var txt_output = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_output")).AsTextBox();
            txt_sender.Enter("JohnSmith");
            btn_create.Click();
            var txt_messageTwitter = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageTwitter")).AsTextBox();
            txt_messageTwitter.Text = "hello, pls come AAP #me #you @where";
            Assert.AreEqual(txt_output.Text, "hello, pls come AAP<Always a pleasure\r\n> #me #you @where");
        }
    }
}
