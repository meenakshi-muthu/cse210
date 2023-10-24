using System;

class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public string GetFullAddress()
    {
        return $"{Street}, {City}, {State}, {Country}";
    }
}

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

    public virtual string GetFullDetails()
    {
        return $"Title: {EventTitle}\nDescription: {Description}\nDate: {Date.ToShortDateString()}\nTime: {Time}\nAddress: {Address.GetFullAddress()}";
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
        string lectureDetails = base.GetFullDetails();
        return $"{lectureDetails}\nType: Lecture\nSpeaker: {Speaker}\nCapacity: {Capacity}";
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
        string receptionDetails = base.GetFullDetails();
        return $"{receptionDetails}\nType: Reception\nRSVP Email: {RsvpEmail}";
    }
}

class OutdoorGathering : Event
{
    public OutdoorGathering(string eventTitle, string description, DateTime date, string time, Address address)
        : base(eventTitle, description, date, time, address)
    {
    }
}

class Program
{
    static void Main()
    {
        // Create address objects
        Address address1 = new Address
        {
            Street = "123 Main St",
            City = "Anytown",
            State = "CA",
            Country = "USA"
        };

        Address address2 = new Address
        {
            Street = "456 Elm St",
            City = "Another Town",
            State = "NY",
            Country = "USA"
        };

        Address address3 = new Address
        {
            Street = "789 Oak St",
            City = "Yet Another Town",
            State = "TX",
            Country = "USA"
        };

        // Create events using the address objects
        Event lecture = new Lecture("C# Basics", "Introduction to C# programming", DateTime.Now, "6:00 PM", address1, "John Doe", 100);
        Event reception = new Reception("Networking Event", "Casual networking event", DateTime.Now, "7:30 PM", address2, "rsvp@example.com");
        Event outdoorGathering = new OutdoorGathering("Summer Picnic", "Casual picnic in the park", DateTime.Now, "12:00 PM", address3);

        // Display event details
        Console.WriteLine(lecture.GetFullDetails());
        Console.WriteLine();
        Console.WriteLine(reception.GetFullDetails());
        Console.WriteLine();
        Console.WriteLine(outdoorGathering.GetFullDetails());
        Console.WriteLine();
    }
}
