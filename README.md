This is the GarageExercise project

It is written in C# and allow you to manage a garage with several vehicles of various types:

* Motorcycle
* Airplane
* Car
* Bus
* Boat

The user can enter new vehicles via the main menu or populate the garage by giving a file name to the application at start. Please
note that a capacity is the first input parameter on the commandline. If no arguments are supplied, the user will be asked to enter
initial capacity for the garage.

Examples:

> GarageExercise 12 garage1.inv
- This will create a garage with room for 12 vehicles and attempt to populate it from data in the file garage1.inv. 
  (File suffix is of no importance)

> GarageExercise 8
- This will create an empty garage with room for 8 vehicles, which can be populated from application menu.

Take a look at GarageExercise.Tests/Resources/garage1.inv to get hang of the load- and save file format.
A simple guide to each vehicle entry:
VehicleTpe: make, color, registration number, model, <additional properties comma separated>

For example:  
Car: Saab, blue, SAB111, Sedan, Manual  
Bus: Volvo, Red, VOL222, City, 50  
Boat: Fisher1, red, FIS331, Motorboat, 16  

A tip: Start by adding one of each vehicle from the menu and you will get the hang of what properties there are for each vehicle.
An absolute safe way is ofcourse to look at the class files in the Models folder.

