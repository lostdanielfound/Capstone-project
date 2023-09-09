# BoxIt Proposal

Daniel Guzman, CSCI 490 Capstone project

## Project introduction
Backups of an organization or to the simple house user is a key part in disaster recovery in rebuilding data after a disaster has happen. Backups systems should allow customization that will fit the user's need. 

The current backup system on windows 10 isn't that great. User experience isn't great, there isn't any customization that the user can do besides creating an image of the OS and doing full backups of default directory. (Similar to Time machine on mac but worse). 

## My approach on the problem

I want to create a Desktop Backup manager, ideally, this manager will allow the user to create and manage their backups of any type of file system, this system should be robust but also allow customization of the backup process, i.e: scheduling, incremental / decremental types, backup destination, and backup encryption. 

## Existing work 
Currtently there are quite a few of backup managers available for the Windows OS, they offer a lot of custimizable features with how you want to have your backups be stored i.e: encrypted or stored as parts of an image. Some backup managers support online backups where you upload the backups to a remote destination, while others can serve as an entire partition manager. 

R-Drive Image is an example of a reliable image backup software that also serves as a File and folder backup manager. 

Retrospect Solo is another backup manager that said to be good towards added ransomware protection that would prevent ransomware from overwriting backup images. 

While these are great and rebust backup managers, they do come with a cost that most users would opt-out on. 

## Algoithms and resources
A Compression algorithm that will not only compress a file within a directory but also the directory itself, and most importantly a decompression algorithm to unpack the image. 

A searching algorithm would be needed to implement File and Folder backup feature to search for which files have been changed either added into the directory or removed. 

I will be using Visual Basic to develop and design the GUI since it allows me to work very closly with the .NET framework and will make it easy for Directory and Drive selection. 

Another thing to think about is Unit testing, testing will be and should be frequent ensure that data content is being properly compressed and decompressed during the backup process. 

## Project Timeline
The timeline of this project is high-leveled at the moment but I will put in finer details later on.

### September - Learning phase
 - [ ] Review how to use Visual Basic and the .NET Framework. 
 - [ ] Create basic searching algorithm that will record and find which files have changed within a span of time. (Incremental backup) 
 - [ ] Get in touch with Parallel Professor to talk about compression algorithm would be best for backing up files. (Data Compresion)

### October - Working phase
 - [ ] Create Minimal basic GUI to implement a File and folder backup system. This system should simply perform a Full Backup of a file or directory to it's destination and allow the user to schedule this task.
 - [ ] Create a Job status menu that will keep track of the currently scheduled Full Backups. (Logging will be required)
 - [ ] Implement Incremental backup feature that will backup all files that have only been changed since the last backup of a directory. 

### November - Redesign and Testing 

- [ ] Redesign GUI layout and start Unit testing and coverages.
- [ ] Implement Differential backup featue that will backup all the files that only have changed since the last Full backup.
- [ ] Expand on Backup scheduling; When a task / process starts, a backup will occur. 
- [ ] Look into C.I through Github to possibly improve Testing.
- [ ] If there is more time, implement an Encryption feature 

### December - Production
- [ ] Build Application on Github and "Easy setup" process for the user.

## Deliverables
I hope to being into production a Backup manager that  will do the following:  
 - Basic Directory and file backups 
 - Perform Full, Increment / Decremental, Differetial backups 
 - Compression backups and Decompress them. 
 - Schedule backups according to Time or process signal.
 - Display currently scheduled backups jobs.