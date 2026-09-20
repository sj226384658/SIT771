using System;
using System.Collections.Generic;
using SplashKitSDK;


// Represents the different screens or states of the game.
public enum GameState
{
    Menu,
    HowToPlay,
    Playing,
    GameOver
}


// Controls the main game logic, game states,
// objects and drawing operations.
public class Game
{
    // Define the size of the game window.
    public const int WindowWidth = 1000;
    public const int WindowHeight = 700;

    // Gameplay constants keep important rules in one place.
    private const int TargetFrameRate = 60;
    private const int InitialLives = 5;
    private const int MaximumEnemies = 3;
    private const int MaximumPowerUps = 2;
    private const double PowerUpSpawnInterval = 8.0;
    private const double EnemySpawnInterval = 1.0;
    private const double MinimumEnemySpawnDistance = 180.0;
    private const int MaximumSpawnAttempts = 20;
    private const int BasicRobotScore = 10;
    private const int TankRobotScore = 30;


    // Stores the game window and current game state.
    private Window _window;
    private GameState _state;


    // Player is created when StartGame() is called.
    private PlayerRobot _player = null!;


    // Represents the area where the game objects can move.
    private Arena _arena;


    // Store all active enemies, bullets and power-ups.
    private List<EnemyRobot> _enemies;
    private List<Bullet> _bullets;
    private List<PowerUp> _powerUps;


    // Stores the current score and number of player lives.
    private int _score;
    private int _lives;


    // Random number generator used for spawning objects
    // and selecting enemy types.
    private Random _random;


    // Timers used to control power-up and enemy spawning.
    private double _powerUpTimer;
    private double _enemySpawnTimer;


    // ==================================================
    // CONSTRUCTOR
    // ==================================================

    // Creates and initialises the game.
    public Game()
    {
        // Create the main game window.
        _window = new Window(
            "Robot Battle",
            WindowWidth,
            WindowHeight);


        // Start the game on the main menu.
        _state = GameState.Menu;


        // Create a random number generator.
        _random = new Random();


        // Create the arena inside the game window.
        _arena = new Arena(
            40,
            100,
            WindowWidth - 80,
            WindowHeight - 150);


        // Create empty collections for game objects.
        _enemies = new List<EnemyRobot>();

        _bullets = new List<Bullet>();

        _powerUps = new List<PowerUp>();
    }


    // ==================================================
    // RUN GAME
    // ==================================================

    // Runs the main game loop.
    public void Run()
    {
        while (!_window.CloseRequested)
        {
            // Process keyboard and mouse events.
            SplashKit.ProcessEvents();


            // Handle user input based on the current game state.
            HandleInput();


            // Update game objects only while the game is being played.
            if (_state == GameState.Playing)
            {
                Update();
            }


            // Draw the current game screen.
            Draw();


            // Refresh the screen at approximately 60 frames per second.
            SplashKit.RefreshScreen(TargetFrameRate);
        }
    }


    // ==================================================
    // HANDLE INPUT
    // ==================================================

    // Handles keyboard and mouse input for each game state.
    private void HandleInput()
    {
        // ========================================
        // MAIN MENU
        // ========================================

        if (_state == GameState.Menu)
        {
            // ------------------------------------
            // Keyboard: Start Game
            // ------------------------------------

            // Press Enter or Space to start the game.
            if (SplashKit.KeyTyped(KeyCode.ReturnKey) ||
                SplashKit.KeyTyped(KeyCode.SpaceKey))
            {
                StartGame();
            }


            // ------------------------------------
            // Keyboard: How To Play
            // ------------------------------------

            // Press H to open the How To Play screen.
            else if (SplashKit.KeyTyped(KeyCode.HKey))
            {
                _state = GameState.HowToPlay;
            }


            // ------------------------------------
            // ESC = Exit entire program
            // ------------------------------------

            // Press Escape to close the game.
            else if (SplashKit.KeyTyped(KeyCode.EscapeKey))
            {
                _window.Close();
            }


            // ------------------------------------
            // Mouse buttons
            // ------------------------------------

            // Check whether the left mouse button was clicked.
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mouse =
                    SplashKit.MousePosition();


                // START button
                if (mouse.X >= 390 &&
                    mouse.X <= 610 &&
                    mouse.Y >= 300 &&
                    mouse.Y <= 355)
                {
                    // Start a new game.
                    StartGame();
                }


                // HOW TO PLAY button
                else if (mouse.X >= 350 &&
                         mouse.X <= 650 &&
                         mouse.Y >= 380 &&
                         mouse.Y <= 435)
                {
                    // Change to the How To Play screen.
                    _state = GameState.HowToPlay;
                }


                // EXIT button
                else if (mouse.X >= 390 &&
                         mouse.X <= 610 &&
                         mouse.Y >= 460 &&
                         mouse.Y <= 515)
                {
                    // Close the game window.
                    _window.Close();
                }
            }
        }


