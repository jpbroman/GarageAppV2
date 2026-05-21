using System.Dynamic;

namespace GarageApp;
public class VehicleFactory
{   
    public static Vehicle? CreateVehicleFromData(string type, string data)
    {
        int seats = 0;
        string[] parts = data.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length < 3)
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
                throw new ArgumentException($"Unknown vehicle type {type}.");
        }
    }
 }   