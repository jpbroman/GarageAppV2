using System.Text.RegularExpressions;

namespace GarageApp;
public class Vehicle
{
    public enum VehicleTypeE { Car, Motorcycle, Bus, Airplane, Boat, Unknown};

    public string? RegNumber { get; }
    public string Make { get; }
    public string Color  { get; }
    public VehicleTypeE Type { get; }
    public int NumberOfSeats { get;}

    public Vehicle(VehicleTypeE type, string make, string color, string regNumber, int numSeats)
    {
        Type = type;     
        Make = make;
        Color = color;
        NumberOfSeats = numSeats;
        RegNumber = RegNumberFormat(regNumber); // Validate and format to consisten format.
        if (RegNumber == null)
        {
            throw new ArgumentException("Invalid registration number format");
        }
    }
    protected string? RegNumberFormat(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (Type == VehicleTypeE.Car || Type == VehicleTypeE.Motorcycle || Type == VehicleTypeE.Bus)
        {
            // Cars, busses and motorcycles: 3 letters followed by 3 digits, with optional dash (e.g., ABC123 or ABC-123)
            string cleaned = Regex.Replace(input.ToUpper(), @"[\s-]", "");

            // Validate format: ABC123 where last char can be 0-9 or A-Z
            if (!Regex.IsMatch(cleaned, @"^[A-Z]{3}\d{2}[0-9A-Z]$"))
                return null;

            return cleaned;
        }
        return input.ToUpper(); // For other types, just return uppercase.
    }

    public override string ToString()
        => $"{Type.ToString()}, {Make}, {Color}, {RegNumber}, {NumberOfSeats}";
}

public class Car : Vehicle
{
    public enum CarTypeE { Sedan, Hatchback, SUV, Coupe, Convertible, Wagon, Van, Other };
    public enum TransmissionE { Manual, Automatic}; 
    private CarTypeE CarType { get; }
    private TransmissionE Transmission { get; }
    public Car(string make, string color, string regNumber, int nSeats, CarTypeE carType, TransmissionE transmission) 
        : base(VehicleTypeE.Car, make, color, regNumber, nSeats)
    {
        CarType = carType;
        Transmission = transmission;
    }

    public override string ToString()
        => base.ToString() + $", {CarType}, {Transmission}";

}

public class Motorcycle : Vehicle
{
    public enum McTypeE { Standard, Cruiser, Sport, Touring, OffRoad, Scooter, Other };
    public enum EngineTypeE { TwoStroke, FourStroke, Electric, Other };
    private McTypeE McType { get; set; }
    private EngineTypeE EngineType { get; set; }

    public Motorcycle(string make, string color, string regNumber, int nSeats, McTypeE mcType, EngineTypeE engineType) 
        : base(VehicleTypeE.Motorcycle, make, color, regNumber, nSeats)
    {
        McType = mcType;
        EngineType = engineType;
    }

    public override string ToString()
        => base.ToString() + $", {McType}, {EngineType}";
}

public class Airplane : Vehicle
{
    public enum AirplaneTypeE { Commercial, Private, Cargo, Military, Other };
    private AirplaneTypeE AirplaneType { get; set; }
    private int NumberOfEngines { get; set; }
    public Airplane(string make, string color, string regNumber, int numberOfSeats, AirplaneTypeE airplaneType, 
                    int numberOfEngines) :
                    base(VehicleTypeE.Airplane, make, color, regNumber, numberOfSeats)
    {
        AirplaneType = airplaneType;
        NumberOfEngines = numberOfEngines;
    }

    public override string ToString()
        => base.ToString() + $", {AirplaneType}, {NumberOfEngines}";
}

public class Boat : Vehicle
{
    public enum BoatTypeE { Sailboat, Motorboat, Yacht, Kayak, Canoe, Other };
    private BoatTypeE BoatType { get; set; }
    private double Length { get; set; }  // in meters
    public Boat(string make, string color, string regNumber, int numSeats, BoatTypeE boatType, double length) 
        : base(VehicleTypeE.Boat, make, color, regNumber, numSeats)
    {
        BoatType = boatType;
        Length = length;
    }

    public override string ToString()
        => base.ToString() + $", {BoatType}, {Length}";
}   

public class Bus : Vehicle
{
    public enum BusTypeE { City, Intercity, Coach, Minibus, SchoolBus, Other };
    private BusTypeE BusType { get; set; }
    public Bus(string make, string color, string regNumber, int numberOfSeats, BusTypeE busType) 
        : base(VehicleTypeE.Bus, make, color, regNumber, numberOfSeats)
    {
        BusType = busType;
    }

    public override string ToString()
        => base.ToString() + $", {BusType}";

}
