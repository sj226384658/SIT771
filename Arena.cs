using System;
using SplashKitSDK;

// Represents the game arena where the player,
// enemies and power-ups are placed.
public class Arena
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private const int GridSize = 50;

    public Arena(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    // Checks whether an object is completely inside the arena.
    public bool Contains(double x, double y, double radius)
    {
        return
            x - radius >= X &&
            x + radius <= X + Width &&
            y - radius >= Y &&
            y + radius <= Y + Height;
    }

    // Generates a random position where the object fits inside the arena.
    public Point2D RandomSafePosition(Random random, double radius)
    {
        double x = X + radius + random.NextDouble() * (Width - radius * 2);
        double y = Y + radius + random.NextDouble() * (Height - radius * 2);

        return new Point2D
        {
            X = x,
            Y = y
        };
    }

    public void Draw()
    {
        SplashKit.FillRectangle(
            Color.RGBColor(28, 32, 42),
            X, Y, Width, Height);

        SplashKit.DrawRectangle(
            Color.RGBColor(90, 100, 115),
            X, Y, Width, Height);

        for (int x = X + GridSize; x < X + Width; x += GridSize)
        {
            SplashKit.DrawLine(
                Color.RGBColor(40, 45, 55),
                x, Y, x, Y + Height);
        }

        for (int y = Y + GridSize; y < Y + Height; y += GridSize)
        {
            SplashKit.DrawLine(
                Color.RGBColor(40, 45, 55),
                X, y, X + Width, y);
        }
    }
}