        // ========================================
        // HOW TO PLAY
        // ========================================

        else if (_state == GameState.HowToPlay)
        {
            // ESC or H = return to Main Menu

            // Allow the player to return to the main menu.
            if (SplashKit.KeyTyped(KeyCode.EscapeKey) ||
                SplashKit.KeyTyped(KeyCode.HKey))
            {
                _state = GameState.Menu;
            }


            // Mouse BACK button

            // Check whether the Back button was clicked.
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mouse =
                    SplashKit.MousePosition();


                if (mouse.X >= 350 &&
                    mouse.X <= 650 &&
                    mouse.Y >= 540 &&
                    mouse.Y <= 595)
                {
                    // Return to the main menu.
                    _state = GameState.Menu;
                }
            }
        }


        // ========================================
        // PLAYING
        // ========================================

        else if (_state == GameState.Playing)
        {
            // Pass player input to the PlayerRobot object.
            _player.HandleInput(
                _arena,
                _bullets);
        }


        // ========================================
        // GAME OVER
        // ========================================

        else if (_state == GameState.GameOver)
        {
            // ------------------------------------
            // Keyboard: Retry
            // ------------------------------------

            // Press Enter or Space to restart the game.
            if (SplashKit.KeyTyped(KeyCode.ReturnKey) ||
                SplashKit.KeyTyped(KeyCode.SpaceKey))
            {
                StartGame();
            }


            // ------------------------------------
            // Keyboard: Main Menu
            // ------------------------------------

            // Press M to return to the main menu.
            else if (SplashKit.KeyTyped(KeyCode.MKey))
            {
                _state = GameState.Menu;
            }


            // ------------------------------------
            // Mouse buttons
            // ------------------------------------

            // Check for mouse clicks on Game Over buttons.
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                Point2D mouse =
                    SplashKit.MousePosition();


                // RETRY button

                if (mouse.X >= 320 &&
                    mouse.X <= 680 &&
                    mouse.Y >= 340 &&
                    mouse.Y <= 395)
                {
                    // Start a new game.
                    StartGame();
                }


                // MAIN MENU button

                else if (mouse.X >= 350 &&
                         mouse.X <= 650 &&
                         mouse.Y >= 430 &&
                         mouse.Y <= 485)
                {
                    // Return to the main menu.
                    _state = GameState.Menu;
                }
            }
        }
    }


    // ==================================================
    // START GAME
    // ==================================================

    // Resets the game and creates a new player and enemies.
    private void StartGame()
    {
        // Reset the score and player's lives.
        _score = 0;
        _lives = InitialLives;


        // Reset the spawn timers.
        _powerUpTimer = 0;
        _enemySpawnTimer = 0;


        // Remove all objects from the previous game.
        _bullets.Clear();
        _enemies.Clear();
        _powerUps.Clear();


        // Create player in the centre of Arena.
        _player = new PlayerRobot(
            _arena.X + _arena.Width / 2,
            _arena.Y + _arena.Height / 2);


        // ========================================
        // INITIAL ENEMIES
        // ========================================

        // Basic Robot
        SpawnEnemy(false);

        // Tank Robot
        SpawnEnemy(true);

        // Basic Robot
        SpawnEnemy(false);


        // Change the game state to Playing.
        _state = GameState.Playing;
    }


    // ==================================================
    // UPDATE GAME
    // ==================================================

    // Updates all active game objects and game conditions.
    private void Update()
    {
        // ------------------------------------
        // Player
        // ------------------------------------

        // Update the player's movement and power-up timers.
        _player.Update();


        // ------------------------------------
        // Enemies
        // ------------------------------------

        // Update the movement and shooting behaviour
        // of every enemy robot.
        foreach (EnemyRobot enemy in _enemies)
        {
            enemy.Update(
                _player,
                _arena,
                _bullets);
        }


        // ------------------------------------
        // Bullets
        // ------------------------------------

        // Update the position of every bullet.
        foreach (Bullet bullet in _bullets)
        {
            bullet.Update();
        }


        // ------------------------------------
        // Collision checking
        // ------------------------------------

        // Check collisions between bullets and robots.
        CheckBulletCollisions();

        // Check collisions between the player and enemies.
        CheckPlayerEnemyCollisions();

        // Check collisions between the player and power-ups.
        CheckPowerUpCollisions();


        // ------------------------------------
        // Remove inactive objects
        // ------------------------------------

        // Clean up bullets, enemies and power-ups
        // that are no longer active.
        RemoveInactiveObjects();


        // ========================================
        // POWER-UP SPAWN
        // ========================================

        // Increase the power-up timer based on 60 FPS.
        _powerUpTimer += 1.0 / TargetFrameRate;


        // Spawn a power-up every 8 seconds
        // when fewer than two power-ups exist.
        if (_powerUpTimer >= PowerUpSpawnInterval &&
            _powerUps.Count < MaximumPowerUps)
        {
            SpawnPowerUp();

            _powerUpTimer = 0;
        }


        // ========================================
        // ENEMY SPAWN
        // ========================================

        // Increase the enemy spawn timer.
        _enemySpawnTimer += 1.0 / TargetFrameRate;


        // Maintain up to three enemies at a time.
        if (_enemies.Count < MaximumEnemies &&
            _enemySpawnTimer >= EnemySpawnInterval)
        {
            // Give Tank Robots a lower chance of spawning.
            bool spawnTank =
                _random.Next(0, 3) == 0;


            SpawnEnemy(spawnTank);

            _enemySpawnTimer = 0;
        }


        // ========================================
        // GAME OVER
        // ========================================

        // Change to the Game Over screen when
        // the player has no lives remaining.
        if (_lives <= 0)
        {
            _state = GameState.GameOver;
        }
    }


    // ==================================================
    // BULLET COLLISIONS
    // ==================================================

    // Checks collisions between player/enemy bullets and robots.
    private void CheckBulletCollisions()
    {
        foreach (Bullet bullet in _bullets)
        {
            // Ignore bullets that are already inactive.
            if (!bullet.Active)
            {
                continue;
            }


            // ====================================
            // PLAYER BULLET
            // ====================================

            if (bullet.IsPlayerBullet)
            {
                // Check the bullet against every enemy robot.
                foreach (EnemyRobot enemy in _enemies)
                {
                    // Ignore enemies that have already been destroyed.
                    if (!enemy.Active)
                    {
                        continue;
                    }


                    // Check whether the bullet hits the enemy.
                    if (bullet.CollidesWith(enemy))
                    {
                        // Remove the bullet after a successful hit.
                        bullet.Active = false;


                        // Let the enemy decide whether
                        // the hit destroys it.
                        bool destroyed =
                            enemy.TakeHit();


                        if (destroyed)
                        {
                            // Tank = 30 points
                            // Basic = 10 points

                            // Award different scores depending
                            // on the enemy robot type.
                            if (enemy is TankRobot)
                            {
                                _score += TankRobotScore;
                            }
                            else
                            {
                                _score += BasicRobotScore;
                            }


                            // Spawn replacement enemy.

                            bool newTank;


                            // Use a different probability depending
                            // on which enemy was destroyed.
                            if (enemy is TankRobot)
                            {
                                newTank =
                                    _random.Next(0, 2) == 0;
                            }
                            else
                            {
                                newTank =
                                    _random.Next(0, 4) == 0;
                            }


                            // Replace the destroyed enemy.
                            SpawnEnemy(newTank);
                        }


                        // Stop checking this bullet after a collision.
                        break;
                    }
                }
            }


            // ====================================
            // ENEMY BULLET
            // ====================================

            else
            {
                // Check whether the enemy bullet hits the player.
                if (bullet.CollidesWith(_player))
                {
                    // Remove the bullet after it hits the player.
                    bullet.Active = false;


                    // Shield prevents damage.

                    // Only reduce a life when the shield is inactive.
                    if (!_player.ShieldActive)
                    {
                        _lives--;

                        // Move the player back to the arena centre.
                        _player.Respawn(_arena);
                    }
                }
            }
        }
    }


    // ==================================================
    // PLAYER / ENEMY COLLISIONS
    // ==================================================

    // Checks for direct collisions between the player and enemies.
    private void CheckPlayerEnemyCollisions()
    {
        foreach (EnemyRobot enemy in _enemies)
        {
            // Ignore inactive enemies.
            if (!enemy.Active)
            {
                continue;
            }


            // Check whether the player touches the enemy.
            if (_player.CollidesWith(enemy))
            {
                // Remove the enemy after the collision.
                enemy.Active = false;


                // Shield protects the player from losing a life.
                if (!_player.ShieldActive)
                {
                    _lives--;

                    // Respawn the player after taking damage.
                    _player.Respawn(_arena);
                }
            }
        }
    }
    
    // ==================================================
    // POWER-UP COLLISIONS
    // ==================================================

    // Checks whether the player collects a power-up.
    private void CheckPowerUpCollisions()
    {
        foreach (PowerUp powerUp in _powerUps)
        {
            // Ignore power-ups that are already inactive.
            if (!powerUp.Active)
            {
                continue;
            }


            // Check whether the player has collected the power-up.
            if (powerUp.CollidesWith(_player))
            {
                // Apply the selected power-up effect to the player.
                powerUp.ApplyTo(
                    _player,
                    ref _lives);


                // Remove the collected power-up.
                powerUp.Active = false;
            }
        }
    }


    // ==================================================
    // REMOVE INACTIVE OBJECTS
    // ==================================================

    // Removes objects that are no longer active
    // from their respective collections.
    private void RemoveInactiveObjects()
    {
        _bullets.RemoveAll(
            bullet => !bullet.Active);

        _enemies.RemoveAll(
            enemy => !enemy.Active);

        _powerUps.RemoveAll(
            powerUp => !powerUp.Active);
    }


    // ==================================================
    // SPAWN ENEMY
    // ==================================================

    // Creates a new Basic Robot or Tank Robot
    // at a safe random position inside the arena.
    private void SpawnEnemy(bool tank)
    {
        Point2D position =
            _arena.RandomSafePosition(
                _random,
                tank ? 35 : 25);


        int attempts = 0;


        // Make sure enemies do not spawn
        // too close to the player.

        while (
            attempts < MaximumSpawnAttempts &&
            Distance(
                position.X,
                position.Y,
                _player.X,
                _player.Y) < MinimumEnemySpawnDistance)
        {
            // Generate another random position.
            position =
                _arena.RandomSafePosition(
                    _random,
                    tank ? 35 : 25);

            attempts++;
        }


        // Create the requested enemy type.
        if (tank)
        {
            _enemies.Add(
                new TankRobot(
                    position.X,
                    position.Y));
        }
        else
        {
            _enemies.Add(
                new BasicRobot(
                    position.X,
                    position.Y));
        }
    }


    // ==================================================
    // SPAWN POWER-UP
    // ==================================================

    // Creates a random power-up at a safe position in the arena.
    private void SpawnPowerUp()
    {
        Point2D position =
            _arena.RandomSafePosition(
                _random,
                25);


        // Select a concrete power-up object.
        // The collection stores the base type, so Game can treat every
        // power-up uniformly while polymorphism provides the effect.
        int type = _random.Next(0, 3);

        PowerUp powerUp;

        if (type == 0)
        {
            powerUp = new ExtraLifePowerUp(
                position.X,
                position.Y);
        }
        else if (type == 1)
        {
            powerUp = new RapidFirePowerUp(
                position.X,
                position.Y);
        }
        else
        {
            powerUp = new ShieldPowerUp(
                position.X,
                position.Y);
        }

        _powerUps.Add(powerUp);
    }


    // ==================================================
    // DISTANCE
    // ==================================================

    // Calculates the distance between two points.
    // This is used to control enemy spawn positions.
    private double Distance(
        double x1,
        double y1,
        double x2,
        double y2)
    {
        // Calculate the horizontal difference.
        double dx = x1 - x2;

        // Calculate the vertical difference.
        double dy = y1 - y2;


        // Use the distance formula to calculate
        // the distance between the two points.
        return Math.Sqrt(
            dx * dx +
            dy * dy);
    }


    // ==================================================
    // DRAW
    // ==================================================

    // Draws the correct screen based on the current game state.
    private void Draw()
    {
        // Clear the previous frame.
        SplashKit.ClearScreen(
            Color.Black);


        // Select which screen should be displayed.
        if (_state == GameState.Menu)
        {
            DrawMenu();
        }
        else if (_state == GameState.HowToPlay)
        {
            DrawHowToPlay();
        }
        else if (_state == GameState.Playing)
        {
            DrawGame();
        }
        else
        {
            DrawGameOver();
        }


        // Display the completed frame.
        SplashKit.RefreshScreen();
    }


    // ==================================================
    // MAIN MENU
    // ==================================================

    // Draws the main menu and its controls.
    private void DrawMenu()
    {
        // Draw the menu background.
        SplashKit.FillRectangle(
            Color.RGBColor(20, 25, 35),
            0,
            0,
            WindowWidth,
            WindowHeight);


        // ========================================
        // TITLE
        // ========================================

        // Draw the main game title.
        DrawCenteredText(
            "ROBOT BATTLE",
            Color.White,
            "Arial",
            80,
            WindowWidth / 2,
            120);


        // Draw the game subtitle.
        DrawCenteredText(
            "2D Robot Arena",
            Color.RGBColor(180, 190, 205),
            "Arial",
            24,
            WindowWidth / 2,
            220);


        // ========================================
        // BUTTONS
        // ========================================

        // Draw the Start button.
        DrawButton(
            "START",
            390,
            300,
            220,
            55);


        // Draw the How To Play button.
        DrawButton(
            "HOW TO PLAY (H)",
            350,
            380,
            300,
            55);


        // Draw the Exit button.
        DrawButton(
            "EXIT",
            390,
            460,
            220,
            55);


        // ========================================
        // CONTROLS
        // ========================================

        // Display the keyboard controls at the bottom.
        DrawCenteredText(
            "ENTER / SPACE = Start",
            Color.Gray,
            "Arial",
            18,
            WindowWidth / 2,
            570);


        DrawCenteredText(
            "H = How to Play",
            Color.Gray,
            "Arial",
            18,
            WindowWidth / 2,
            610);


        DrawCenteredText(
            "ESC = Exit the program",
            Color.Gray,
            "Arial",
            18,
            WindowWidth / 2,
            650);
    }


    // ==================================================
    // HOW TO PLAY
    // ==================================================

    // Displays the instructions for playing the game.
    private void DrawHowToPlay()
    {
        // Draw the screen background.
        SplashKit.FillRectangle(
            Color.RGBColor(20, 25, 35),
            0,
            0,
            WindowWidth,
            WindowHeight);


        // Display the How To Play title.
        SplashKit.DrawText(
            "HOW TO PLAY",
            Color.White,
            "Arial",
            40,
            380,
            70);


        // Display movement instructions.
        SplashKit.DrawText(
            "MOVE",
            Color.RGBColor(120, 210, 255),
            "Arial",
            24,
            120,
            160);


        SplashKit.DrawText(
            "Arrow keys or W A S D",
            Color.White,
            "Arial",
            20,
            120,
            200);


        // Display shooting instructions.
        SplashKit.DrawText(
            "SHOOT",
            Color.RGBColor(120, 210, 255),
            "Arial",
            24,
            120,
            250);


        SplashKit.DrawText(
            "SPACE",
            Color.White,
            "Arial",
            20,
            120,
            290);


        // Display information about enemy types.
        SplashKit.DrawText(
            "ENEMIES",
            Color.RGBColor(120, 210, 255),
            "Arial",
            24,
            120,
            340);


        SplashKit.DrawText(
            "Basic Robot = 1 hit",
            Color.White,
            "Arial",
            20,
            120,
            380);


        SplashKit.DrawText(
            "Tank Robot = 3 hits",
            Color.White,
            "Arial",
            20,
            120,
            415);


        // Display information about power-ups.
        SplashKit.DrawText(
            "POWER-UPS",
            Color.RGBColor(120, 210, 255),
            "Arial",
            24,
            560,
            160);


        SplashKit.DrawText(
            "L = +1 life",
            Color.White,
            "Arial",
            20,
            560,
            200);


        SplashKit.DrawText(
            "R = rapid fire",
            Color.White,
            "Arial",
            20,
            560,
            240);


        SplashKit.DrawText(
            "S = temporary shield",
            Color.White,
            "Arial",
            20,
            560,
            280);


        // Display the main gameplay objectives.
        SplashKit.DrawText(
            "Destroy enemies to earn points.",
            Color.White,
            "Arial",
            20,
            560,
            340);


        SplashKit.DrawText(
            "Collect power-ups to improve your abilities.",
            Color.White,
            "Arial",
            20,
            560,
            380);


        SplashKit.DrawText(
            "Avoid enemy bullets and collisions.",
            Color.White,
            "Arial",
            20,
            560,
            420);


        // Draw the button used to return to the main menu.
        DrawButton(
            "BACK (ESC / H)",
            350,
            540,
            300,
            55);
    }


    // ==================================================
    // GAME SCREEN
    // ==================================================

    // Draws the main gameplay screen.
    private void DrawGame()
    {
        // Draw the game background.
        SplashKit.FillRectangle(
            Color.RGBColor(12, 15, 22),
            0,
            0,
            WindowWidth,
            WindowHeight);


        // ========================================
        // HUD
        // ========================================

        // Draw the HUD background at the top of the screen.
        SplashKit.FillRectangle(
            Color.RGBColor(35, 40, 50),
            0,
            0,
            WindowWidth,
            75);


        // Display the Lives label.
        SplashKit.DrawText(
            "LIVES",
            Color.White,
            "Arial",
            20,
            30,
            20);


        // Draw lives.

        // Draw five life indicators.
        for (int i = 0; i < 5; i++)
        {
            Color lifeColor;


            // Active lives are displayed using a bright colour.
            if (i < _lives)
            {
                lifeColor =
                    Color.RGBColor(
                        220,
                        55,
                        55);
            }
            else
            {
                // Lost lives are displayed in grey.
                lifeColor =
                    Color.RGBColor(
                        80,
                        80,
                        80);
            }


            SplashKit.FillRectangle(
                lifeColor,
                100 + i * 32,
                22,
                24,
                28);
        }


        // Score

        // Display the player's current score.
        SplashKit.DrawText(
            "SCORE: " + _score,
            Color.White,
            "Arial",
            22,
            760,
            20);


        // ========================================
        // ARENA
        // ========================================

        // Draw the game arena.
        _arena.Draw();


        // ========================================
        // POWER-UPS
        // ========================================

        // Draw all active power-ups.
        foreach (PowerUp powerUp in _powerUps)
        {
            powerUp.Draw();
        }


        // ========================================
        // BULLETS
        // ========================================

        // Draw all active bullets.
        foreach (Bullet bullet in _bullets)
        {
            bullet.Draw();
        }


        // ========================================
        // ENEMIES
        // ========================================

        // Draw all active enemy robots.
        foreach (EnemyRobot enemy in _enemies)
        {
            enemy.Draw();
        }


        // ========================================
        // PLAYER
        // ========================================

        // Draw the player robot.
        _player.Draw();


        // ========================================
        // CONTROLS
        // ========================================

        // Display the basic gameplay controls.
        SplashKit.DrawText(
            "MOVE: ARROWS / WASD     SHOOT: SPACE",
            Color.RGBColor(170, 175, 185),
            "Arial",
            16,
            270,
            675);


        // Rapid Fire indicator

        // Display the Rapid Fire status when active.
        if (_player.RapidFireActive)
        {
            SplashKit.DrawText(
                "RAPID FIRE",
                Color.Yellow,
                "Arial",
                16,
                45,
                675);
        }


        // Shield indicator

        // Display the Shield status when active.
        if (_player.ShieldActive)
        {
            SplashKit.DrawText(
                "SHIELD",
                Color.Cyan,
                "Arial",
                16,
                125,
                675);
        }
    }


    // ==================================================
    // GAME OVER
    // ==================================================

    // Draws the Game Over screen and final score.
    private void DrawGameOver()
    {
        // Draw the Game Over background.
        SplashKit.FillRectangle(
            Color.RGBColor(20, 25, 35),
            0,
            0,
            WindowWidth,
            WindowHeight);


        // ========================================
        // GAME OVER TITLE
        // ========================================

        // Display the Game Over title.
        DrawCenteredText(
            "GAME OVER",
            Color.RGBColor(235, 70, 70),
            "Arial",
            64,
            WindowWidth / 2,
            140);


        // ========================================
        // FINAL SCORE
        // ========================================

        // Display the player's final score.
        DrawCenteredText(
            "FINAL SCORE: " + _score,
            Color.White,
            "Arial",
            28,
            WindowWidth / 2,
            245);


        // ========================================
        // RETRY BUTTON
        // ========================================

        // Allow the player to start another game.
        DrawButton(
            "RETRY (ENTER / SPACE)",
            320,
            340,
            360,
            55);


        // ========================================
        // MAIN MENU BUTTON
        // ========================================

        // Allow the player to return to the main menu.
        DrawButton(
            "MAIN MENU (M)",
            350,
            430,
            300,
            55);
    }


    // ==================================================
    // DRAW BUTTON
    // ==================================================

    // Draws a reusable button with centred text.
    private void DrawButton(
        string text,
        int x,
        int y,
        int width,
        int height)
    {
        // Button background

        SplashKit.FillRectangle(
            Color.RGBColor(55, 65, 80),
            x,
            y,
            width,
            height);


        // Button border

        SplashKit.DrawRectangle(
            Color.RGBColor(120, 210, 255),
            x,
            y,
            width,
            height);


        // Button text is automatically centred.

        DrawCenteredText(
            text,
            Color.White,
            "Arial",
            20,
            x + width / 2.0,
            y + 16);
    }


    // ==================================================
    // DRAW CENTRED TEXT
    // ==================================================

    // Calculates the text width and positions the text
    // so that it is centred around the specified X coordinate.
    private void DrawCenteredText(
        string text,
        Color color,
        string fontName,
        int fontSize,
        double centerX,
        double y)
    {
        // Calculate the width of the text.
        int textWidth =
            SplashKit.TextWidth(
                text,
                fontName,
                fontSize);


        // Calculate the left position required
        // to centre the text.
        double x =
            centerX -
            textWidth / 2.0;


        // Draw the text at the calculated position.
        SplashKit.DrawText(
            text,
            color,
            fontName,
            fontSize,
            x,
            y);
    }
}