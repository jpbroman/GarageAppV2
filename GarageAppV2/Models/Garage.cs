using System.Collections.Generic;
using System.Linq;

namespace GarageApp;
public class Garage : IGarage
{
    private readonly List<Vehicle> _vehicles = new List<Vehicle>();

    public bool ParkVehicle(Vehicle? vehicle)
    {
        if (vehicle == null)
            return false;

        if (_vehicles.Any(v => v.RegNumber == vehicle.RegNumber))
            return false; // Already parked

        _vehicles.Add(vehicle);
        return true;
    }

    public bool UnparkVehicle(string regNumber)
    {
        var vehicle = _vehicles.FirstOrDefault(v => v.RegNumber == regNumber);
        if (vehicle == null)
            return false;

        _vehicles.Remove(vehicle);
        return true;
    }

    public Vehicle? GetVehicle(string regNo)
    {
        if (string.IsNullOrWhiteSpace(regNo))
            return null;

        string normalized = regNo
            .Replace("-", "")
            .Replace(" ", "")
            .ToUpper();

        return _vehicles.FirstOrDefault(v =>
            v.RegNumber?
                .Replace("-", "")
                .Replace(" ", "")
                .ToUpper() == normalized);
    }
    public IEnumerable<Vehicle> GetParkedVehicles() => _vehicles.AsReadOnly();
}
