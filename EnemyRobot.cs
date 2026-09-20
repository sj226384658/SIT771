using System;
using SplashKitSDK;

// Abstract base class for all enemy robots.
// It provides common movement and shooting behaviour
// that can be shared by different enemy robot types.
public abstract class EnemyRobot : Robot
{
    // Shared enemy timing constants.
    private const double FrameDuration = 1.0 / 60.0;
    private const double InitialShootDelay = 1.5;
    private const double MinimumShootInterval = 1.2;
    private const double AdditionalShootInterval = 1.2;
    private const double BulletSpeed = 5.0;

    // Controls the time remaining before the enemy can shoot again.
    protected double ShootCooldown;

    // Random object used to generate different shooting intervals.
    protected Random Random;


    // Creates an enemy robot with its position,
    // radius and movement speed.
    protected EnemyRobot(
        double x,
        double y,
        double radius,
        double speed)
        : base(
            x,
            y,
            radius,
            speed)
    {
        // Reuse the shared random generator instead of creating
        // a separate seeded generator for every enemy.
        Random = Random.Shared;

        // Set a random initial shooting delay.
        ShootCooldown =
            Random.NextDouble() * InitialShootDelay;
    }


    // Requires each enemy robot type to define
    // how it responds when hit by a bullet.
    public abstract bool TakeHit();


    // Updates the enemy robot's movement and shooting behaviour.
    public virtual void Update(
        PlayerRobot player,
        Arena arena,
        System.Collections.Generic.List<Bullet> bullets)
    {
        // Calculate the horizontal distance
        // between the enemy and the player.
        double dx =
            player.X - X;

        // Calculate the vertical distance
        // between the enemy and the player.
        double dy =
            player.Y - Y;

        // Calculate the distance between
        // the enemy and the player.
        double distance =
            Math.Sqrt(
                dx * dx +
                dy * dy);


        // Move towards the player when they are not
        // at exactly the same position.
        if (distance > 0)
        {
            // Normalise the direction so that
            // the enemy moves at a consistent speed.
            dx /= distance;
            dy /= distance;

            // Calculate the enemy's next horizontal position.
            double newX =
                X + dx * Speed;

            // Calculate the enemy's next vertical position.
            double newY =
                Y + dy * Speed;


            // Update the horizontal position only if
            // the enemy remains inside the arena.
            if (arena.Contains(
                newX,
                Y,
                Radius))
            {
                X = newX;
            }


            // Update the vertical position only if
            // the enemy remains inside the arena.
            if (arena.Contains(
                X,
                newY,
                Radius))
            {
                Y = newY;
            }
        }


        // Reduce the shooting cooldown based on
        // the game's approximately 60 frames per second.
        ShootCooldown -= FrameDuration;


        // Check whether the enemy is ready to shoot.
        if (ShootCooldown <= 0)
        {
            // Calculate the direction from the enemy
            // towards the player.
            double bulletDx =
                player.X - X;

            double bulletDy =
                player.Y - Y;


            // Calculate the distance to the player
            // so the shooting direction can be normalised.
            double bulletDistance =
                Math.Sqrt(
                    bulletDx * bulletDx +
                    bulletDy * bulletDy);


            // Create a bullet only when the player
            // is not at the exact same position.
            if (bulletDistance > 0)
            {
                // Normalise the direction vector.
                bulletDx /=
                    bulletDistance;

                bulletDy /=
                    bulletDistance;


                // Create and add an enemy bullet.
                // The final false parameter identifies
                // this as an enemy bullet.
                bullets.Add(
                    new Bullet(
                        X,
                        Y,
                        bulletDx * BulletSpeed,
                        bulletDy * BulletSpeed,
                        false));
            }


            // Reset the cooldown using a random interval
            // between approximately 1.2 and 2.4 seconds.
            ShootCooldown =
                MinimumShootInterval +
                Random.NextDouble() * AdditionalShootInterval;
        }
    }
}