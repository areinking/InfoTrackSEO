# InfoTrack SEO API and UI

## Solution Layout

* InfoTrackSEO.Api
  * The entry into the backend services
  * Runs on port 5001 locally
* InfoTrackSEO.Domain
  * The main business logic of the app
  * Also contains the repository layer, currently using EF
  * The domain protects its integrity as tightly as it can
* InfoTrackSEO.Tests
  * Unit tests and integration tests
* InfoTrackSEO.UITests
  * Selenium tests for the UI
* InfoTrackSEO.Web
  * Static files served to interact with the API
  * Used KnockoutJS
  * Runs on port 54321 locally

## Debugging

* Install Sql Server LocalDb if you do not have it
  * Create the InfoTrackSEO database
  * The connection string is set for trusting windows authentication for the current user
  * The database `should` automatically be migrated up when debugging
* Debug with a script:
  * Open a powershell terminal
  * Make sure your current session can run scripts with `Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope Process`
  * Run debug.ps1 `.\debug.ps1`
    * Two new terminals will be open to run the API and Web projects
  * When finished, just close the new terminals that were opened
* Debug using VS Code:
  * On the debug tab, launch the api project with .NET Core Launch (api) selection
  * Launch the web project with the .NET CoreLaunch (web) selection
* Debug using Visual Studio:
  * Right click on the API project and Web project, go to debug and then run
* A list will populate with previous entries
* Enter keywords, a url to search for, and choose the search engine, and then click Submit
  * The results will be added to the list below
* Click the header fields to sort as needed
* Check the View Detail box to see the links that were returned by the search engine

## UI

* Home screen loading
![Alt text](ReadMe.Images/InfoTrackSEO_1.png?raw=true "Home page after reading past results")

* Home screen after loading past results
![Alt text](ReadMe.Images/InfoTrackSEO_2.png?raw=true "Home page after reading past results")

* Sorting by Positions header
![Alt text](ReadMe.Images/InfoTrackSEO_3.png?raw=true "Sorting by Positions header")

* Expanding detail
![Alt text](ReadMe.Images/InfoTrackSEO_4.png?raw=true "Expanding detail")

* Searching ...
![Alt text](ReadMe.Images/InfoTrackSEO_5.png?raw=true "Searching ...")

* New result
![Alt text](ReadMe.Images/InfoTrackSEO_6.png?raw=true "New result")
