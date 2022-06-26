[![Build Status](https://dev.azure.com/bhathiyamadusanka/1337/_apis/build/status/1337-ASP.NET%20Core-CI?branchName=dev)](https://dev.azure.com/bhathiyamadusanka/1337/_build/latest?definitionId=11&branchName=dev)

# What is WebAppDownloader?
WebAppDownloader is a simple console application where you can download the content of a web application asynchronously and save it to the disk while keeping the online file structure. You will be able to change the download path and web app url using the settings define in the appsettings.json file. 

# Application Design
Application consist of a startup project and two other projects ApplicationServices and Shared. Startup project is the entry point to the project and where DI configured in. ApplicationServices project consists of service interfaces and the implementations. Mainly this layer contains all application logic. Shared project has helpers and that can be referenced to any other layers.

This solution has two main branches main and dev. Also two feature branches origin from the dev. Dev branch has branch policies implemented. It enforces to raise PR when feature branches are merge to the dev brach. Every changes made to the dev branch will triggered the continuous integration pipeline. It will check the build satus to be  success for merge branches and display the build status in the GitHub.

# How to run the Application?
Application has built using .NetCore 3.1. Download or fork the repository. You will be able to run the application using visual studio or visual studio code. Set Startup project to be Startup. If you need executable file please publish the application. Change the file download path and web app url accordingly using appsettings.json.

Note : Download performance improved solution added under the branch feature/improvement_download_website.  

