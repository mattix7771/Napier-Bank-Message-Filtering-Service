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

        [Fact]
        public void SMSTypeTest()
        {
            //Get wanted elements
            var btn_create = mainWindow.FindFirstDescendant(cf.ByAutomationId("btn_create")).AsButton();
            var txt_sender = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_sender")).AsTextBox();

            //Enter text and click button
            txt_sender.Enter("1234");
            btn_create.Click();

            //Check that correct textbox appears
            var txt_messageSMS = mainWindow.FindFirstDescendant(cf.ByAutomationId("txt_messageSMS")).AsTextBox();
            Assert.True(txt_messageSMS.IsAvailable);
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
        public void Test()
        {

        }

        [Fact]
        public void Test()
        {

        }

        [Fact]
        public void Test()
        {

        }
    }
}
