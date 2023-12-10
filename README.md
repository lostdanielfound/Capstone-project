# BoxIT 
Capstone Project

### Overview
BoxIT is a backup management software that serves as a utility in monitoring scheduled tasks in backing up files, folders and hard drives. Aside from backing up data, the software also allows the user to restore the data in the case of a Data loss incident. The key feature of the design is allowing the user to customize when scheduled backup tasks should occur to however the user prefers. 

This type of application is meant to target enterprises that need software to serve as a Schedule Management utility in backing up bulks of data. I approach making this application by using the Visual Studio IDE for GUI design along with the programming language Visual Basic. In terms of archiving backups, I used a .NET framework called DotNetZip which allows me to create / update zip archives. Most importantly, DotNetZip supports Zip64 which means I can interact with .zip that are greater than 4gb which is very important server wise. I also used another .NET framework called NewtonsoftJSON which is used to create / update backup logs and keep records of current backup plans. For Schedule Management, assuming this application will be deployed on a server running Windows 10 / 11, I used the built-in Task Scheduler application to schedule the backup tasks. This makes it easy to schedule backup tasks since it allows me to set Triggers which are conditions(s) that can be defined to trigger the task. 

### Features
 - Full-Backup
 - Incremental-backup
 - Customizable backup

### Showcase
  (To be added soon) 
