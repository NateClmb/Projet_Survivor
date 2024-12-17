using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace Projet_Survivor;

public class World : Game
{
    //liste des entités présentes à un instant t
    //Le joueur est toujours l'entité à l'indice 0
    private static ArrayList _entities = new ArrayList();

    private GraphicsDeviceManager _graphics;

    public static int WorldWidth;
    public static int WorldHeight;
    private SpriteBatch _spriteBatch;

    public Sprite _shipSprite; // instance de Sprite
    public Sprite _enemySprite; // instance de Sprite
    public static Texture2D xpBarBackground;
    public static Texture2D xpBarForeground;
    public static Texture2D healthUpgradeTexture;
    public static Texture2D defaultProjectileTexture;
    public static Texture2D _enemyTexture;

    private Button healthUpgradeButton;
    private Player player;
    private Random random;
    private static bool isPaused;
    
    public static bool IsPaused { get => isPaused; }

    public World()
    {
        _graphics = new GraphicsDeviceManager(this);
        WorldWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        WorldHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        Content.RootDirectory = "Content/images";
        isPaused = false;
        IsMouseVisible = true;
        random = new Random();
    }

    public static void AddEntity(Entity e)
    {
        _entities.Add(e);
    }

    public static void ShowLevelUpButtons()
    {
    }

    public static ArrayList GetEntities()
    {
        return _entities;
    }

    public static void RemoveEntity(Entity e)
    {
        _entities.Remove(e);
    }

    private void spawnEnemy(GameTime gameTime)
    {
        if ((int)gameTime.TotalGameTime.Ticks % (6 * (40 - player.level)) == 0)
        {
            int x = random.Next(0, WorldWidth);
            int y = random.Next(0, WorldHeight);
            _entities.Add(new Enemy(new Rectangle(x, y, 64, 64),
                new Sprite(_enemyTexture, new Vector2(500, 500), 100), new Vector2(x, y), new Vector2(1, 1), 30,
                "virus", 10, Behavior.HAND_TO_HAND));
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
        Texture2D shipTexture = Content.Load<Texture2D>("ship2");
        _shipSprite = new Sprite(shipTexture, new Vector2(150, 150), 60);
        _enemyTexture = Content.Load<Texture2D>("virus1");
        defaultProjectileTexture = Content.Load<Texture2D>("missile1");
        healthUpgradeTexture = Content.Load<Texture2D>("healthUpgrade");
        xpBarBackground = Content.Load<Texture2D>("xp_bar_background");
        xpBarForeground = Content.Load<Texture2D>("xp_bar_foreground");

        healthUpgradeButton = new Button(healthUpgradeTexture, new Vector2(150, 150));
        healthUpgradeButton.addAction(() => player.increaseMaxHp());

        player = new Player(new Rectangle(WorldWidth / 2, WorldHeight / 2, 30, 30), _shipSprite,
            new Vector2(WorldWidth / 2, WorldHeight / 2), new Vector2(),
            5, 1.0);
        _entities.Add(player);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (isPaused)
        {
            healthUpgradeButton.Update(gameTime);
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
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        if (isPaused)
        {
            healthUpgradeButton.Draw(_spriteBatch);
        }

        foreach (Entity e in _entities)
        {
            e.Sprite.Draw(_spriteBatch);
        }

        player.XpBar.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}