Instructions for running the app:
1. The app will launch in your terminal with the following flow:
       * You’ll be prompted to log in or create an account.
       * After logging in, you can set your house, view house info, 
       check student count, and see membership percentages.

Summary of design decisions and architecture
- 

How we used Singleton, Observer, and Strategy patterns:
Singleton:

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