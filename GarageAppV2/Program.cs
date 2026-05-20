using System;

namespace GarageApp;

class Program
{
    static void Main(string[] args)
    {
        IUI ui = new ConsoleUI();
        IGarage garage = new Garage();
        var manager = new GarageManager(garage, ui);

        if (args.Length > 0)
        {
            if (!File.Exists(args[0]))
            {
                ui.ShowMessage($"File {args[0]} not found. Starting with empty garage.");
            }
            else
            {
                manager.LoadParkedVehiclesFromFile(args[0]);
                ui.ShowMessage($"Loaded parked vehicles from {args[0]}");
            }
        }

        manager.Run();
    }
}
