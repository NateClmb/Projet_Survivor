using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace Projet_Survivor;

public class World : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static int WorldWidth;
    public static int WorldHeight;

    //liste des entités présentes à un instant t
    //Le joueur est toujours l'entité à l'indice 0
    private static ArrayList _entities = new ArrayList();

    private static ArrayList _buttons = new ArrayList();

    //List of visual effects to draw on screen such as enemy spawn warning
    private static ArrayList _visualEffects = new ArrayList();

    //List containing spawned enemy time during last second
    private static ArrayList _spawnTimes = new ArrayList();

    public static Texture2D xpBarBackground;
    public static Texture2D xpBarForeground;
    public static Texture2D defaultProjectileTexture;
    private Texture2D enemySpawnWarningTexture;

    private Texture2D backgroundTexture;

    //Lists containing Texture2D used to create sprite sheets for animated sprites
    private ArrayList _enemyTextureList = new ArrayList();
    private ArrayList _playerTextureList = new ArrayList();

    public static Player player;
    public static Random random;
    private static bool isPaused;
    private static bool isGameOver;
    private readonly double SPAWN_WARNING_DURATION = 1500;

    public static bool IsPaused
    {
        get => isPaused;
    }

    public World()
    {
        _graphics = new GraphicsDeviceManager(this);
        WorldWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        WorldHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        Content.RootDirectory = "Content/images";
        isPaused = false;
        isGameOver = false;
        IsMouseVisible = true;
        random = new Random();
    }

    public static void AddEntity(Entity e)
    {
        _entities.Add(e);
    }

    public static ArrayList GetEntities()
    {
        return _entities;
    }

    public static void RemoveEntity(Entity e)
    {
        _entities.Remove(e);
    }

    private ArrayList ConstructSpriteSheet(ArrayList textureList)
    {
        ArrayList spriteSheet = new ArrayList();
        foreach (var t in textureList)
        {
            spriteSheet.Add(new Sprite((Texture2D)t, Vector2.Zero, 100));
        }

        return spriteSheet;
    }

    private void spawnEnemy(GameTime gameTime)
    {
        if ((int)gameTime.TotalGameTime.Ticks % (4 * (40 - player.level)) == 0)
        {
            int x = random.Next(0, WorldWidth);
            int y = random.Next(0, WorldHeight);

            _visualEffects.Add(new Sprite(enemySpawnWarningTexture, new Vector2(x, y), 100));
            _spawnTimes.Add(gameTime.TotalGameTime.TotalMilliseconds);
        }
        double[] copySpawnTimes = new double[_spawnTimes.Count];
        _spawnTimes.CopyTo(copySpawnTimes);
        foreach (double t in copySpawnTimes)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds >= t + SPAWN_WARNING_DURATION)
            {
                _spawnTimes.Remove(t);
                Vector2 pos = ((Sprite) _visualEffects[0])._position;
                _entities.Add(new Enemy(new Rectangle((int)pos.X, (int)pos.Y, 45, 70), ConstructSpriteSheet(_enemyTextureList),
                    pos, new Vector2(1, 1), 3, "eyeShooter", 10, Behavior.HAND_TO_HAND));
                _visualEffects.RemoveAt(0);
            }
        }
    }

    public static void Pause()
    {
        isPaused = true;
    }

    public static void Unpause()
    {
        isPaused = false;
    }

    public static void GameOver()
    {
        isGameOver = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = WorldWidth;
        _graphics.PreferredBackBufferHeight = WorldHeight;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Texture2D playerTexture = Content.Load<Texture2D>("playerStill");
        backgroundTexture = Content.Load<Texture2D>("background");

        _playerTextureList.Add(Content.Load<Texture2D>("playerStill"));
        for (int i = 0; i < 2; i++)
        {
            _playerTextureList.Add(Content.Load<Texture2D>("playerMoving" + i.ToString()));
        }

        for (int i = 0; i < 3; i++)
        {
            _playerTextureList.Add(Content.Load<Texture2D>("playerHit" + i.ToString()));
        }

        for (int i = 0; i < 8; i++)
        {
            _enemyTextureList.Add(Content.Load<Texture2D>("enemyShooter" + i.ToString()));
        }

        defaultProjectileTexture = Content.Load<Texture2D>("standardProjectile");
        xpBarBackground = Content.Load<Texture2D>("xp_bar_background");
        xpBarForeground = Content.Load<Texture2D>("xp_bar_foreground");

        Texture2D healthUpgradeTexture = Content.Load<Texture2D>("healthUpgrade");
        Texture2D speedUpgradeTexture = Content.Load<Texture2D>("speedUpgrade");
        Texture2D damageUpgradeTexture = Content.Load<Texture2D>("damageUpgrade");
        Texture2D attackSpeedUpgradeTexture = Content.Load<Texture2D>("attackSpeedUpgrade");

        Button healthUpgradeButton = new Button(healthUpgradeTexture,
            new Vector2(WorldWidth / 25, WorldHeight / 4));
        healthUpgradeButton.addAction(() => player.increaseMaxHp());
        _buttons.Add(healthUpgradeButton);
        Button speedUpgradeButton =
            new Button(speedUpgradeTexture, new Vector2((5 + 2) * WorldWidth / 25, WorldHeight / 4));
        speedUpgradeButton.addAction(() => player.increaseMaxSpeed());
        _buttons.Add(speedUpgradeButton);
        Button damageUpgradeButton =
            new Button(damageUpgradeTexture, new Vector2((5 * 2 + 3) * WorldWidth / 25, WorldHeight / 4));
        damageUpgradeButton.addAction(() => player.increaseDamage());
        _buttons.Add(damageUpgradeButton);
        Button attackSpeedUpgradeButton = new Button(attackSpeedUpgradeTexture,
            new Vector2((5 * 3 + 4) * WorldWidth / 25, WorldHeight / 4));
        attackSpeedUpgradeButton.addAction(() => player.increaseAttackSpeed());
        _buttons.Add(attackSpeedUpgradeButton);

        enemySpawnWarningTexture = Content.Load<Texture2D>("spawnWarning");

        player = new Player(new Rectangle(WorldWidth / 2, WorldHeight / 2, 60, 50),
            ConstructSpriteSheet(_playerTextureList),
            new Vector2(WorldWidth / 2, WorldHeight / 2), new Vector2(),
            5, 1.0);
        _entities.Add(player);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (isGameOver) ;
        else if (isPaused)
        {
            foreach (Button button in _buttons)
            {
                button.Update(gameTime);
            }
        }
        else
        {
            spawnEnemy(gameTime);
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
        if (isGameOver)
        {
            backgroundTexture = Content.Load<Texture2D>("gameOverScreen");
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, WorldWidth, WorldHeight), null, Color.White, 0.0f,
                Vector2.Zero, SpriteEffects.None, 0f);
        }
        else
        {
            _spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            foreach (Entity e in _entities)
            {
                e.Sprite.Draw(_spriteBatch);
                //Used to show hitboxes
                //_spriteBatch.Draw(Content.Load<Texture2D>("hitboxDebug"), e.Hitbox, Color.White);
            }

            foreach (Sprite s in _visualEffects)
            {
                s.Draw(_spriteBatch);
            }

            if (isPaused)
            {
                foreach (Button button in _buttons)
                {
                    button.Draw(_spriteBatch);
                }
            }

            player.XpBar.Draw(_spriteBatch);
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}