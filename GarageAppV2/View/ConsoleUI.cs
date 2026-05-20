using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using static GarageApp.Vehicle;

namespace GarageApp;
public class ConsoleUI : IUI
{
    private static string MainMenu = "1. Add a vehicle\n2. Remove a vehicle\n3. Display garage contents\n" +
         "4. Find vehicle by properties\n5. Save inventory to file\n6. Load inventory from file\n0. Exit";

    public void ShowMenu()
    {
        Console.WriteLine("Garage Application Menu:");
        Console.WriteLine(MainMenu);
        Console.Write("Select option: ");
    }

    public string GetInput(string prompt) {
        return Utils.SafeInput(prompt);
    }

    public Vehicle? VehicleToPark()
    {
        int nSeats = 0;
        Vehicle.VehicleTypeE vehicleType = Utils.PromptEnumSelection<Vehicle.VehicleTypeE>("Type of vehicle:");
        string make = GetInput("Enter make: ");
        string color = GetInput("Enter color: ");
        string regNumber = GetInput("Enter registration number: ");
        while (!int.TryParse(GetInput("Enter number of seats: "), out nSeats))
        {
            Console.WriteLine("Enter a valid integer for number of seats.");
        }
        return GetVehicleTypeData(new Vehicle(vehicleType, make, color, regNumber, nSeats));
    }

    public string VehicleToUnpark()
    {
        return (GetInput("Enter registration number: "));
    }

    public void ShowVehicles(IEnumerable<Vehicle> vehicles)
    {
        var vehicleList = vehicles?.ToList() ?? new List<Vehicle>();

        if (!vehicleList.Any())
        {
            Console.WriteLine("No vehicles parked.");
            return;
        }

        // Table headers
        string[] headers = { "Type", "Make", "Color", "RegNumber", "Seats", "Details" };

        // Collect rows as string arrays
        var rows = new List<string[]>();

        foreach (var v in vehicleList)
        {
            string details = v switch
            {
                Car car => $"CarType: {GetEnumName(car, "CarType")}, Transmission: {GetEnumName(car, "Transmission")}",
                Motorcycle mc => $"McType: {GetEnumName(mc, "McType")}, Engine: {GetEnumName(mc, "EngineType")}",
                Airplane ap => $"Type: {GetEnumName(ap, "AirplaneType")}, Engines: {GetPrivatePropertyValue<int>(ap, "NumberOfEngines")}",
                Boat boat => $"BoatType: {GetEnumName(boat, "BoatType")}, Length: {GetPrivatePropertyValue<double>(boat, "Length"):0.0}m",
                Bus bus => $"BusType: {GetEnumName(bus, "BusType")}",
                _ => ""
            };

            rows.Add(new string[]
            {
                v.Type.ToString(),
                v.Make,
                v.Color,
                v.RegNumber ?? "",
                v.NumberOfSeats.ToString(),
                details
            });
        }

        // Max width for columns
        int[] maxWidths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            int maxCol = rows.Select(r => r[i]?.Length ?? 0).DefaultIfEmpty(0).Max();
            maxWidths[i] = Math.Max(headers[i].Length, maxCol);
        }

        string PadCell(string s, int width) => s.PadRight(width);

        // Print header
        string headerLine = string.Join(" | ", headers.Select((h, i) => PadCell(h, maxWidths[i])));
        Console.WriteLine(headerLine);

        // Print separator
        string separator = string.Join("-+-", maxWidths.Select(w => new string('-', w)));
        Console.WriteLine(separator);

        // Print rows
        foreach (var row in rows)
        {
            string line = string.Join(" | ", row.Select((c, i) => PadCell(c ?? "", maxWidths[i])));
            Console.WriteLine(line);
        }

        Console.WriteLine();
    }

    // Helper to get private enum property names
    private string GetEnumName(object obj, string propertyName)
    {
        var prop = obj.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (prop != null)
        {
            var val = prop.GetValue(obj);
            return val?.ToString() ?? "";
        }
        return "";
    }

    // Helper to get private typed property values 
    private T GetPrivatePropertyValue<T>(object obj, string propertyName)
    {
        var prop = obj.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (prop != null && prop.GetValue(obj) is T val)
            return val;
        return default!;
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public Vehicle? GetVehicleTypeData(Vehicle prototype)
    {
        try
        {
            // Common data from prototype:
            string make = prototype.Make;
            string color = prototype.Color;
            string reg = prototype.RegNumber ?? "";
            int seats = prototype.NumberOfSeats;

            switch (prototype.Type.ToString().ToLower())
            {
                case "car":
                    {
                        var carType = Utils.PromptEnumSelection<Car.CarTypeE>("Select a Car Type:");
                        var transmission = Utils.PromptEnumSelection<Car.TransmissionE>("Transmission:");

                        return new Car(make, color, reg, seats, carType, transmission);
                    }

                case "motorcycle":
                    {
                        var mcType = Utils.PromptEnumSelection<Motorcycle.McTypeE>("Motorcycle Type:");
                        var engine = Utils.PromptEnumSelection<Motorcycle.EngineTypeE>("Engine Type");

                        return new Motorcycle(make, color, reg, seats, mcType, engine);
                    }

                case "airplane":
                    {
                        var airplaneType = Utils.PromptEnumSelection<Airplane.AirplaneTypeE>("Airplane Type");
                        if (!int.TryParse(Utils.SafeInput("Number of engines"), out int engines) ||
                            engines < 0) 
                        {
                            Console.WriteLine("Invalid number of engines.");
                            return null;                            
                        }
                        return new Airplane(make, color, reg, seats, airplaneType, engines);
                    }

                case "boat":
                    {
                        var boatType = Utils.PromptEnumSelection<Boat.BoatTypeE>("Boat Type");

                        Console.Write("Length in meters (double): ");
                        if (!double.TryParse(Console.ReadLine(), out double length) || length < 0)
                        {
                            Console.WriteLine("Invalid length.");
                            return null;
                        }

                        return new Boat(make, color, reg, seats, boatType, length);
                    }

                case "bus":
                    {
                        var busType = Utils.PromptEnumSelection<Bus.BusTypeE>("Bus Type");

                        return new Bus(make, color, reg, seats, busType);
                    }

                default:
                    Console.WriteLine("Unknown vehicle type.");
                    return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

}