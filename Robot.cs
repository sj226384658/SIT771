using SplashKitSDK;

// Abstract base class for all robots in the game.
// It contains the common properties and behaviours shared by
// the player robot and different types of enemy robots.
public abstract class Robot
{
    // Stores the horizontal position of the robot.
    public double X { get; protected set; }

    // Stores the vertical position of the robot.
    public double Y { get; protected set; }

    // Stores the size of the robot's collision area.
    public double Radius { get; protected set; }

    // Stores the movement speed of the robot.
    public double Speed { get; protected set; }

    // Determines whether the robot is currently active in the game.
    // An inactive robot can be removed or ignored by the game.
    public bool Active { get; set; }

    // Constructor used by derived robot classes to initialise
    // the common robot properties.
    protected Robot(
        double x,
        double y,
        double radius,
        double speed)
    {
        X = x;
        Y = y;
        Radius = radius;
        Speed = speed;

        // Robots are active when they are first created.
        Active = true;
    }

    // Checks whether this robot is colliding with another robot.
    // The distance between the centres of the two robots is
    // compared with the sum of their radii.
    public bool CollidesWith(Robot other)
    {
        // Calculate the horizontal and vertical distance
        // between the two robots.
        double dx = X - other.X;
        double dy = Y - other.Y;

        // Calculate the distance between the two robot centres
        // using the Pythagorean theorem.
        double distance =
            System.Math.Sqrt(
                dx * dx +
                dy * dy);

        // A collision occurs when the distance between the
        // two centres is less than or equal to their combined radii.
        return distance <=
               Radius + other.Radius;
    }

    // Virtual drawing method that provides a default appearance
    // for a robot. Derived classes can override this method
    // to draw their own robot design.
    public virtual void Draw()
    {
        SplashKit.FillCircle(
            Color.Gray,
            X,
            Y,
            Radius);
    }
}