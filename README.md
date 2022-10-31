INTRODUCTION
------------

This is a test task for VP by Alexander Khazagarov.
The test case is an ASP.NET Core Web Api with a RESTfull HTTP service and ReactJS on frontend.

REQUIREMENTS
------------

You must have the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) 
and [nodejs](https://nodejs.org/en/) installed to run.

For data storage, you can use MSSQL.
[MSSQL] (https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

Launching the application
-------------------------

You should perform migrations in your DBMS from the "ToDoR.Migrations" folder. Script number 001 may differ depending on your DBMS.

To connect to the database, you need to set your connection string to "ToDoR\ToDoR.Api\appsettings.json"

You can use Visual Studio or Rider to run and debug.
To do so, open the "ToDoR.sln" file in the project's solution folder and click run "ToDoR.Api".
This will open the console and the web view of Swagger in the browser at Localhost.

To start the frontend, use the "npm start" command in the "ToDoR.Frontend" directory.

DESCRIPTION
-----------

The solution consists of 6 main directories:
* ToDoR.Api - web service.
* ToDoR.Common - Common files for the project, contracts..
* ToDoR.DataAccess- library contains classes for working with the database.
* ToDoR.Migrations - сontains migration files for the database.
* ToDoR.Frontend - Web GUI on React JS.
* Tests - folder containing tests for TasksController.

ToDoR is the simplest example of implementing a diary using C# and React JS. The project can:
* Adding tasks.
* Editing tasks.
* Deleting tasks.
* A task can be marked as completed or not completed.
* For a task, you can set a due date and if the task is overdue, it is marked in red.
* When creating a new task, the minimum title length is checked (at least 10 characters).


REMARK
------

* It was planned to use authorization for multi-user use in the project. This mechanism was not implemented, but some of the classes in the backend logic remained.

* It was planned to use groups for tasks. For example, tasks could be divided into important, deferred, etc. These lists were supposed to be editable. This mechanism was not implemented, but some of the classes on the backend side remained.

