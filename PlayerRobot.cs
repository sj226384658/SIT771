using SplashKitSDK;


// Represents the player-controlled robot.
// It inherits common robot properties and behaviour from Robot.
public class PlayerRobot : Robot
{
    // Player timing constants make the gameplay rules explicit.
    private const double NormalShootCooldown = 0.28;
    private const double RapidFireShootCooldown = 0.12;
    private const double PowerUpDuration = 8.0;
    private const double FrameDuration = 1.0 / 60.0;

    // Controls the time between player shots.
    private double _shootCooldown;

    // Stores the remaining duration of the Rapid Fire power-up.
    private double _rapidFireTime;

    // Stores the remaining duration of the Shield power-up.
    private double _shieldTime;


    // Returns true when the Rapid Fire power-up is active.
    public bool RapidFireActive
    {
        get
        {
            return _rapidFireTime > 0;
        }
    }


    // Returns true when the Shield power-up is active.
    public bool ShieldActive
    {
        get
        {
            return _shieldTime > 0;
        }
    }


    // Creates the player robot with its starting position.
    public PlayerRobot(
        double x,
        double y)
        : base(
            x,
            y,
            18,
            4.2)
    {
        // Initialise the shooting cooldown.
        _shootCooldown = 0;

        // Rapid Fire is inactive when the game starts.
        _rapidFireTime = 0;

        // Shield is inactive when the game starts.
        _shieldTime = 0;
    }


    // Handles keyboard input for player movement and shooting.
    public void HandleInput(
        Arena arena,
        System.Collections.Generic.List<Bullet> bullets)
    {
        // Store the horizontal and vertical movement.
        double dx = 0;
        double dy = 0;


        // Left
        if (
            SplashKit.KeyDown(KeyCode.LeftKey) ||
            SplashKit.KeyDown(KeyCode.AKey))
        {
            // Move the player to the left.
            dx -= Speed;
        }


        // Right
        if (
            SplashKit.KeyDown(KeyCode.RightKey) ||
            SplashKit.KeyDown(KeyCode.DKey))
        {
            // Move the player to the right.
            dx += Speed;
        }


        // Up
        if (
            SplashKit.KeyDown(KeyCode.UpKey) ||
            SplashKit.KeyDown(KeyCode.WKey))
        {
            // Move the player upwards.
            dy -= Speed;
        }


        // Down
        if (
            SplashKit.KeyDown(KeyCode.DownKey) ||
            SplashKit.KeyDown(KeyCode.SKey))
        {
            // Move the player downwards.
            dy += Speed;
        }


        // Calculate the player's next position.
        double newX = X + dx;
        double newY = Y + dy;


        // Keep player inside Arena.

        // Only update the horizontal position when
        // the player remains inside the arena.
        if (arena.Contains(
            newX,
            Y,
            Radius))
        {
            X = newX;
        }


        // Only update the vertical position when
        // the player remains inside the arena.
        if (arena.Contains(
            X,
            newY,
            Radius))
        {
            Y = newY;
        }


        // Shoot

        // Create a bullet when Space is held down
        // and the shooting cooldown has finished.
        if (
            SplashKit.KeyDown(KeyCode.SpaceKey) &&
            _shootCooldown <= 0)
        {
            // Create a player bullet moving upwards.
            bullets.Add(
                new Bullet(
                    X,
                    Y - Radius - 5,
                    0,
                    -8,
                    true));


            // Rapid Fire reduces the delay between shots.
            if (RapidFireActive)
            {
                _shootCooldown = RapidFireShootCooldown;
            }
            else
            {
                // Normal shooting uses a longer cooldown.
                _shootCooldown = NormalShootCooldown;
            }
        }
    }


    // Updates the player's shooting cooldown
    // and power-up timers.
    public void Update()
    {
        // Reduce the shooting cooldown over time.
        _shootCooldown -= 1.0 / 60.0;


        // Prevent the cooldown from becoming negative.
        if (_shootCooldown < 0)
        {
            _shootCooldown = 0;
        }


        // Reduce the remaining Rapid Fire duration.
        if (_rapidFireTime > 0)
        {
            _rapidFireTime -= FrameDuration;
        }


        // Reduce the remaining Shield duration.
        if (_shieldTime > 0)
        {
            _shieldTime -= FrameDuration;
        }
    }


    // Activates the Rapid Fire power-up for 8 seconds.
    public void ActivateRapidFire()
    {
        _rapidFireTime = PowerUpDuration;
    }


    // Activates the Shield power-up for 8 seconds.
    public void ActivateShield()
    {
        _shieldTime = PowerUpDuration;
    }


    // Moves the player back to the centre of the arena.
    // This is used after the player loses a life.
    public void Respawn(Arena arena)
    {
        X =
            arena.X +
            arena.Width / 2;

        Y =
            arena.Y +
            arena.Height / 2;
    }


    // Draws the player robot and its visual effects.
    public override void Draw()
    {
        // Shield

        // Draw a shield around the player when
        // the Shield power-up is active.
        if (ShieldActive)
        {
            SplashKit.DrawCircle(
                Color.Cyan,
                X,
                Y,
                Radius + 8);
        }


        // Body

        // Draw the main body of the player robot.
        SplashKit.FillRectangle(
            Color.RGBColor(70, 190, 220),
            X - 16,
            Y - 14,
            32,
            28);


        // Head

        // Draw the robot's head.
        SplashKit.FillRectangle(
            Color.RGBColor(100, 220, 240),
            X - 11,
            Y - 24,
            22,
            12);


        // Eyes

        // Draw the left eye.
        SplashKit.FillCircle(
            Color.Black,
            X - 5,
            Y - 19,
            2);


        // Draw the right eye.
        SplashKit.FillCircle(
            Color.Black,
            X + 5,
            Y - 19,
            2);


        // Legs

        // Draw the left leg.
        SplashKit.DrawLine(
            Color.White,
            X - 8,
            Y + 14,
            X - 10,
            Y + 22);


        // Draw the right leg.
        SplashKit.DrawLine(
            Color.White,
            X + 8,
            Y + 14,
            X + 10,
            Y + 22);


        // Gun

        // Draw the player's gun above the robot.
        SplashKit.FillRectangle(
            Color.RGBColor(220, 225, 230),
            X - 3,
            Y - 34,
            6,
            12);
    }
}