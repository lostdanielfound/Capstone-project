# BoxIt Proposal

Daniel Guzman, CSCI 490 Capstone project

## Project introduction
Backups of an organization or to the simple house user is a key part in disaster recovery in rebuilding data after a disaster has happen. Backups systems should allow customization that will fit the user's need. 

The current backup system on windows 10 isn't that great. User experience isn't great, there isn't any customization that the user can do besides creating an image of the OS and doing full backups of default directory. (Similar to Time machine on mac but worse). 

## Existing work 
Currtently there are quite a few of backup managers available for the Windows OS, they offer a lot of custimizable features with how you want to have your backups be stored i.e: encrypted or stored as parts of an image. Some backup managers support online backups where you upload the backups to a remote destination, while others can serve as an entire partition manager. 

R-Drive Image is an example of a reliable image backup software that also serves as a File and folder backup manager. 

Retrospect Solo is another backup manager that said to be good towards added ransomware protection that would prevent ransomware from overwriting backup images. 

While these are great and rebust backup managers, they do come with a cost that most users would opt-out on. 

## My approach on the problem

I want to create a Desktop Backup manager, ideally, this manager will allow the user to create and manage their backups of any type of file system, this system should be robust but also allow customization of the backup process, i.e: scheduling, incremental / decremental types, backup destination, and backup encryption. 

## Algoithms and resources