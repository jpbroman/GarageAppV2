
using System;
using System.IO;
using System.Text;

using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using GarageApp;

namespace GarageAppV2.Tests;

internal class FakeUI() : IUI
{
    public string GetInput(string prompt = "")
    {
        throw new NotImplementedException();
    }

    public void ShowMenu()
    {
        throw new NotImplementedException();
    }

    public void ShowMessage(string message)
    {
        return;
    }

    public void ShowVehicles(IEnumerable<Vehicle> vehicles)
    {
        throw new NotImplementedException();
    }

    public Vehicle? VehicleToPark()
    {
        throw new NotImplementedException();
    }

    public string VehicleToUnpark()
    {
        throw new NotImplementedException();
    }
}

public class UnitTest2
{
    [Fact]
    public void TestCreateGarage()
    {
        Garage garage = new Garage();
        Assert.NotNull(garage);
    }

    [Fact]
    public void TestAddVehicle()
    {
        Garage garage = new Garage();
        Vehicle vehicle = new Vehicle(Vehicle.VehicleTypeE.Car, "Toyota", "Red", "ABC123", 5);
        Assert.True(garage.ParkVehicle(vehicle));
        Assert.Single(garage.GetParkedVehicles());
    }

    [Fact]
    public void TestRemoveVehicle()
    {
        Garage garage = new Garage();
        Vehicle vehicle = new Vehicle(Vehicle.VehicleTypeE.Car, "Toyota", "Red", "ABC123", 5);
        garage.ParkVehicle(vehicle);
        Assert.Single(garage.GetParkedVehicles());
        string regNumber = (vehicle.RegNumber != null)? vehicle.RegNumber: "EMPTY";
        garage.UnparkVehicle(regNumber);
        Assert.Empty(garage.GetParkedVehicles());
    }

    [Fact]
    public void TestListVehicles()
    {
        Garage garage = new Garage();
        Vehicle vehicle1 = new Vehicle(Vehicle.VehicleTypeE.Car, "Toyota", "Red", "ABC123", 5);
        Vehicle vehicle2 = new Car("Honda", "Blue", "XYZ789", 5, Car.CarTypeE.SUV, Car.TransmissionE.Manual);
        Vehicle vehicle3 = new Motorcycle("Haeley", "Black", "POI123", 1, Motorcycle.McTypeE.Cruiser, Motorcycle.EngineTypeE.FourStroke);
        Vehicle vehicle4 = new Airplane("Boeing", "White", "JKL012", 160, Airplane.AirplaneTypeE.Commercial, 4);
        Vehicle vehicle5 = new Bus("Volvo", "Yellow", "MNO345", 50, Bus.BusTypeE.City);
        Vehicle vehicle6 = new Boat("Yamaha", "Blue", "PQR678", 8, Boat.BoatTypeE.Sailboat, 30);
        garage.ParkVehicle(vehicle1);
        garage.ParkVehicle(vehicle2);
        garage.ParkVehicle(vehicle3);
        garage.ParkVehicle(vehicle4);
        garage.ParkVehicle(vehicle5);
        garage.ParkVehicle(vehicle6);

        var sw = new StringWriter();  // redirect stdout from class under test
        Console.SetOut(sw);
        foreach (var v in garage.GetParkedVehicles())
        {
            Console.WriteLine(v);
        }
        string result = sw.ToString();
        Assert.Contains("Toyota, Red, ABC123", result);
        Assert.Contains("Honda, Blue, XYZ789", result);
        Assert.Contains("Haeley, Black, POI123", result);
        Assert.Contains("Boeing, White, JKL012", result);
        Assert.Contains("Volvo, Yellow, MNO345", result);
        Assert.Contains("Yamaha, Blue, PQR678", result);
    }

    [Fact]
    public void TestGetObjectData()
    {
        Garage garage = new Garage();
        Vehicle vehicle = new Car("Honda", "Blue", "XYZ789", 5, Car.CarTypeE.SUV, Car.TransmissionE.Manual);
        garage.ParkVehicle(vehicle);
        string result = garage.GetVehicle(vehicle.RegNumber).ToString();
        Assert.Contains("Car, Honda", result);
        Assert.Contains("Blue", result);
        Assert.Contains("XYZ789", result);
        vehicle = new Motorcycle("Haeley", "Black", "POI123", 1, Motorcycle.McTypeE.Cruiser, Motorcycle.EngineTypeE.FourStroke);
        garage.ParkVehicle(vehicle);
        result = garage.GetVehicle(vehicle.RegNumber).ToString();
        Assert.Contains("Haeley", result);
        Assert.Contains("Black", result);    
        Assert.Contains("POI123", result);
    }
    
    [Fact]
    public void TestSaveToFile()
    {
        Garage garage = new Garage();
        Vehicle vehicle1 = new Car("Honda", "Blue", "XYZ789", 6, Car.CarTypeE.SUV, Car.TransmissionE.Manual);
        Vehicle vehicle2 = new Car("Skoda", "Red", "ABC123", 5, Car.CarTypeE.Sedan, Car.TransmissionE.Automatic);
        garage.ParkVehicle(vehicle1);
        garage.ParkVehicle(vehicle2);

        string filePath = "test_garage.txt";
        IUI ui = new ConsoleUI();
        GarageManager gm = new GarageManager(garage, ui);
        gm.SaveParkedVehiclesToFile(filePath);
        Assert.True(File.Exists(filePath));
        string content = File.ReadAllText(filePath);
    }

