using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Notice that the list is a list of "Shape" objects. That means
        // you can put any Shape objects in there, and also, any object where
        // the class inherits from Shape
        List<Shape> shapes = new List<Shape>();

        Square s1 = new Square("Red", 3);
        shapes.Add(s1);

        Rectangle s2 = new Rectangle("Blue", 4, 5);
        shapes.Add(s2);

        Circle s3 = new Circle("Green", 6);
        shapes.Add(s3);

        foreach (Shape s in shapes)
        {
            // Notice that all shapes have a GetColor method from the base class
            string color = s.GetColor();

            // Notice that all shapes have a GetArea method, but the behavior is
            // different for each type of shape
            double area = s.GetArea();

            Console.WriteLine($"The {color} shape has an area of {area}.");
        }
    }
}

public class Shape
{
    private string _color;

    public string Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public Shape(string color)
    {
        Color = color;
    }

    public virtual double GetArea()
    {
        return 0; // Default implementation for the base class
    }

    public string GetColor()
    {
        return Color;
    }
}

public class Square : Shape
{
    private double _side;

    public double Side
    {
        get { return _side; }
        set { _side = value; }
    }

    public Square(string color, double side) : base(color)
    {
        Side = side;
    }

    public override double GetArea()
    {
        return _side * _side;
    }
}

public class Rectangle : Shape
{
    private double _length;
    private double _width;

    public double Length
    {
        get { return _length; }
        set { _length = value; }
    }

    public double Width
    {
        get { return _width; }
        set { _width = value; }
    }

    public Rectangle(string color, double length, double width) : base(color)
    {
        Length = length;
        Width = width;
    }

    public override double GetArea()
    {
        return _length * _width;
    }
}

public class Circle : Shape
{
    private double _radius;

    public double Radius
    {
        get { return _radius; }
        set { _radius = value; }
    }

    public Circle(string color, double radius) : base(color)
    {
        Radius = radius;
    }

    public override double GetArea()
    {
        return Math.PI * _radius * _radius;
    }
}
