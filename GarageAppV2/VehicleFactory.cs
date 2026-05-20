using System.Dynamic;

namespace GarageApp;
public class VehicleFactory
{   
 
    // public static Vehicle? CreateVehicle(IUI ui, string type, string make, string color, string regNumber, int numSeats)
    // {
    //     Vehicle? vehicle = null;
    //     // base data
    //     Vehicle.VehicleTypeE vehicleType = Enum.TryParse<Vehicle.VehicleTypeE>(type, true, out var vt) ? vt : Vehicle.VehicleTypeE.Unknown;
    //     try {
    //         vehicle = new Vehicle(vehicleType, make, color, regNumber, numSeats);
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         return null;
    //     }
    // }

    public static Vehicle? CreateVehicleFromData(string type, string data)
    {
        int seats = 0;
        string[] parts = data.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length < 4)
        {
            throw new ArgumentException("Invalid data format for vehicle.");
        }
        string make = parts[0];
        string color = parts[1];
        string regNumber = parts[2];
        if (!int.TryParse(parts[3], out  seats))
        {
            return null;
        }

        switch (type.ToLower().Substring(0, Math.Min(type.Length, 3))) // first 3 letters unique for type
        {
            case "car":
                string cartypeStr = parts[4];
                string transmissionStr = parts[5];
                Car.CarTypeE carType = Enum.Parse<Car.CarTypeE>(cartypeStr, true);
                Car.TransmissionE transmission = Enum.Parse<Car.TransmissionE>(transmissionStr, true);
                Car car = new Car(make, color, regNumber, seats, carType, transmission);
                return car;
            case "mot":
                string mctypeStr = parts[4];
                string engineTypeStr = parts[5];
                Motorcycle.McTypeE mcType = Enum.Parse<Motorcycle.McTypeE>(mctypeStr, true);
                Motorcycle.EngineTypeE engineType = Enum.Parse<Motorcycle.EngineTypeE>(engineTypeStr, true);
                Motorcycle motorcycle = new Motorcycle(make, color, regNumber, seats, mcType, engineType);
                return motorcycle;
            case "bus":
                string bustypeStr = parts[4];
                Bus.BusTypeE busType = Enum.Parse<Bus.BusTypeE>(bustypeStr, true);
                Bus bus = new Bus(make, color, regNumber, seats, busType);
                return bus;
            case "air":
                string airplaneTypeStr = parts[4];
                string numEnginesStr = parts[5];
                Airplane.AirplaneTypeE airplaneType = Enum.Parse<Airplane.AirplaneTypeE>(airplaneTypeStr, true);
                int numEngines = int.Parse(numEnginesStr);
                Airplane airplane = new Airplane(make, color, regNumber, seats, airplaneType, numEngines);
                return airplane;
            case "boa":
                string boatTypeStr = parts[4];
                string lengthStr = parts[5];
                Boat.BoatTypeE boatType = Enum.Parse<Boat.BoatTypeE>(boatTypeStr, true);
                double length = double.Parse(lengthStr);
                Boat boat = new Boat(make, color, regNumber, seats, boatType, length);
                return boat;
            default:
                throw new ArgumentException($"Unknown vehicle type: {type}");
        }
    }
    // private static Car CreateCar(Vehicle vehicle)   
    // {
    //      prompt = ($"Enter car type ({string.Join("/", Enum.GetNames<Car.CarTypeE>())}): ");
    //      Car.CarTypeE carType = Enum.Parse<Car.CarTypeE>(Utils.SafeInput(prompt), true);
    //     // prompt = $"Enter transmission type (Automatic/Manual): ";
    // //    Car.TransmissionE transmission = Enum.Parse<Car.TransmissionE>(Utils.SafeInput(prompt), true);
    //     Car car = new Car(vehicle.Make, vehicle.Color, vehicle.RegNumber, type, trans);
    //     return car;
    // }

    // private static Motorcycle CreateMotorcycle(Vehicle vehicle)
    // {
    //     prompt = ($"Enter motorcycle type ({string.Join("/", Enum.GetNames<Motorcycle.McTypeE>())}): ");
    //     Motorcycle.McTypeE mcType = Enum.Parse<Motorcycle.McTypeE>(Utils.SafeInput(prompt), true);
    //     prompt = "Enter engine type (TwoStroke/FourStroke): ";
    //     Motorcycle.EngineTypeE engineType = Enum.Parse<Motorcycle.EngineTypeE>(Utils.SafeInput(prompt), true);
    //     Motorcycle motorcycle = new Motorcycle(vehicle.Make, vehicle.Color, vehicle.RegNumber, mcType, engineType);
    //     return motorcycle;
    // }

    // private static Bus CreateBus(Vehicle vehicle)
    // {
    //     prompt = ($"Enter bus type ({string.Join("/", Enum.GetNames<Bus.BusTypeE>())}): ");
    //     Bus.BusTypeE busType = Enum.Parse<Bus.BusTypeE>(Utils.SafeInput(prompt), true);
    //     prompt = "Enter seating capacity: ";
    //     int seatingCapacity;
    //     while (!int.TryParse(Utils.SafeInput(prompt), out seatingCapacity))
    //     {
    //         Console.WriteLine("Enter a valid integer for seating capacity:");
    //     }
    //     Bus bus = new Bus(vehicle.Make, vehicle.Color, vehicle.RegNumber, busType, seatingCapacity);
    //     return bus;
    // }

    // private static Airplane CreateAirplane(Vehicle vehicle)
    // {
    //     prompt = ($"Enter airplane type ({string.Join("/", Enum.GetNames<Airplane.AirplaneTypeE>())}): ");
    //     Airplane.AirplaneTypeE airplaneType = Enum.Parse<Airplane.AirplaneTypeE>(Utils.SafeInput(prompt), true);
    //     prompt = "Enter number of engines: ";
    //     int numEngines;
    //     while (!int.TryParse(Utils.SafeInput(prompt), out numEngines))
    //     {
    //         Console.WriteLine("Enter a valid integer for number of engines:");
    //     }
    //     prompt = "Enter number of seats: ";
    //     int numSeats;
    //     while (!int.TryParse(Utils.SafeInput(prompt), out numSeats))
    //     {
    //         Console.WriteLine("Enter a valid integer for number of seats:");
    //     }
    //     Airplane airplane = new Airplane(vehicle.Make, vehicle.Color, vehicle.RegNumber, airplaneType, numEngines, numSeats);
    //     return airplane;
    // }

    // private static Boat CreateBoat(Vehicle vehicle)
    // {
    //     prompt = ($"Enter boat type ({string.Join("/", Enum.GetNames<Boat.BoatTypeE>())}): ");
    //     Boat.BoatTypeE boatType = Enum.Parse<Boat.BoatTypeE>(Utils.SafeInput(prompt), true);
    //     prompt = "Enter length (ft): ";
    //     double length;
    //     while (!double.TryParse(Utils.SafeInput(prompt), out length))
    //     {
    //         Console.WriteLine("Enter a valid number for length:");
    //     }
    //     Boat boat = new Boat(vehicle.Make, vehicle.Color, vehicle.RegNumber, boatType, length);
    //     return boat;
    // }
}   