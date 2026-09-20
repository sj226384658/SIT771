using SplashKitSDK;

// Represents a bullet fired by either the player or an enemy robot.
public class Bullet
{
    // Stores the horizontal position of the bullet.
    public double X
    {
        get;
        private set;
    }

    // Stores the vertical position of the bullet.
    public double Y
    {
        get;
        private set;
    }

    // Stores the horizontal movement of the bullet.
    private double _dx;

    // Stores the vertical movement of the bullet.
    private double _dy;

    // Indicates whether the bullet was fired by the player.
    // This is used to distinguish player bullets from enemy bullets.
    public bool IsPlayerBullet
    {
        get;
        private set;
    }

    // Indicates whether the bullet is still active in the game.
    public bool Active
    {
        get;
        set;
    }


    // Creates a bullet with its position, movement direction
    // and owner type.
    public Bullet(
        double x,
        double y,
        double dx,
        double dy,
        bool isPlayerBullet)
    {
        // Set the initial position of the bullet.
        X = x;
        Y = y;

        // Set the bullet's horizontal and vertical movement.
        _dx = dx;
        _dy = dy;

        // Store whether the bullet belongs to the player.
        IsPlayerBullet =
            isPlayerBullet;

        // New bullets are active when they are created.
        Active = true;
    }


    // Updates the bullet's position.
    public void Update()
    {
        // Move the bullet according to its direction.
        X += _dx;
        Y += _dy;

        // Remove bullets outside
        // the game window.
        if (
            X < 0 ||
            X > Game.WindowWidth ||
            Y < 0 ||
            Y > Game.WindowHeight)
        {
            Active = false;
        }
    }


    // Checks whether the bullet has collided with a robot.
    public bool CollidesWith(
        Robot robot)
    {
        // Calculate the horizontal distance
        // between the bullet and the robot.
        double dx =
            X - robot.X;

        // Calculate the vertical distance
        // between the bullet and the robot.
        double dy =
            Y - robot.Y;

        // Calculate the distance between
        // the bullet and the robot.
        double distance =
            System.Math.Sqrt(
                dx * dx +
                dy * dy);

        // A collision occurs when the distance
        // is smaller than the combined radius
        // of the robot and the bullet.
        return distance <=
               robot.Radius + 5;
    }


    // Draws the bullet on the screen.
    public void Draw()
    {
        // Store the colour used to draw the bullet.
        Color color;

        // Player bullets are drawn in yellow.
        if (IsPlayerBullet)
        {
            color = Color.Yellow;
        }
        else
        {
            // Enemy bullets are drawn in a different colour
            // so that they can be distinguished from player bullets.
            color =
                Color.RGBColor(
                    255,
                    120,
                    120);
        }

        // Draw the bullet as a small circle.
        SplashKit.FillCircle(
            color,
            X,
            Y,
            5);
    }
}