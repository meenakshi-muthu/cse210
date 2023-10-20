using System;

class Event
{
    public string EventTitle { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; }
    public Address Address { get; set; }

    public Event(string eventTitle, string description, DateTime date, string time, Address address)
    {
        EventTitle = eventTitle;
        Description = description;
        Date = date;
        Time = time;
        Address = address;
    }

    public virtual string GetStandardDetails()
    {
        return $"Title: {EventTitle}\nDescription: {Description}\nDate: {Date.ToShortDateString()}\nTime: {Time}\nAddress: {Address.GetFullAddress()}";
    }

    public virtual string GetFullDetails()
    {
        return GetStandardDetails();
    }

    public virtual string GetShortDescription()
    {
        return $"{GetType().Name}: {EventTitle}\nDate: {Date.ToShortDateString()}";
    }
}

class Lecture : Event
{
    public string Speaker { get; set; }
    public int Capacity { get; set; }

    public Lecture(string eventTitle, string description, DateTime date, string time, Address address, string speaker, int capacity)
        : base(eventTitle, description, date, time, address)
    {
        Speaker = speaker;
        Capacity = capacity;
    }

    public override string GetFullDetails()
    {
        return $"{base.GetFullDetails()}\nType: Lecture\nSpeaker: {Speaker}\nCapacity: {Capacity}";
    }
}

class Reception : Event
{
    public string RsvpEmail { get; set; }

    public Reception(string eventTitle, string description, DateTime date, string time, Address address, string rsvpEmail)
        : base(eventTitle, description, date, time, address)
    {
        RsvpEmail = rsvpEmail;
    }

    public override string GetFullDetails()
    {
        return $"{base.GetFullDetails()}\nType: Reception\nRSVP Email: {RsvpEmail}";
    }
}

class OutdoorGathering : Event
{
    public OutdoorGathering(string eventTitle, string description, DateTime date, string time, Address address)
        : base(eventTitle, description, date, time, address)
    {
    }

    public override string GetFullDetails()
    {
        return $"{base.GetFullDetails()}\nType: Outdoor Gathering";
    }
}

class Program
{
    static void Main()
    {
        Event lecture = new Lecture("C# Basics", "Introduction to C# programming", DateTime.Now, "6:00 PM", new Address(), "John Doe", 100);
        Event reception = new Reception("Networking Event", "Casual networking event", DateTime.Now, "7:30 PM", new Address(), "rsvp@example.com");
        Event outdoorGathering = new OutdoorGathering("Summer Picnic", "Casual picnic in the park", DateTime.Now, "12:00 PM", new Address());

        Console.WriteLine(lecture.GetFullDetails());
        Console.WriteLine();
        Console.WriteLine(reception.GetFullDetails());
        Console.WriteLine();
        Console.WriteLine(outdoorGathering.GetFullDetails());
        Console.WriteLine();
    }
}
