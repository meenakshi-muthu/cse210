using System;
using System.Collections.Generic;
using System.IO;

public abstract class Goal
{
    public string Name { get; private set; }
    public int Value { get; private set; }
    public bool Completed { get; protected set; }

    public Goal(string name, int value)
    {
        Name = name;
        Value = value;
    }

    protected Goal(string name, int value, bool completed)
    {
        Name = name;
        Value = value;
        Completed = completed;
    }

    public abstract int MarkComplete();
}

public class SimpleGoal : Goal
{
    public SimpleGoal(string name, int value, bool completed = false) : base(name, value, completed) { }

    public override int MarkComplete()
    {
        Completed = true;
        return Value;
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, int value) : base(name, value) { }

    public override int MarkComplete()
    {
        // Eternal goals are never completed
        return Value;
    }
}

public class ChecklistGoal : Goal
{
    private int _completedCount;
    private int _targetCount;
    private int _bonusValue;

    public ChecklistGoal(string name, int value, int targetCount, int bonusValue, bool completed = false)
        : base(name, value, completed)
    {
        _completedCount = 0;
        _targetCount = targetCount;
        _bonusValue = bonusValue;
    }

    public int CompletedCount => _completedCount;
    public int TargetCount => _targetCount;
    public int BonusValue => _bonusValue;

    public override int MarkComplete()
    {
        _completedCount++;
        Completed = (_completedCount == _targetCount);
        return Completed ? Value + _bonusValue : Value;
    }
}

public class ProgressGoal : Goal
{
    private int _progress;
    private int _targetProgress;
    private int _progressValue;

    public ProgressGoal(string name, int value, int targetProgress, int progressValue, bool completed = false)
        : base(name, value, completed)
    {
        _progress = 0;
        _targetProgress = targetProgress;
        _progressValue = progressValue;
    }

    public int Progress => _progress;
    public int TargetProgress => _targetProgress;
    public int ProgressValue => _progressValue;

    public override int MarkComplete()
    {
        _progress += _progressValue;
        Completed = (_progress >= _targetProgress);
        return Completed ? Value : 0;
    }
}

public class NegativeGoal : Goal
{
    public NegativeGoal(string name, int value) : base(name, value) { }

    public override int MarkComplete()
    {
        Completed = true;
        return -Value; // Deduct points for negative goals
    }
}

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
                string goalType = goal.GetType().Name;
                string name = goal.Name;
                int value = goal.Value;
                bool completed = goal.Completed ? true : false;

                if (goal is ChecklistGoal checklistGoal)
                {
                    int targetCount = checklistGoal.TargetCount;
                    int bonusValue = checklistGoal.BonusValue;
                    outputFile.WriteLine($"{goalType}:{name},{value},{completed},{targetCount},{bonusValue}");
                }
                else if (goal is ProgressGoal progressGoal)
                {
                    int targetProgress = progressGoal.TargetProgress;
                    int progressValue = progressGoal.ProgressValue;
                    outputFile.WriteLine($"{goalType}:{name},{value},{completed},{targetProgress},{progressValue}");
                }
                else
                {
                    outputFile.WriteLine($"{goalType}:{name},{value},{completed}");
                }
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
                    if (details.Length >= 3)
                    {
                        string goalType = parts[0];
                        string name = details[0];
                        int value = int.Parse(details[1]);
                        bool completed = bool.Parse(details[2]);

                        switch (goalType.ToLower())
                        {
                            case "simple":
                                _goals.Add(new SimpleGoal(name, value, completed));
                                break;
                            case "eternal":
                                _goals.Add(new EternalGoal(name, value));
                                break;
                            case "checklist":
                                if (details.Length == 5)
                                {
                                    int targetCount = int.Parse(details[3]);
                                    int bonusValue = int.Parse(details[4]);
                                    _goals.Add(new ChecklistGoal(name, value, targetCount, bonusValue, completed));
                                }
                                break;
                            case "progress":
                                if (details.Length == 5)
                                {
                                    int targetProgress = int.Parse(details[3]);
                                    int progressValue = int.Parse(details[4]);
                                    _goals.Add(new ProgressGoal(name, value, targetProgress, progressValue, completed));
                                }
                                break;
                            case "negative":
                                _goals.Add(new NegativeGoal(name, value));
                                break;
                            default:
                                throw new ArgumentException("Invalid goal type");
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

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Create a new goal");
                Console.WriteLine("2. Record an event");
                Console.WriteLine("3. Show goals");
                Console.WriteLine("4. Show score");
                Console.WriteLine("5. Show level");
                Console.WriteLine("6. Save goals to a file");
                Console.WriteLine("7. Load goals from a file");
                Console.WriteLine("8. Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter goal type (simple/eternal/checklist/progress/negative):");
                        string goalType = Console.ReadLine();
                        Console.WriteLine("Enter goal name:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter goal value:");
                        int value = int.Parse(Console.ReadLine());

                        if (goalType == "checklist" || goalType == "progress")
                        {
                            Console.WriteLine("Enter target count (0 if not applicable):");
                            int targetCount = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter bonus value (0 if not applicable):");
                            int bonusValue = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter target progress (0 if not applicable):");
                            int targetProgress = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter progress value (0 if not applicable):");
                            int progressValue = int.Parse(Console.ReadLine());

                            program.CreateGoal(goalType, name, value, targetCount, bonusValue, targetProgress, progressValue);
                        }
                        else
                        {
                            program.CreateGoal(goalType, name, value);
                        }

                        break;
                    case "2":
                        Console.WriteLine("Enter the goal name to record an event:");
                        string eventGoalName = Console.ReadLine();
                        program.RecordEvent(eventGoalName);
                        break;
                    case "3":
                        program.ShowGoals();
                        break;
                    case "4":
                        program.ShowScore();
                        break;
                    case "5":
                        program.ShowLevel();
                        break;
                    case "6":
                        Console.WriteLine("Enter the filename to save goals:");
                        string saveFileName = Console.ReadLine();
                        program.SaveGoals(saveFileName);
                        break;
                    case "7":
                        Console.WriteLine("Enter the filename to load goals:");
                        string loadFileName = Console.ReadLine();
                        program.LoadGoals(loadFileName);
                        break;
                    case "8":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose a valid option.");
                        break;
                }
            }
        }
    }
}
