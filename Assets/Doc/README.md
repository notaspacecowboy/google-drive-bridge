# sfml-tower-defense
## Table of Contents
  * [Overview](#Overview)
  * [How to Use it](#How-to-Use-it)
  * [How to Extend it](#How-to-Extend-it)
 
## Overview
Google Sheet Bridge allows Unity Developers to integrate their game projects with Google 
APIs, upload their game data to Google SpreadSheet for data analytic purposes.
***

## How to Use it
The package contains two parts - a front-end Unity package that can be directly import into the game project, and a backend Google App Script that needs to be deployed to game developer's Google Drive.

### I. Import the Unity Package to your Game Project
This step is straight-forward, simply copy&paste everything in the Github trunk into a folder inside your Unity Asset folder.

<br />

### II. Create the configuration file
Inside Unity, right click on Create->Google Drive Bridge->Drive Configuration Data, this will create a Unity ScriptableObject asset for you, leave it only for now, we will come back to it later.

<br />

### III. Setup the web service
go to https://script.google.com/home/projects/1JqwCmCkDHocSssyNcPCfcoNoiMpWQau1PLOUBIVp-PQmvhuOC6D9_gYa/edit, create a copy of this Google App Script and put the new script in your Google Drive. Then, inside the script editor, navigate to Project settings->Script Properties and add a key-value pair. The key name must be *PASSWORD*, and you can fill its value on your own.  

<br />

### IV. Deploy the web service
Now you are ready to deploy the web service, make sure to set *who has access* to *anyone*, click *Deploy* and copy the URL.

<br />

### V. Complete the front-end configuration file
Go back to unity, fill in the asset you just created in step II.

<br />

### VI. Set up the Global Google Drive Bridge Instance
Attach GoogleDrive to a gameObject in your game's startup scene and assign the configuration asset to its corresponding field and you are all set!

***
## How to Extend it
This package is originally designed as a plugin for some school game projects. Therefore, some of the APIs(request code 100-999) it provides are not very useful for game developers that simply needs to write data into / read data from the google drive. Feel free to remove these APIs as you need. 
<br />

The APIs are split into 3 categories: APIs that are more specific to my own game projects (login, send 6 digit code, send email link), APIs that actually interacts with google drive & google sheets, and APIs for unit testing. I did this because I want the package to be more generic and extendable. If new APIs are required in the future, simply make sure they fell into the correct categories, otherwise the package might crash.