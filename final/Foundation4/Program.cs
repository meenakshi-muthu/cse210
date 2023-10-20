using System;
using System.Collections.Generic;

class Activity
{
    public DateTime Date { get; set; }
    public int LengthInMinutes { get; set; }

    public Activity(DateTime date, int lengthInMinutes)
    {
        Date = date;
        LengthInMinutes = lengthInMinutes;
    }

    public virtual double GetDistance()
    {
        return 0; // Base class does not have a distance
    }

    public virtual double GetSpeed()
    {
        return 0; // Base class does not have a speed
    }

    public virtual double GetPace()
    {
        return 0; // Base class does not have a pace
    }

    public virtual string GetSummary()
    {
        return $"{Date.ToShortDateString()} - {GetType().Name} ({LengthInMinutes} min)";
    }
}

class Running : Activity
{
    public double Distance { get; set; }

    public Running(DateTime date, int lengthInMinutes, double distance)
        : base(date, lengthInMinutes)
    {
        Distance = distance;
    }

    public override double GetDistance()
    {
        return Distance;
    }

    public override double GetSpeed()
    {
        return Distance / (LengthInMinutes / 60.0);
    }

    public override double GetPace()
    {
        return LengthInMinutes / Distance;
    }

    public override string GetSummary()
    {
        return $"{base.GetSummary()} - Distance: {Distance} miles, Speed: {GetSpeed()} mph, Pace: {GetPace()} min/mile";
    }
}

class Cycling : Activity
{
    public double Speed { get; set; }

    public Cycling(DateTime date, int lengthInMinutes, double speed)
        : base(date, lengthInMinutes)
    {
        Speed = speed;
    }

    public override double GetSpeed()
    {
        return Speed;
    }

    public override double GetPace()
    {
        return 60.0 / Speed;
    }

    public override string GetSummary()
    {
        return $"{base.GetSummary()} - Speed: {Speed} mph, Pace: {GetP
