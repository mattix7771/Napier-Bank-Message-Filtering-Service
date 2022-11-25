using System;
using Xunit;
using Napier_Bank_Message_Filtering_Service_NEW;
using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.Conditions;
using FlaUI.Core.AutomationElements;
using System.Linq.Expressions;

namespace FilterTesting
{
    public class UnitTests
    {
        //Test initiation
        static Application application = Application.Launch("Napier Bank Message Filtering Service NEW.exe");
        static UIA3Automation auto = new UIA3Automation();
        Window mainWindow = application.GetMainWindow(auto);
        ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());



        [Fact]
        public void ASMSTypeTest()
        {
            //Get wanted elements
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();

            //Enter text and click button
            txt_sender.Enter("123456789");
            btn_create.Click();

            //Check that correct textbox appears
            var txt_messageSMS = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageSMS")).AsTextBox();
            Assert.True(txt_messageSMS.IsAvailable);
        }

        [Fact]
        public void BSMSBodyLenghtTest()
        {
            //Check that user may only write 140 characters
            var txt_messageSMS = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageSMS")).AsTextBox();
            //Assert.Throws<System.InvalidOperationException>(() => 
            txt_messageSMS.Enter("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
        }

        [Fact]
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
            Assert.True(txt_messageEmail.IsAvailable);
        }

        [Fact]
        public void EmailSubjectLenghtTest()
        {
            //Check that user may only write 20 characters
            var txt_subject = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_subject")).AsTextBox();
            Assert.Throws<System.InvalidOperationException>(() => txt_subject.Enter("Lorem ipsum dolor sit amet, consectetur"));
        }

        [Fact]
        public void EmailBodyLenghtTest()
        {
            //Check that user may only write 1048 characters
            var txt_messageEmail = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageEmail")).AsTextBox();
            Assert.Throws<System.InvalidOperationException>(() => txt_messageEmail.Enter("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
        }

        [Fact]
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
            Assert.True(txt_messageTwitter.IsAvailable);
        }

        [Fact]
        public void TwitterIDLenghtTest()
        {
            //Check that user may only write 20 characters
            var txt_twitterID = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_twitterID")).AsTextBox();
            Assert.Throws<System.InvalidOperationException>(() => txt_twitterID.Enter("Lorem ipsum dolor sit amet"));
        }

        [Fact]
        public void TweetBodyLenghtTest()
        {
            //Check that user may only write 140 characters
            var txt_messageTwitter = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageTwitter")).AsTextBox();
            Assert.Throws<System.InvalidOperationException>(() => txt_messageTwitter.Enter("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
        }

        [Fact]
        public void PreviewReadOnlyTest()
        {
            //Check that the preview box is readonly
            var txt_output = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_output")).AsTextBox();
            Assert.Throws<System.InvalidOperationException>(() => txt_output.Enter("hello"));
        }
        
        [Fact]
        public void TabSelectionTest()
        {
            //Check that tabs work properly
            var tab_statistics = mainWindow.FindFirstDescendant(cf.ByName("tab_statistics")).AsTabItem();
            var lb_TrendList = mainWindow.FindFirstDescendant(cf.ByName("lb_TrendList")).AsListBox();
            tab_statistics.Click();
            Assert.True(lb_TrendList.IsAvailable);
        }

        [Fact]
        public void SIRCheckBoxTest()
        {
            //Check that SIR checkbox works properly
            var check_SIR = mainWindow.FindFirstDescendant(cf.ByName("check_SIR")).AsCheckBox();
            var txt_SIRSortCode = mainWindow.FindFirstDescendant(cf.ByName("txt_SIRSortCode")).AsTextBox();
            check_SIR.Click();
            Assert.True(txt_SIRSortCode.IsAvailable);
        }
    }
}
