using System.Text.RegularExpressions;

namespace GarageApp;
public class GarageManager : IManager
{
    private IGarage _garage;
    private readonly IUI _ui;

    public GarageManager(IGarage garage, IUI ui)
    {
        _garage = garage;
        _ui = ui;
    }

    public void Run()
    {
        while (true)
        {
            _ui.ShowMenu();
            var input = _ui.GetInput();

            switch (input)
            {
                case "1":
                    Vehicle? vehicle = _ui.VehicleToPark();
                    if(_garage.ParkVehicle(vehicle))
                        _ui.ShowMessage($"Vehicle {vehicle} parked successfully.");
                    else
                        _ui.ShowMessage("Failed to park vehicle.");
                    break;

                case "2":
                    string regNumber = _ui.VehicleToUnpark();
                    if(_garage.UnparkVehicle(regNumber))
                        _ui.ShowMessage($"Vehicle {_garage.GetVehicle(regNumber)} unparked.");
                    else
                        _ui.ShowMessage($"Vehicle {regNumber} not found.");
                    break;

                case "3":
                    var parked = _garage.GetParkedVehicles();
                    _ui.ShowVehicles(parked);
                    break;

                case "4":
                    string vehicleTypeStr = _ui.GetInput("Enter vehicle type to search for (or press Enter to skip): ");
                    Vehicle.VehicleTypeE vType = Vehicle.VehicleTypeE.Unknown;
                    if (!string.IsNullOrEmpty(vehicleTypeStr) && !Enum.TryParse<Vehicle.VehicleTypeE>(vehicleTypeStr, true, out vType))
                    {
                        _ui.ShowMessage("Invalid vehicle type. Searching for all types.");
                    }
                    string vMake = _ui.GetInput("Enter make (or press Enter to skip): ");
                    string vColor = _ui.GetInput("Enter color (or press Enter to skip): ");
                    _ui.ShowVehicles(FindVehiclesByProp(vType, vMake, vColor));
                    break;

                case "5":
                    string fileName = _ui.GetInput("Enter filname: ");
                    SaveParkedVehiclesToFile(fileName);
                    break;

                case "6":
                    fileName = _ui.GetInput("Enter filename: ");
                    LoadParkedVehiclesFromFile(fileName);
                    break;
                                        
                case "0":
                    return; // Exit

                default:
                    _ui.ShowMessage("Invalid selection.");
                    break;
            }
        }
    }

    public IEnumerable<Vehicle> FindVehiclesByProp(Vehicle.VehicleTypeE? type = null,
        string? make = null,
        string? color = null)
    {
        Console.WriteLine($"in: {type}, {make}, {color}");

        return _garage.GetParkedVehicles().Where(v =>
            (type.Value  == Vehicle.VehicleTypeE.Unknown || v.Type == type.Value) &&
            (string.IsNullOrWhiteSpace(make) || string.Equals(v.Make, make, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(color) || string.Equals(v.Color, color, StringComparison.OrdinalIgnoreCase)));
    }
    private void LoadParkedVehiclesFromFile(string filePath)
    {
        if (!File.Exists(filePath)) 
        {
            _ui.ShowMessage($"File not found: {filePath}");
            return;
        }
        foreach (string line in File.ReadLines(filePath))
        {
            int index = line.IndexOf(',');

            string vehicleType = line[..index].Trim();
            string vehicleaData = line[(index + 1)..].Trim();

            Vehicle? vehicle = VehicleFactory.CreateVehicleFromData(vehicleType, vehicleaData);
            if (_garage.ParkVehicle(vehicle))
                _ui.ShowMessage($"Vehicle: {vehicle} parked.");
        }
    }

    protected void SaveParkedVehiclesToFile(string filePath)
    {
        IEnumerable<Vehicle> vehicles = _garage.GetParkedVehicles();

        File.WriteAllLines(
            filePath,
            vehicles.Select(v => v.ToString()),
            System.Text.Encoding.UTF8);
    }
 }
