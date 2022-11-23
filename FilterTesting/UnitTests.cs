using System;
using Xunit;
using Napier_Bank_Message_Filtering_Service_NEW;
using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.Conditions;
using FlaUI.Core.AutomationElements;

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
        public void BadInputTest()
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
    }
}
