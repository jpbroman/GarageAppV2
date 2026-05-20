using System;

namespace GarageApp;

class Program
{
    static void Main(string[] args)
    {
        IUI ui = new ConsoleUI();
        IGarage garage = new Garage();
        var manager = new GarageManager(garage, ui);

        manager.Run();
    }
}
