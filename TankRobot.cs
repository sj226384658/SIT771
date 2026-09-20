using SplashKitSDK;

// TankRobot is a specialised enemy robot that inherits
// common properties and behaviours from EnemyRobot.
public class TankRobot : EnemyRobot
{
    private const int MaximumHits = 3;

    // Stores the number of hits the tank can still take
    // before it is destroyed.
    private int _hitsRemaining;

    // Creates a TankRobot at the specified position.
    public TankRobot(
        double x,
        double y)
        : base(
            x,
            y,
            24,
            0.8)
    {
        // Tank requires 3 hits to be destroyed.
        _hitsRemaining = MaximumHits;
    }

    // Overrides the TakeHit method from EnemyRobot.
    // Returns true when the tank is destroyed.
    public override bool TakeHit()
    {
        // Reduce the remaining health by one when
        // the tank is hit by a bullet.
        _hitsRemaining--;

        // Check whether the tank has no hits remaining.
        if (_hitsRemaining <= 0)
        {
            // Deactivate the tank so it can be removed
            // from the active game objects.
            Active = false;

            // Return true to indicate that the tank
            // has been destroyed.
            return true;
        }

        // Return false because the tank is still active
        // and requires more hits to be destroyed.
        return false;
    }

    // Overrides the default Draw method to create
    // a larger tank-style appearance.
    public override void Draw()
    {
        // =================================
        // LARGE TANK BODY
        // =================================

        // Draw the main body of the tank.
        SplashKit.FillRectangle(
            Color.RGBColor(170, 115, 55),
            X - 23,
            Y - 19,
            46,
            38);

        // Draw the raised top section of the tank.
        SplashKit.FillRectangle(
            Color.RGBColor(205, 150, 70),
            X - 16,
            Y - 28,
            32,
            13);

        // Draw the tank cannon.
        SplashKit.FillRectangle(
            Color.RGBColor(235, 190, 90),
            X - 4,
            Y - 40,
            8,
            18);

        // Draw the left track of the tank.
        SplashKit.FillRectangle(
            Color.RGBColor(70, 70, 75),
            X - 28,
            Y - 15,
            6,
            30);

        // Draw the right track of the tank.
        SplashKit.FillRectangle(
            Color.RGBColor(70, 70, 75),
            X + 22,
            Y - 15,
            6,
            30);

        // Display the tank's remaining health.
        // The value decreases each time the tank is hit.
        SplashKit.DrawText(
            "HP " +
            _hitsRemaining +
            "/" + MaximumHits,
            Color.White,
            "Arial",
            13,
            X - 24,
            Y + 27);
    }
}