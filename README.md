## Pre-requisites
This application uses .NET 5.0 Framework

## Running
To run the project, simply start the program through Visual Studio or through the comman promt without any parameters.

##How to use
Type the desider sender.
	- phone number without prefix for SMS
	- email address for email
	- twitter id without @ at the start for tweet
Select "Create" to create a message
Type in all the information in the avaliable fields
Click "Send" when done

All outputs can be found in "Napier Bank Message Filtering Service NEW\bin\Debug\net5.0-windows\messages.json"

## Features
Tabs are avaliable to look at statistics
To input messages from a text file, navigate to "Napier Bank Message Filtering Service NEW\bin\Debug\net5.0-windows\input" and enter the desired messages in the following manner:

### For SMS
Sender; text
(eg. +4412345678901; hello, we should meet asap)

### For Emails
Sender; text; subject
(eg. 40442647@live.napier.ac.uk; hello, here's the link you asked for www.gmail.com; link)

### For SIR Emails
Sender; text; subject; date (yyyy-mm-dd); sort code (xxxxxx); Nature of Incident
(eg. 40442647@live.napier.ac.uk; hello, here's the link you asked for www.gmail.com; link; 2022-05-16; 99-24-56; Raid)

### For Tweets
Sender; text
(eg. @therealmark; Hope we can meet up soon @napierbank #brave)

Once done, start the program and click "Read File"

