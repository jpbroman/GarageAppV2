using System.Collections.Generic;

namespace GarageApp;
public interface IGarage
{
    bool ParkVehicle(Vehicle? vehicle);
    bool UnparkVehicle(string licensePlate);
    Vehicle? GetVehicle(string regNo);

    IEnumerable<Vehicle> GetParkedVehicles();
}

public interface IUI
{
    void ShowMenu();
    Vehicle? VehicleToPark();
    string VehicleToUnpark();
    void ShowVehicles(IEnumerable<Vehicle> vehicles);
    void ShowMessage(string message);
    string GetInput(string prompt = "");
}

public interface IManager
{
    void Run();
}
