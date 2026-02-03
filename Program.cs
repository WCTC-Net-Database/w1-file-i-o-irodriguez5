using System;

/// <summary>
/// Week 1: File I/O Basics - Console RPG Character Manager
///
/// This program teaches fundamental file operations in C#:
/// - Reading data from CSV files using File.ReadAllLines()
/// - Parsing comma-separated values using String.Split()
/// - Writing data back to files using File.WriteAllLines()
///
/// The menu structure is provided for you to review and understand.
/// Your tasks are marked with TODO comments throughout the code.
/// </summary>
class Program
{
    // The path to our data file - we'll read and write character data here
    static string filePath = "input.csv";

    static void Main()
    {
        // Welcome message
        Console.WriteLine("=== Console RPG Character Manager ===");
        Console.WriteLine("Week 1: File I/O Basics\n");

        // Main program loop - keeps running until user chooses to exit
        bool running = true;
        while (running)
        {
            // Display the menu options
            DisplayMenu();

            // Get user's choice
            Console.Write("\nEnter your choice: ");
            string? choice = Console.ReadLine();
            choice = choice?.Trim();

            // Process the user's choice using a switch statement
            switch (choice)
            {
                case "1":
                    DisplayAllCharacters();
                    break;
                case "2":
                    AddCharacter();
                    break;
                case "3":
                    LevelUpCharacter();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("\nGoodbye! Thanks for playing.");
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    break;
            }

            // Add a blank line for readability between menu displays
            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    /// <summary>
    /// Displays the main menu options to the user.
    /// This is complete - review it to understand the structure.
    /// </summary>
    static void DisplayMenu()
    {
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("1. Display All Characters");
        Console.WriteLine("2. Add New Character");
        Console.WriteLine("3. Level Up Character");
        Console.WriteLine("0. Exit");
    }

    /// <summary>
    /// Reads all characters from the CSV file and displays them.
    ///
    /// CSV Format: Name,Class,Level,HP,Equipment
    /// Example: John,Fighter,1,10,sword|shield|potion
    /// </summary>
    static void DisplayAllCharacters()
    {
        Console.WriteLine("\n=== All Characters ===\n");

        // Step 1: Read all lines from the file
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Data file not found.");
            return;
        }
        string[] lines = File.ReadAllLines(filePath);

        // Step 2: Loop through each line and display it
        if (lines.Length == 0)
        {
            Console.WriteLine("No characters found.");
            return;
        }

        foreach (string line in lines)
        {
            // Skip empty/whitespace lines
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var cols = line.Split(',');
            if (cols.Length < 5)
            {
                Console.WriteLine("Skipping malformed line.");
                continue;
            }

            // Trim each field to avoid stray spaces
            var name = cols[0].Trim();
            var profession = cols[1].Trim();
            var level = cols[2].Trim();
            var hp = cols[3].Trim();
            var equipmentRaw = cols[4].Trim();

            var equipment = equipmentRaw.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Console.WriteLine($"\nName: {name}");
            Console.WriteLine($"Profession: {profession}");
            Console.WriteLine($"Level: {level}");
            Console.WriteLine($"HP: {hp}");
            Console.WriteLine("Equipment:");
            foreach (var eq in equipment)
            {
                Console.WriteLine($" - {eq}");
            }
            Console.WriteLine();
            Console.WriteLine("___________________________________");
        }
    }

    /// <summary>
    /// Prompts the user for character information and adds it to the file.
    /// </summary>
    static void AddCharacter()
    {
        Console.WriteLine("\n=== Add New Character ===\n");

        // Prompting for character details
        Console.Write("Enter character name: ");
        string? name = Console.ReadLine();
        name = (name ?? string.Empty).Trim();

        Console.Write("Enter character class: ");
        string? profession = Console.ReadLine();
        profession = (profession ?? string.Empty).Trim();

        // Validate Level as a number (re-prompt until valid)
        int levelValue;
        while (true)
        {
            Console.Write("Enter character level (number): ");
            string? levelInput = Console.ReadLine();
            levelInput = levelInput?.Trim();
            if (int.TryParse(levelInput, out levelValue))
                break;

            Console.WriteLine("Invalid level. Please enter a number.");
        }

        // Validate HP as a number (re-prompt until valid)
        int hpValue;
        while (true)
        {
            Console.Write("Enter character HP (number): ");
            string? hpInput = Console.ReadLine();
            hpInput = hpInput?.Trim();
            if (int.TryParse(hpInput, out hpValue))
                break;

            Console.WriteLine("Invalid HP. Please enter a number.");
        }

        Console.Write("Enter character equipment (separate items with '|'): ");
        string? equipment = Console.ReadLine();
        equipment = (equipment ?? string.Empty).Trim();

        // Format as CSV line
        string newLine = $"{name},{profession},{levelValue},{hpValue},{equipment}";

        // Append to file (creates file if it doesn't exist); use platform-agnostic newline
        File.AppendAllText(filePath, newLine + Environment.NewLine);

        Console.WriteLine("Character added.");
    }

    /// <summary>
    /// Finds a character by name and increases their level by 1.
    /// </summary>
    static void LevelUpCharacter()
    {
        Console.WriteLine("\n=== Level Up Character ===\n");

        // Read all lines from the file
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Data file not found.");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            Console.WriteLine("No characters found.");
            return;
        }

        Console.WriteLine("Character List: ");
        for (int i = 0; i < lines.Length; i++)
        {
            var cols = lines[i].Split(',');
            var name = cols.Length > 0 ? cols[0].Trim() : "(unknown)";
            var level = cols.Length > 2 ? cols[2].Trim() : "(n/a)";
            Console.WriteLine($"{i + 1}. {name}: Level {level}");
        }

        // Prompt for character number to level up
        Console.Write("\nEnter the number of the character to level up: ");
        string? input = Console.ReadLine();
        input = input?.Trim();

        if (!int.TryParse(input, out int selection))
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            return;
        }

        int index = selection - 1;
        if (index < 0 || index >= lines.Length)
        {
            Console.WriteLine("Selection out of range.");
            return;
        }

        // Parse the selected line, increment level, rebuild the line
        var parts = lines[index].Split(',');
        if (parts.Length < 5)
        {
            Console.WriteLine("Selected character data is malformed.");
            return;
        }

        // parts: [0]=Name, [1]=Class, [2]=Level, [3]=HP, [4]=Equipment
        parts[0] = parts[0].Trim();
        parts[1] = parts[1].Trim();
        parts[2] = parts[2].Trim();
        parts[3] = parts[3].Trim();
        parts[4] = parts[4].Trim();

        if (!int.TryParse(parts[2], out int currentLevel))
        {
            Console.WriteLine("Character level is not a valid number.");
            return;
        }

        int newLevel = currentLevel + 1;
        parts[2] = newLevel.ToString();

        // Rebuild the CSV line; keep existing equipment as-is
        lines[index] = string.Join(",", parts);

        // Write all lines back to the file
        File.WriteAllLines(filePath, lines);

        Console.WriteLine($"Leveled up {parts[0]} to Level {newLevel}.");
    }
    /*
     <summary>
        Lists all characters with their current level.
        User selects a character by number.
        Program validates selection and parses the CSV line:
         [0]=Name, [1]=Class, [2]=Level, [3]=HP, [4]=Equipment
        Level is incremented by 1 and the CSV line is rebuilt.
        All lines are written back to input.csv.
        A summary is displayed:
         Name, Class, previous level -> new level, HP, equipment item count.
    </summary>
     */
}