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
    abstract void ShowMenu();
    abstract Vehicle? VehicleToPark();
    abstract string VehicleToUnpark();
    abstract void ShowVehicles(IEnumerable<Vehicle> vehicles);
    abstract void ShowMessage(string message);
    abstract string GetInput(string prompt = "");
}

public interface IManager
{
    void Run();
}
