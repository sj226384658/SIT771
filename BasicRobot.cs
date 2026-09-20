using SplashKitSDK;

// Represents a basic enemy robot.
// It inherits common properties and behaviour from EnemyRobot.
public class BasicRobot : EnemyRobot
{
    // Creates a Basic Robot with its position,
    // size and movement speed.
    public BasicRobot(
        double x,
        double y)
        : base(
            x,
            y,
            16,
            1.5)
    {
    }


    // Handles the event when the Basic Robot is hit by a bullet.
    public override bool TakeHit()
    {
        // Basic Robot is destroyed after one hit.
        Active = false;

        // Return true to indicate that the robot was destroyed.
        return true;
    }


    // Draws the Basic Robot using simple shapes.
    public override void Draw()
    {
        // Draw the main body of the robot.
        SplashKit.FillRectangle(
            Color.RGBColor(235, 90, 90),
            X - 14,
            Y - 14,
            28,
            28);


        // Draw the robot's head.
        SplashKit.FillRectangle(
            Color.RGBColor(255, 130, 130),
            X - 9,
            Y - 23,
            18,
            9);


        // Draw the left eye.
        SplashKit.FillCircle(
            Color.Black,
            X - 4,
            Y - 19,
            2);

        // Draw the right eye.
        SplashKit.FillCircle(
            Color.Black,
            X + 4,
            Y - 19,
            2);


        // Draw the left leg.
        SplashKit.DrawLine(
            Color.White,
            X - 8,
            Y + 14,
            X - 10,
            Y + 21);

        // Draw the right leg.
        SplashKit.DrawLine(
            Color.White,
            X + 8,
            Y + 14,
            X + 10,
            Y + 21);
    }
}