# WaterMango 
Sample project requested by TapMango. The project is divded into two smaller projects currently separated by folders however, in the realworld one would typically create new git projects or at the very least different branches.

More information about and how to start the invidual projects are below.

Please note you can start the applications in either order however, they both need to be started in order for the application to function. 

### API Folder (Backend)
RESTFul WebService written in .NET Core 3.0. The purpose of this application is to store and track states of various plant objects in a database. For the purpose of this project the database is a simple "in-memory" storage in which is populated during initialization. Please see the "PlantRepository" constructor for more details. 

The easiest way to launch this application is to navigate to the "API\Watermango" folder and enter the following command into the terminal application of your choice (assuming you have .net core 3.0 installed and configured correctly...): 

`dotnet run`

Upon entering the command you should be greeted with a few lines in the terminal stating the application has started successfully and is now accessible at the configured URL/Port for example: 

`Now listening on: https://localhost:5001`

### UI Folder (Frontend)
The frontend of the application was written using Angular 8. The provides the user with the ability to complete the desired user stories via integration with the API service. The easiest way to launch this application is to navigate to the "UI\WaterMango" folder in the terminal of your choice and enter the following commands:

`npm install`

This will install all required packages to run the angular application.

`ng serve`

This will start the application and allow you to access it from the following url in a browser of your choice: 

`https://localhost:4200/` 

More information about starting / install the packages is available in the README.MD provided by angular in the UI folder. 