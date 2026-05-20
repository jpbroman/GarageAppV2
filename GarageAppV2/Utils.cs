namespace GarageApp;

public static class Utils
{
    public static string SafeInput(string prompt)
    {
        string? input = "";
        Console.Write(prompt);
        try {
            input = Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: Incorrect input. {ex.Message}");
        }
        return input?.Trim() ?? "";
    }

    public static T PromptEnumSelection<T>(string prompt) where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        Console.WriteLine(prompt);
        int index = 1;

        foreach (var val in values)
        {
            Console.WriteLine($"{index}. {val}");
            index++;
        }

        Console.Write($"Select an option (1-{values.Length}): ");

        while (true)
        {
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice) &&
                choice >= 1 && choice <= values.Length)
            {
                return values[choice - 1];
            }
            Console.Write($"Invalid input. Please enter a number between 1 and {values.Length}: ");
        }
    } 

}

