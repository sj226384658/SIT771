using SplashKitSDK;


// Base class for all power-ups.
// Each derived class provides its own effect and appearance.
public abstract class PowerUp
{
    private const double CollisionRadius = 25;
    private const double DrawRadius = 14;

    public double X { get; private set; }
    public double Y { get; private set; }
    public bool Active { get; set; }

    protected PowerUp(double x, double y)
    {
        X = x;
        Y = y;
        Active = true;
    }

    // Each power-up defines its own gameplay effect.
    public abstract void ApplyTo(PlayerRobot player, ref int lives);

    // Each power-up defines its own visual representation.
    protected abstract Color GetColor();
    protected abstract string GetLabel();

    public bool CollidesWith(PlayerRobot player)
    {
        double dx = X - player.X;
        double dy = Y - player.Y;
        double distance = System.Math.Sqrt(dx * dx + dy * dy);

        return distance <= CollisionRadius;
    }

    public void Draw()
    {
        SplashKit.FillCircle(
            GetColor(),
            X,
            Y,
            DrawRadius);

        SplashKit.DrawCircle(
            Color.White,
            X,
            Y,
            DrawRadius);

        SplashKit.DrawText(
            GetLabel(),
            Color.Black,
            "Arial",
            16,
            X - 5,
            Y - 9);
    }
}


// Gives the player one additional life, up to the game limit.
public class ExtraLifePowerUp : PowerUp
{
    private const int MaximumLives = 5;

    public ExtraLifePowerUp(double x, double y)
        : base(x, y)
    {
    }

    public override void ApplyTo(PlayerRobot player, ref int lives)
    {
        if (lives < MaximumLives)
        {
            lives++;
        }
    }

    protected override Color GetColor()
    {
        return Color.RGBColor(80, 220, 100);
    }

    protected override string GetLabel()
    {
        return "L";
    }
}


// Temporarily reduces the delay between player shots.
public class RapidFirePowerUp : PowerUp
{
    public RapidFirePowerUp(double x, double y)
        : base(x, y)
    {
    }

    public override void ApplyTo(PlayerRobot player, ref int lives)
    {
        player.ActivateRapidFire();
    }

    protected override Color GetColor()
    {
        return Color.Yellow;
    }

    protected override string GetLabel()
    {
        return "R";
    }
}


// Temporarily prevents the player from losing a life from collisions.
public class ShieldPowerUp : PowerUp
{
    public ShieldPowerUp(double x, double y)
        : base(x, y)
    {
    }

    public override void ApplyTo(PlayerRobot player, ref int lives)
    {
        player.ActivateShield();
    }

    protected override Color GetColor()
    {
        return Color.Cyan;
    }

    protected override string GetLabel()
    {
        return "S";
    }
}
