using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class JournalEntry
{
    public string Prompt { get; set; }
    public string Response { get; set; }
    public string Date { get; set; }

    public JournalEntry(string prompt, string response, string date)
    {
        Date = date;
        Prompt = $"{Date}: {prompt}"; // Add date to the prompt.
        Response = response;
    }

    public string ToCsvString()
    {
        // Escape quotes in response and replace line breaks with spaces for CSV compatibility.
        string escapedResponse = Response.Replace("\"", "\"\"").Replace("\n", " ");
        return $"\"{Date}\",\"{Prompt}\",\"{escapedResponse}\"";
    }
}

class Journal
{
    private List<JournalEntry> _entries = new List<JournalEntry>();

    public void AddEntry(JournalEntry entry)
    {
        _entries.Add(entry);
    }

    public void DisplayEntries()
    {
        foreach (var entry in _entries)
        {
            Console.WriteLine($"Date: {entry.Date}");
            Console.WriteLine($"Prompt: {entry.Prompt}");
            Console.WriteLine($"Response: {entry.Response}\n");
        }
    }

    public void SaveToCsvFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine("Date,Prompt,Response");
            foreach (var entry in _entries)
            {
                writer.WriteLine(entry.ToCsvString());
            }
        }
    }

    public void LoadFromCsvFile(string filename)
    {
        _entries.Clear();
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                bool isFirstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false; // Skip the header line.
                        continue;
                    }

                    string[] parts = line.Split(',');
                    if (parts.Length >= 3)
                    {
                        string date = parts[0];
                        string prompt = parts[1];
                        string response = parts[2];
                        _entries.Add(new JournalEntry(prompt, response, date));
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found.");
        }
    }

    public void SearchByDate(string date)
    {
        var matchingEntries = _entries.Where(entry => entry.Date == date).ToList();
        if (matchingEntries.Count > 0)
        {
            Console.WriteLine($"Entries for {date}:\n");
            foreach (var entry in matchingEntries)
            {
                Console.WriteLine($"Prompt: {entry.Prompt}");
                Console.WriteLine($"Response: {entry.Response}\n");
            }
        }
        else
        {
            Console.WriteLine($"No entries found for {date}.");
        }
    }
}

class Program
{
    static void Main()
    {
        Journal journal = new Journal();
        string[] prompts = {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "Is there any favorite place you visited today?"
        };

        Random random = new Random();

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a CSV file");
            Console.WriteLine("4. Load the journal from a CSV file");
            Console.WriteLine("5. Search entries by date");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    string randomPrompt = prompts[random.Next(prompts.Length)];
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    Console.WriteLine($"Prompt ({currentDate}): {randomPrompt}");
                    Console.Write("Enter your response: ");
                    string response = Console.ReadLine();
                    journal.AddEntry(new JournalEntry(randomPrompt, response, currentDate));
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    Console.Write("Enter the filename to save (CSV): ");
                    string saveFilename = Console.ReadLine();
                    journal.SaveToCsvFile(saveFilename);
                    break;
                case "4":
                    Console.Write("Enter the filename to load (CSV): ");
                    string loadFilename = Console.ReadLine();
                    journal.LoadFromCsvFile(loadFilename);
                    break;
                case "5":
                    Console.Write("Enter the date to search (yyyy-MM-dd): ");
                    string searchDate = Console.ReadLine();
                    journal.SearchByDate(searchDate);
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}

// Exceed Requirement 
// I have added an extra option for users to exit if they don't want to continue 
// and also added code to save the entered data as file.