using System;
using System.Collections.Generic;
using System.IO;

// Base class for goals
public abstract class Goal
{
    private string _name;
    private int _value;

    public string Name { get => _name; private set => _name = value; }
    public int Value { get => _value; private set => _value = value; }
    public bool Completed { get; protected set; }

    public Goal(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public abstract int MarkComplete();
}

// Derived class for simple goals
public class SimpleGoal : Goal
{
    public SimpleGoal(string name, int value) : base(name, value) { }

    public override int MarkComplete()
    {
        Completed = true;
        return Value;
    }
}

// Derived class for eternal goals
public class EternalGoal : Goal
{
    public EternalGoal(string name, int value) : base(name, value) { }

    public override int MarkComplete()
    {
        // Eternal goals are never completed
        return Value;
    }
}

// Derived class for checklist goals
public class ChecklistGoal : Goal
{
    private int _completedCount;
    private int _targetCount;
    private int _bonusValue;

    public ChecklistGoal(string name, int value, int targetCount, int bonusValue)
        : base(name, value)
    {
        _completedCount = 0;
        _targetCount = targetCount;
        _bonusValue = bonusValue;
    }

    public int CompletedCount => _completedCount;
    public int TargetCount => _targetCount;

    public override int MarkComplete()
    {
        _completedCount++;
        Completed = (_completedCount == _targetCount);
        return Completed ? Value + _bonusValue : Value;
    }
}

// Derived class for progress goals
public class ProgressGoal : Goal
{
    private int _progress;
    private int _targetProgress;
    private int _progressValue;

    public ProgressGoal(string name, int value, int targetProgress, int progressValue)
        : base(name, value)
    {
        _progress = 0;
        _targetProgress = targetProgress;
        _progressValue = progressValue;
    }

    public int Progress => _progress;
    public int TargetProgress => _targetProgress;

    public override int MarkComplete()
    {
        _progress += _progressValue;
        Completed = (_progress >= _targetProgress);
        return Completed ? Value : 0;
    }
}

// Derived class for negative goals
public class NegativeGoal : Goal
{
    public NegativeGoal(string name, int value) : base(name, value) { }

    public override int MarkComplete()
    {
        Completed = true;
        return -Value; // Deduct points for negative goals
    }
}

// Class to manage goals, score, and player level
public class EternalQuestProgram
{
    private List<Goal> _goals;
    private int _score;
    private int _xp;
    private int _level;

    public EternalQuestProgram()
    {
        _goals = new List<Goal>();
        _score = 0;
        _xp = 0;
        _level = 1;
    }

    public void CreateGoal(string goalType, string name, int value, int targetCount = 0, int bonusValue = 0, int targetProgress = 0, int progressValue = 0)
    {
        Goal goal;
        switch (goalType.ToLower())
        {
            case "simple":
                goal = new SimpleGoal(name, value);
                break;
            case "eternal":
                goal = new EternalGoal(name, value);
                break;
            case "checklist":
                goal = new ChecklistGoal(name, value, targetCount, bonusValue);
                break;
            case "progress":
                goal = new ProgressGoal(name, value, targetProgress, progressValue);
                break;
            case "negative":
                goal = new NegativeGoal(name, value);
                break;
            default:
                throw new ArgumentException("Invalid goal type");
        }
        _goals.Add(goal);
    }

    public void RecordEvent(string goalName)
    {
        foreach (var goal in _goals)
        {
            if (goal.Name == goalName && !goal.Completed)
            {
                _score += goal.MarkComplete();
                _xp += goal.Value;

                // Check for level up
                if (_xp >= 100 * _level)
                {
                    LevelUp();
                }

                break;
            }
        }
    }

    private void LevelUp()
    {
        _level++;
        Console.WriteLine($"Congratulations! You leveled up to Level {_level}!");
        // Additional rewards or bonuses for leveling up can be added here
    }

