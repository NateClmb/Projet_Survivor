using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Projet_Survivor.C_Sharp;

public class World : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static int WorldWidth;
    public static int WorldHeight;

    //List of entities existing at an instant
    //The player is always the entity at index 0
    private static ArrayList _entities = new();

    //List containing buttons showed when player levels up
    private static ArrayList _buttons = new();

    //List of visual effects to draw on screen such as enemy spawn warning
    private static ArrayList _visualEffects = new();

    //List containing spawned enemy time during last second
    private static ArrayList _spawnTimes = new();

    //Textures used in World
    private Texture2D _enemySpawnWarningTexture;
    private Texture2D _backgroundTexture;
    private SpriteFont _font;

    //Textures used elsewhere
    public static Texture2D XpBarBackground;
    public static Texture2D XpBarForeground;
    public static Texture2D DefaultProjectileTexture;
    public static Texture2D EnemyProjectileTexture;

    //Lists containing Texture2D used to create sprite sheets for animated sprites
    private readonly ArrayList _enemyHandToHandTextureList = new();
    private readonly ArrayList _enemyDistanceTextureList = new();
    private readonly ArrayList _playerTextureList = new();

    public static Player Player;

    //Every class shares the same random generator
    public static Random Random;

    //Game state, default being running
    private static bool _isPaused;
    private static bool _isGameOver;
    public static bool IsPaused => _isPaused;

    //Game stats
    private static double _gameOverTime;
    private static int _nbKilled;

    //Represents time passed when the game was running
    private static double _inGameTime;

    //Difficulty level influence enemise spawn rate and their stats
    private int _difficultyLevel;

    //Duration of a glyph drawn to warn player about incoming enemy spawn
    private readonly double SPAWN_WARNING_DURATION = 1500;

    public World()
    {
        _graphics = new GraphicsDeviceManager(this);
        WorldWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        WorldHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        Content.RootDirectory = "Content/images";
        _isPaused = false;
        _isGameOver = false;
        IsMouseVisible = true;
        _difficultyLevel = 0;
        Random = new Random();
    }

    public static void AddEntity(Entity e)
    {
        _entities.Add(e);
    }

    //Return an array that contains a copy of _entities
    public static Entity[] GetEntities()
    {
        Entity[] entities = new Entity[_entities.Count];
        _entities.CopyTo(entities);
        return entities;
    }

    //Remove an Entity from _entities and update the kill counter accordingly
    public static void RemoveEntity(Entity e)
    {
        _entities.Remove(e);
        if (e is Enemy)
            _nbKilled++;
    }

    //Creates an ArrayList of Sprite from an ArrayList of Texture2D. All Sprite have a size of 100
    private ArrayList ConstructSpriteSheet(ArrayList textureList)
    {
        ArrayList spriteSheet = new ArrayList();
        foreach (var t in textureList)
        {
            spriteSheet.Add(new Sprite((Texture2D)t, Vector2.Zero, 100));
        }

        return spriteSheet;
    }

    //Manage all things related to enemy spawn. From warning glyphes to enemy type choice
    private void SpawnEnemy(GameTime gameTime)
    {
        if ((int)gameTime.TotalGameTime.Ticks % (Math.Max(30, 300 - _difficultyLevel * 10)) == 0)
        {
            int x = Random.Next(0, WorldWidth);
            int y = Random.Next(0, WorldHeight);

            _visualEffects.Add(new Sprite(_enemySpawnWarningTexture, new Vector2(x, y), 100));
            _spawnTimes.Add(gameTime.TotalGameTime.TotalMilliseconds);
        }

        double[] copySpawnTimes = new double[_spawnTimes.Count];
        _spawnTimes.CopyTo(copySpawnTimes);
        foreach (double t in copySpawnTimes)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds >= t + SPAWN_WARNING_DURATION)
            {
                _spawnTimes.Remove(t);
                Vector2 pos = ((Sprite)_visualEffects[0]).Position;
                var chooseEnemy = Random.Next() % 3 == 0
                    ? _entities.Add(new DistanceEnemy(new Rectangle((int)pos.X, (int)pos.Y, 45, 70),
                        ConstructSpriteSheet(_enemyDistanceTextureList), pos, new Vector2(2, 2), 3, "eyeShooter", 15,
                        1))
                    : _entities.Add(new HandToHandEnemy(new Rectangle((int)pos.X, (int)pos.Y, 70, 45),
                        ConstructSpriteSheet(_enemyHandToHandTextureList), pos, new Vector2(3, 3), 3, "eyeSprinter", 10,
                        1));

                _visualEffects.RemoveAt(0);
            }
        }
    }

    //Pause the game
    public static void Pause()
    {
        _isPaused = true;
    }

    //Unpause the game
    public static void Unpause()
    {
        _isPaused = false;
    }

    //Change game state to game over and measured the time 
    public static void GameOver()
    {
        _isGameOver = true;
        _gameOverTime = _inGameTime;
    }

    //Start a new game
    private void Restart()
    {
        Random = new Random();
        _isPaused = false;
        _isGameOver = false;
        _gameOverTime = 0;
        _nbKilled = 0;
        _inGameTime = 0;
        _difficultyLevel = 0;
        _entities.Clear();
        _buttons.Clear();
        _visualEffects.Clear();
        _spawnTimes.Clear();
        _enemyHandToHandTextureList.Clear();
        _enemyDistanceTextureList.Clear();
        _playerTextureList.Clear();
        LoadContent();
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = WorldWidth;
        _graphics.PreferredBackBufferHeight = WorldHeight;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    //Load all Texture2D used in the game and create a Player
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("font");

        _backgroundTexture = Content.Load<Texture2D>("background");

        _playerTextureList.Add(Content.Load<Texture2D>("playerStill"));
        for (int i = 0; i < 2; i++)
        {
            _playerTextureList.Add(Content.Load<Texture2D>("playerMoving" + i.ToString()));
        }

        for (int i = 0; i < 3; i++)
        {
            _playerTextureList.Add(Content.Load<Texture2D>("playerHit" + i.ToString()));
        }

        for (int i = 0; i < 3; i++)
        {
            _enemyHandToHandTextureList.Add(Content.Load<Texture2D>("enemyDog" + i.ToString()));
        }

        for (int i = 0; i < 8; i++)
        {
            _enemyDistanceTextureList.Add(Content.Load<Texture2D>("enemyShooter" + i.ToString()));
        }

        DefaultProjectileTexture = Content.Load<Texture2D>("standardProjectile");
        EnemyProjectileTexture = Content.Load<Texture2D>("enemyProjectile");
        XpBarBackground = Content.Load<Texture2D>("xp_bar_background");
        XpBarForeground = Content.Load<Texture2D>("xp_bar_foreground");

        Texture2D healthUpgradeTexture = Content.Load<Texture2D>("healthUpgrade");
        Texture2D speedUpgradeTexture = Content.Load<Texture2D>("speedUpgrade");
        Texture2D damageUpgradeTexture = Content.Load<Texture2D>("damageUpgrade");
        Texture2D attackSpeedUpgradeTexture = Content.Load<Texture2D>("attackSpeedUpgrade");

        Button healthUpgradeButton = new Button(healthUpgradeTexture,
            new Vector2(WorldWidth / 25, WorldHeight / 4));
        healthUpgradeButton.AddAction(() => Player.IncreaseMaxHp());
        _buttons.Add(healthUpgradeButton);
        Button speedUpgradeButton =
            new Button(speedUpgradeTexture, new Vector2((5 + 2) * WorldWidth / 25, WorldHeight / 4));
        speedUpgradeButton.AddAction(() => Player.IncreaseMaxSpeed());
        _buttons.Add(speedUpgradeButton);
        Button damageUpgradeButton =
            new Button(damageUpgradeTexture, new Vector2((5 * 2 + 3) * WorldWidth / 25, WorldHeight / 4));
        damageUpgradeButton.AddAction(() => Player.IncreaseDamage());
        _buttons.Add(damageUpgradeButton);
        Button attackSpeedUpgradeButton = new Button(attackSpeedUpgradeTexture,
            new Vector2((5 * 3 + 4) * WorldWidth / 25, WorldHeight / 4));
        attackSpeedUpgradeButton.AddAction(() => Player.IncreaseAttackSpeed());
        _buttons.Add(attackSpeedUpgradeButton);

        _enemySpawnWarningTexture = Content.Load<Texture2D>("spawnWarning");

        Player = new Player(new Rectangle(WorldWidth / 2, WorldHeight / 2, 60, 50),
            ConstructSpriteSheet(_playerTextureList),
            new Vector2(WorldWidth / 2, WorldHeight / 2), new Vector2(),
            5, 1.0);
        _entities.Add(Player);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (_isGameOver)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Restart();
            }
        }
        else if (_isPaused)
        {
            foreach (Button button in _buttons)
            {
                button.Update(gameTime);
            }
        }
        else
        {
            _inGameTime++;
            if (_inGameTime % 600 == 0)
            {
                _difficultyLevel++;
            }

            SpawnEnemy(gameTime);
            Entity[] copyEntities = new Entity[_entities.Count];
            _entities.CopyTo(copyEntities);
            foreach (Entity e in copyEntities)
            {
                e.Sprite.Update(gameTime, e);
            }

            base.Update(gameTime);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        if (_isGameOver)
        {
            _backgroundTexture = Content.Load<Texture2D>("gameOverScreen");
            _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, WorldWidth, WorldHeight), null, Color.White, 0.0f,
                Vector2.Zero, SpriteEffects.None, 0f);
            String endGameStats = "You killed " + _nbKilled.ToString() + " ennemies !\n";
            endGameStats += "You survived " + Math.Round(_gameOverTime / 60) + " seconds !\n";
            _spriteBatch.DrawString(_font, endGameStats, new Vector2(WorldWidth / 6, 2 * WorldHeight / 3), Color.White);
            _spriteBatch.DrawString(_font, "Press Enter to restart",
                new Vector2(WorldWidth / 2 - 160, 7 * WorldHeight / 8), Color.White);
        }
        else
        {
            _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            _spriteBatch.DrawString(_font, Player.GetHp(), new Vector2(50, 100), Color.White);
            foreach (Entity e in _entities)
            {
                e.Draw(_spriteBatch);
                //Used to show hitboxes
                //_spriteBatch.Draw(Content.Load<Texture2D>("hitboxDebug"), e.Hitbox, Color.White);
            }

            foreach (Sprite s in _visualEffects)
            {
                s.Draw(_spriteBatch);
            }

            if (_isPaused)
            {
                foreach (Button button in _buttons)
                {
                    button.Draw(_spriteBatch);
                }
            }

            Player.XpBar.Draw(_spriteBatch);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}