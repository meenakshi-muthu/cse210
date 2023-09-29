using System;
using System.Collections.Generic;
using System.IO;
public class Word
{
    public string Text { get; private set; }
    public bool IsHidden { get; private set; }
    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }
    public void Hide()
    {
        IsHidden = true;
    }
}
public class ScriptureReference
{
    public string Book { get; private set; }
    public int Chapter { get; private set; }
    public int? StartVerse { get; private set; }
    public int? EndVerse { get; private set; }
    public ScriptureReference(string book, int chapter, int? startVerse = null, int? endVerse = null)
    {
        Book = book;
        Chapter = chapter;
        StartVerse = startVerse;
        EndVerse = endVerse;
    }
    public override string ToString()
    {
        if (StartVerse.HasValue && EndVerse.HasValue)
        {
            return $"{Book} {Chapter}:{StartVerse}-{EndVerse}";
        }
        else if (StartVerse.HasValue)
        {
            return $"{Book} {Chapter}:{StartVerse}";
        }
        else
        {
            return $"{Book} {Chapter}";
        }
    }
    public string GetFormattedReference()
    {
        return ToString();
    }
}
public class Scripture
{
    private List<Word> words;
    private int hiddenWordCount;
    private ScriptureReference reference;
    public Scripture(ScriptureReference reference, string text)
    {
        this.reference = reference;
        words = new List<Word>();
        string[] textWords = text.Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string wordText in textWords)
        {
            words.Add(new Word(wordText));
        }
        hiddenWordCount = 0;
    }
    public void DisplayWithHints()
    {
        Console.Clear();
        Console.WriteLine($"Scripture: {reference.GetFormattedReference()}\n");
        foreach (Word word in words)
        {
            if (word.IsHidden)
                Console.Write("____ ");
            else
                Console.Write($"{word.Text} ");
        }
        Console.WriteLine("\n\nPress Enter to reveal more words or type 'quit' to exit.");
    }
    public bool HideRandomWord()
    {
        Random random = new Random();
        int index = random.Next(words.Count);

        if (!words[index].IsHidden)
        {
            words[index].Hide();
            hiddenWordCount++;
        }

        return hiddenWordCount < words.Count;
    }
    public bool IsComplete()
    {
        return hiddenWordCount == words.Count;
    }
}
public class ScriptureLibrary
{
    private List<Scripture> scriptures;

    public ScriptureLibrary()
    {
        scriptures = new List<Scripture>();
    }
    public void AddScripture(Scripture scripture)
    {
        scriptures.Add(scripture);
    }
    public Scripture GetRandomScripture()
    {
        Random random = new Random();
        int index = random.Next(scriptures.Count);
        return scriptures[index];
    }
    public void LoadScripturesFromFile(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    string referenceText = parts[0].Trim();
                    string scriptureText = parts[1].Trim();
                    ScriptureReference reference = ParseScriptureReference(referenceText);
                    Scripture scripture = new Scripture(reference, scriptureText);
                    AddScripture(scripture);
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"Error reading file: {e.Message}");
        }
    }
    private ScriptureReference ParseScriptureReference(string referenceText)
    {
        string[] parts = referenceText.Split(' ');
        if (parts.Length == 2)
        {
            string[] chapterVerse = parts[1].Split(':');
            if (chapterVerse.Length == 2)
            {
                int chapter = int.Parse(chapterVerse[0]);
                int verse = int.Parse(chapterVerse[1]);
                return new ScriptureReference(parts[0], chapter, verse);
            }
        }
        return new ScriptureReference("Invalid", 0, 0);
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Scripture Memorization Program!");
        // Create a scripture library
        ScriptureLibrary library = new ScriptureLibrary();
        // Load scriptures from a file (you can replace "Scriptures.txt" with your file path)
        library.LoadScripturesFromFile("Scriptures.txt");
        // Get a random scripture from the library
        Scripture randomScripture = library.GetRandomScripture();
        while (!randomScripture.IsComplete())
        {
            randomScripture.DisplayWithHints();

            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "quit")
            {
                Console.WriteLine("Program ended.");
                return;
            }
            randomScripture.HideRandomWord();
        }
        Console.WriteLine("You've successfully memorized the scripture!");
        Console.WriteLine("Program ending.");
    }
}


// Exceed Requirement 
//library of scriptures and loads them from a file