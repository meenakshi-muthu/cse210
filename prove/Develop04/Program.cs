using System;
using System.Threading;

abstract class MindfulnessActivity
{
    public string Name { get; }
    public string Description { get; }

    protected MindfulnessActivity(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void StartActivity(int duration)
    {
        ShowStartingMessage();
        Thread.Sleep(3000); // Pause for preparation

        PerformActivity(duration);

        ShowEndingMessage(duration);
    }

    protected abstract void PerformActivity(int duration);

    private void ShowStartingMessage()
    {
        Console.WriteLine($"\n=== {Name} Activity ===");
        Console.WriteLine(Description);
        Console.WriteLine("Get ready to begin...\n");
    }

    private void ShowEndingMessage(int duration)
    {
        Console.WriteLine($"\n=== Activity Completed ===");
        Console.WriteLine($"Congratulations! You've completed the {Name} activity.");
        Console.WriteLine($"Duration: {duration} seconds\n");
        Thread.Sleep(3000); // Pause before finishing
    }

    // Animation during pauses
    protected void DisplaySpinner(int seconds)
    {
        Console.Write("Processing ");
        for (int i = 0; i < seconds; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000); // Pause for 1 second
            Console.Write("\b \b");
        }
        Console.WriteLine();
    }
}

class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity() : base("Breathing", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    protected override void PerformActivity(int duration)
    {
        int animationDuration = 5; // Adjust as needed
        int animationFrames = 20; // Adjust as needed

        Console.WriteLine("Get ready to begin...\n");

        Console.Write("Enter the duration in seconds: ");
        if (int.TryParse(Console.ReadLine(), out int userDuration) && userDuration > 0)
        {
            int totalDuration = 0;

            while (totalDuration < userDuration)
            {
                Console.WriteLine("Breathe in...");
                PerformCountdown(4); // Pause for 4 seconds

                // Perform the dynamic text animation
                PerformBreathingAnimation("Breathe in...", animationDuration, animationFrames);

                Console.WriteLine();

                totalDuration += 4; // Increment the total duration

                if (totalDuration >= userDuration)
                    break;

                Console.WriteLine("Breathe out...");
                PerformCountdown(4); // Pause for 4 seconds

                // Perform the dynamic text animation
                PerformBreathingAnimation("Breathe out...", animationDuration, animationFrames);

                Console.WriteLine();

                totalDuration += 4; // Increment the total duration
            }

            Console.WriteLine("Breathing activity completed.");
        }
        else
        {
            Console.WriteLine("Invalid duration. Please enter a valid number greater than 0.");
        }
    }

    private void PerformBreathingAnimation(string message, int duration, int frames)
    {
        for (int j = 0; j < frames; j++)
        {
            int delay = (int)(Math.Pow(j, 2) / frames) + 1;
            Console.Write(new string(' ', j) + message + new string(' ', frames - j) + "\r");
            Thread.Sleep(delay);
        }
    }

    private void PerformCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.WriteLine($"{i}...");
            Thread.Sleep(1000); // Pause for 1 second
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
        }
    }
}

class ReflectionActivity : MindfulnessActivity
{
    private static readonly string[] Prompts = {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private static readonly string[] Questions = {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity() : base("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
    }

    protected override void PerformActivity(int duration)
    {
        Console.WriteLine("Get ready to begin...\n");

        Console.Write("Enter the duration in seconds: ");
        if (int.TryParse(Console.ReadLine(), out int userDuration) && userDuration > 0)
        {
            int totalDuration = 0;
            Random random = new Random();

            while (totalDuration < userDuration)
            {
                string randomPrompt = Prompts[random.Next(Prompts.Length)]; // Select a random prompt
                Console.WriteLine(randomPrompt);

                foreach (string question in Questions)
                {
                    Console.WriteLine(question);
                    DisplaySpinner(3); // Display animation for 3 seconds
                    totalDuration += 3; // Increment the total duration

                    // Pause for 2 seconds between questions
                    Thread.Sleep(2000);
                    totalDuration += 2;
                }
            }

            Console.WriteLine("Reflection activity completed.");
        }
        else
        {
            Console.WriteLine("Invalid duration. Please enter a valid number greater than 0.");
        }
    }

}

class ListingActivity : MindfulnessActivity
{
    private static readonly string[] Prompts = {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes"
    };

    public ListingActivity() : base("Listing", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
    }

    protected override void PerformActivity(int duration)
    {
        Console.WriteLine("Get ready to begin...\n");

        Console.WriteLine("Press Enter when ready to start listing...");
        Console.ReadLine(); // Wait for user input

        DateTime startTime = DateTime.Now;
        Random random = new Random();
        int itemsListed = 0;

        while ((DateTime.Now - startTime).TotalSeconds < duration)
        {
            string randomPrompt = Prompts[random.Next(Prompts.Length)]; // Select a random prompt
            Console.WriteLine(randomPrompt); // Display the selected prompt

            Console.Write("Enter an item (or press Enter to finish): ");
            string item = Console.ReadLine();

            if (string.IsNullOrEmpty(item))
                break;

            itemsListed++;
            DisplaySpinner(1); // Display animation for 1 second
        }

        Console.WriteLine($"\nNumber of items listed: {itemsListed}");
        Console.WriteLine("Listing activity completed.");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Mindfulness App\n");

        while (true)
        {
            Console.WriteLine("Select an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("0. Exit");

            Console.Write("Enter your choice: ");
            int choice;

            if (int.TryParse(Console.ReadLine(), out choice))
            {
                if (choice == 0)
                {
                    Console.WriteLine("Exiting the Mindfulness App. Goodbye!");
                    break;
                }

                if (choice >= 1 && choice <= 3)
                {
                    Console.Write("Enter the duration in seconds: ");
                    if (int.TryParse(Console.ReadLine(), out int duration))
                    {
                        MindfulnessActivity activity = null;

                        switch (choice)
                        {
                            case 1:
                                activity = new BreathingActivity();
                                break;
                            case 2:
                                activity = new ReflectionActivity();
                                break;
                            case 3:
                                activity = new ListingActivity();
                                break;
                        }

                        activity?.StartActivity(duration);
                    }
                    else
                    {
                        Console.WriteLine("Invalid duration. Please enter a valid number.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
}

// Exceed Requirement 
// Adding more meaningful animations for the breathing
// Includes a dynamic text animation loop for both "Breathe in..." and "Breathe out...". 