    [Fact]
    public void TestLoadFromFile()
    {
        Garage garage = new Garage();
        GarageManager gm = new GarageManager(garage, new FakeUI());
        string filePath = "test_garage.txt";

        File.WriteAllText(filePath, "Car, Volvo, Red, JKL789, 5, SUV, Automatic\n" + 
            "Boat, Eka, Yellow, JPB888, 3, Other, 5\n");
        gm.LoadParkedVehiclesFromFile(filePath);
        Assert.Equal(2, garage.GetParkedVehicles().Count());
        
        Assert.Contains(garage.GetParkedVehicles(), v => v != null && v.RegNumber == "JKL789");
        Assert.Contains(garage.GetParkedVehicles(), v => v != null && v.RegNumber == "JPB888");
    }


    [Fact]
    public void TestListVehiclesByProperties()
    {
        Garage garage = new Garage();
        GarageManager gm = new GarageManager(garage, new FakeUI());

        Vehicle vehicle1 = new Car("Honda", "Blue", "XYZ789", 5, Car.CarTypeE.SUV, Car.TransmissionE.Manual);
        Vehicle vehicle2 = new Boat("Flipper", "Red", "ABC123", 6, Boat.BoatTypeE.Sailboat, 8);
        garage.ParkVehicle(vehicle1);
        garage.ParkVehicle(vehicle2);

        string vData = "";
        // Find all red vehicles regardless of type or make
        foreach (var v in gm.FindVehiclesByProp(Vehicle.VehicleTypeE.Unknown, null, "Red"))
            vData += v.ToString();

        File.WriteAllText("/tmp/test.out",vData);
        Assert.Contains("Flipper, Red, ABC123", vData);

        vData = string.Empty;
        foreach (var v in gm.FindVehiclesByProp(Vehicle.VehicleTypeE.Car, null, null))
            vData += v.ToString();
        
        Assert.Contains("Honda, Blue, XYZ789", vData);
        Assert.DoesNotContain("Flipper, Red, ABC123", vData);

        vData = string.Empty;
        garage.ParkVehicle(new Car("Honda", "Black", "AEF456", 6, Car.CarTypeE.Hatchback, Car.TransmissionE.Automatic));
        garage.ParkVehicle(new Airplane("Flyer2", "Blue", "POI234", 8, Airplane.AirplaneTypeE.Private, 2));
        foreach (var v in gm.FindVehiclesByProp(Vehicle.VehicleTypeE.Unknown, "Honda", "Black"))
            vData += v.ToString();

        Assert.Contains("Honda, Black, AEF456", vData);
        Assert.DoesNotContain("Honda, Blue, XYZ789", vData);
        Assert.DoesNotContain("Flyer2, Blue, POI234", vData);

        // All Blue vehicles regardless of type or make
        vData = string.Empty;
        garage.ParkVehicle(new Motorcycle("Harley", "Blue", "QWE567", 2, Motorcycle.McTypeE.Cruiser, Motorcycle.EngineTypeE.FourStroke));
        foreach (var v in gm.FindVehiclesByProp(Vehicle.VehicleTypeE.Unknown, null, "Blue"))
            vData += v.ToString();
        Assert.Contains("Honda, Blue, XYZ789", vData);
        Assert.Contains("Flyer2, Blue, POI234", vData);
        Assert.Contains("Harley, Blue, QWE567", vData);
        Assert.DoesNotContain("Honda, Black, AEF456", vData);
    }
    [Fact]
    public void TestUniqueRegNumbers()
    {
        Garage garage = new Garage();
        Vehicle vehicle1 = new Car("Honda", "Blue", "XYZ789", 6, Car.CarTypeE.SUV, Car.TransmissionE.Manual);
        Vehicle vehicle2 = new Car("Skoda", "Red", "ABC123", 5, Car.CarTypeE.Sedan, Car.TransmissionE.Automatic);
        Vehicle vehicle3 = new Boat("Flipper", "Red", "ABC123", 8, Boat.BoatTypeE.Sailboat, 10); // duplicate reg number
        Assert.True(garage.ParkVehicle(vehicle1));
        Assert.True(garage.ParkVehicle(vehicle2));
        Assert.False(garage.ParkVehicle(vehicle3)); // should fail due to duplicate reg number
    }
    [Fact]
    public void TestAddNotRecognizedVehicleType()
    {
        Vehicle? vehicle1 = null;
        Garage garage = new Garage();
        var sw = new StringWriter();  // redirect stdout from class under test
        Console.SetOut(sw);
        Assert.Throws<ArgumentException>(() => vehicle1 = VehicleFactory.CreateVehicleFromData("c", "TestMake, TestColor, TST123, 5"));
//        Vehicle? vehicle1 = VehicleFactory.CreateVehicleFromData("c", "TestMake, TestColor, TST123, 5");
        Assert.Null(vehicle1);
        string result = sw.ToString();
        Assert.Null(vehicle1);
    }

    
    [Fact]
    public void TestInvalidRegNumberFormat()
    {
        Vehicle? vehicle = null;

        Assert.Throws<ArgumentException>(() => vehicle = new Car("Honda", "Blue", "INVALID", 
            6, Car.CarTypeE.SUV, Car.TransmissionE.Manual));
        Assert.Null(vehicle);
    }
 }