using System;
using System.Collections.Generic;

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    private List<Comment> Comments { get; set; } = new List<Comment>();

    public void AddComment(string commenter, string text)
    {
        Comments.Add(new Comment { Commenter = commenter, Text = text });
    }

    public int GetNumberOfComments()
    {
        return Comments.Count;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {LengthInSeconds} seconds");
        Console.WriteLine($"Number of Comments: {GetNumberOfComments()}");

        foreach (var comment in Comments)
        {
            Console.WriteLine($"Comment by {comment.Commenter}: {comment.Text}");
        }

        Console.WriteLine();
    }
}

class Comment
{
    public string Commenter { get; set; }
    public string Text { get; set; }
}

class Program
{
    static void Main()
    {
        List<Video> videos = new List<Video>();

        Video video1 = new Video { Title = "Introduction to C#", Author = "John Doe", LengthInSeconds = 300 };
        video1.AddComment("User1", "Great video!");
        video1.AddComment("User2", "Very informative!");
        videos.Add(video1);

        Video video2 = new Video { Title = "ASP.NET Basics", Author = "Jane Smith", LengthInSeconds = 450 };
        video2.AddComment("User3", "Awesome tutorial!");
        videos.Add(video2);

        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}