    public void ShowGoals()
    {
        Console.WriteLine("Current Goals:");
        foreach (var goal in _goals)
        {
            Console.WriteLine($"{goal.Name} - [{(goal.Completed ? "X" : " ")}]");
            if (goal is ChecklistGoal checklistGoal)
            {
                Console.WriteLine($"  Completed {checklistGoal.CompletedCount}/{checklistGoal.TargetCount} times");
            }
            else if (goal is ProgressGoal progressGoal)
            {
                Console.WriteLine($"  Progress: {progressGoal.Progress}/{progressGoal.TargetProgress}");
            }
        }
        Console.WriteLine();
    }

    public void ShowScore()
    {
        Console.WriteLine($"Current Score: {_score} points");
    }

    public void ShowLevel()
    {
        Console.WriteLine($"Current Level: {_level}");
    }

    // Dummy methods for saving and loading goals
    public void SaveGoals(string fileName)
    {
        using (StreamWriter outputFile = new StreamWriter(fileName))
        {
            foreach (var goal in _goals)
            {
                outputFile.WriteLine($"{goal.GetType().Name}:{goal.Name},{goal.Value},{goal.Completed}");
            }
        }
    }

    public void LoadGoals(string fileName)
    {
        _goals.Clear();
        using (StreamReader inputFile = new StreamReader(fileName))
        {
            string line;
            while ((line = inputFile.ReadLine()) != null)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string[] details = parts[1].Split(',');
                    if (details.Length == 3)
                    {
                        string goalType = parts[0];
                        string name = details[0];
                        int value = int.Parse(details[1]);
                        bool completed = bool.Parse(details[2]);

                        switch (goalType.ToLower())
                        {
                            case "simple":
                                _goals.Add(new SimpleGoal(name, value) { Completed = completed });
                                break;
                            case "eternal":
                                _goals.Add(new EternalGoal(name, value) { Completed = completed });
                                break;
                            case "checklist":
                                int targetCount = int.Parse(details[3]);
                                int bonusValue = int.Parse(details[4]);
                                _goals.Add(new ChecklistGoal(name, value, targetCount, bonusValue) { Completed = completed });
                                break;
                            case "progress":
                                int targetProgress = int.Parse(details[3]);
                                int progressValue = int.Parse(details[4]);
                                _goals.Add(new ProgressGoal(name, value, targetProgress, progressValue) { Completed = completed });
                                break;
                            case "negative":
                                _goals.Add(new NegativeGoal(name, value) { Completed = completed });
                                break;
                            default:
                                throw new ArgumentException("Invalid goal type");
                        }
                    }
                }
            }
        }
    }
}

class Program
{
    static void Main()
    {
        var program = new EternalQuestProgram();

        // Create goals
        program.CreateGoal("simple", "Read Scriptures", 100);
        program.CreateGoal("simple", "Run a Marathon", 1000);
        program.CreateGoal("eternal", "Attend Temple", 50);
        program.CreateGoal("checklist", "Exercise Daily", 20, 5, 100);
        program.CreateGoal("progress", "Run a Marathon", 500, 1000, 50);
        program.CreateGoal("negative", "Eat Junk Food", 50);

        // Display goals
        program.ShowGoals();

        // Record events
        program.RecordEvent("Read Scriptures");
        program.RecordEvent("Run a Marathon");
        program.RecordEvent("Attend Temple");
        program.RecordEvent("Exercise Daily");
        program.RecordEvent("Exercise Daily");
        program.RecordEvent("Exercise Daily");
        program.RecordEvent("Exercise Daily");
        program.RecordEvent("Exercise Daily");

        // Display updated goals, score, and level
        program.ShowGoals();
        program.ShowScore();
        program.ShowLevel();

        // Save goals to a file
        program.SaveGoals("goals.txt");

        // Clear goals and load from the file
        program.LoadGoals("goals.txt");

        // Display loaded goals
        program.ShowGoals();
    }
}




// Exceed Req:
// additional ProgressGoal and NegativeGoal classes added.