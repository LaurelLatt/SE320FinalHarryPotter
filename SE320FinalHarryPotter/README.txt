Instructions for running the app:
1. The app will launch in your terminal with the following flow:
       * You’ll be prompted to log in or create an account.
       * After logging in, you can set your house, view house info, 
       check student count, and see membership percentages.

Summary of design decisions and architecture
- Separation of Concerns
   - UI.cs: Handles all user interaction (text menus, inputs, and outputs)
   - User.cs, Admin.cs: Contains business logic like user login, setting 
   houses, and admin tools
   - SqliteOps.cs, SqlDataAccess.cs: Abstracts away direct database operations 
   for reuse and decoupling
- The IDataAccess interface allows the application to switch between different 
database backends if needed.
- User and Admin classes depend on IDataAccess rather than concrete database 
classes, following dependency inversion.

How we used Singleton, Observer, and Strategy patterns:
Singleton:
The Singleton Pattern is used in this project to restrict the instantiation of the UI class to a single object. 
This ensures that only one instance of the user interface exists and is shared across the application, 
preventing inconsistent behavior or duplicated input loops.
* Singleton Class: UI
* Access Point: UI.Instance
* Used By: Any part of the app that needs to interact with user input and menus
In UI.cs, the constructor is marked private so no other class can instantiate UI directly. 
Instead, a static property Instance checks whether an instance already exists, if not,
it creates one and returns it. This pattern ensures that all parts of the application reference the same UI object, 
promoting consistency and avoiding unnecessary object creation. It also simplifies access, since no constructor 
parameters or dependency injection is required for UI-related tasks.

Observer:
The Observer Pattern is used in this project to demonstrate decoupled communication 
between the Admin class (the subject) and any listeners interested in house creation 
events (observers). It’s implemented in this project as a demonstration of 
modular design and design pattern usage. While the pattern is fully coded, it currently 
does not perform any critical logic or affect application behavior
* Subject: Admin class
* Observer Interface: IHouseObserver
* Concrete Observer: RandomLoggerObserver
In Admin.cs, an interface named IHouseObserver defines a single 
method: void OnHouseCreated(string houseName);
This method is called whenever a new house is created. The class RandomLoggerObserver 
implements this interface. It simply logs the name of the house when notified. The Admin 
class holds a list of observers and exposes an AddObserver() method. 
Inside the CreateHouse() method, after a house is created via the 
IDataAccess interface, NotifyObserver(houseName) is called to inform 
all registered observers.

Strategy: 
The Strategy Pattern is used in this project to abstract and decouple the data access layer from the business logic layer. 
This is accomplished through the IDataAccess interface, which defines a consistent set of methods for database interaction, 
and the SqlDataAccess class, which provides a concrete implementation using SQLite.
* Strategy Interface: IDataAccess
* Concrete Strategy: SqlDataAccess
* Used By: User, Admin, HousePicker, HouseDescriptor, and others
All logic-layer classes depend on the IDataAccess interface rather than directly calling SQL commands or 
using a specific database engine. This enables the application to easily swap out SqlDataAccess for another data
source in the future — such as a mock data provider, a cloud database, or even a flat file, without changing any core logic.
For example, in testing, SqlDataAccess can be initialized with a separate in-memory SqliteOps instance to allow fast 
and isolated unit testing. This pattern also supports future extensibility, where multiple IDataAccess strategies 
could be created (e.g., InMemoryDataAccess, PostgresDataAccess, etc.) depending on deployment context.
This implementation demonstrates flexible, interchangeable behavior, a key principle of the Strategy Pattern.

