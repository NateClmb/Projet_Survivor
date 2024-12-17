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
    private static ArrayList _buttons = new ArrayList();

    private GraphicsDeviceManager _graphics;

    public static int WorldWidth;
    public static int WorldHeight;
    private SpriteBatch _spriteBatch;

    public Sprite _playerSprite; // instance de Sprite
    public Sprite _enemySprite; // instance de Sprite
    public static Texture2D xpBarBackground;
    public static Texture2D xpBarForeground;
    public static Texture2D defaultProjectileTexture;

    private ArrayList _enemyTextureList = new ArrayList();

    private Texture2D backgroundTexture;
    public static Player player;
    private Random random;
    private static bool isPaused;

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

    private ArrayList constructSpriteSheet(ArrayList textureList)
    {
        ArrayList spriteSheet = new ArrayList();
        for (int i = 0; i < textureList.Count; i++)
        {
            spriteSheet.Add(new Sprite((Texture2D)textureList[i], Vector2.Zero, 100));
        }

        return spriteSheet;
    }

    private void spawnEnemy(GameTime gameTime)
    {
        if ((int)gameTime.TotalGameTime.Ticks % (6 * (40 - player.level)) == 0)
        {
            int x = random.Next(0, WorldWidth);
            int y = random.Next(0, WorldHeight);
            _entities.Add(new Enemy(new Rectangle(x, y, 45, 70), constructSpriteSheet(_enemyTextureList),
                new Vector2(x, y), new Vector2(1, 1), 3, "eyeShooter", 10, Behavior.HAND_TO_HAND));
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
        Texture2D playerTexture = Content.Load<Texture2D>("playerStill");
        backgroundTexture = Content.Load<Texture2D>("background");
        _playerSprite = new Sprite(playerTexture, new Vector2(150, 150), 100);
        
        for (int i = 0; i < 8; i++)
        {
            _enemyTextureList.Add(Content.Load<Texture2D>("enemyShooter" + i.ToString()));
        }

        defaultProjectileTexture = Content.Load<Texture2D>("missile1");
        xpBarBackground = Content.Load<Texture2D>("xp_bar_background");
        xpBarForeground = Content.Load<Texture2D>("xp_bar_foreground");

        Texture2D healthUpgradeTexture = Content.Load<Texture2D>("healthUpgrade");
        Texture2D speedUpgradeTexture = Content.Load<Texture2D>("speedUpgrade");
        Texture2D damageUpgradeTexture = Content.Load<Texture2D>("damageUpgrade");
        Texture2D attackSpeedUpgradeTexture = Content.Load<Texture2D>("attackspeedUpgrade");

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

        player = new Player(new Rectangle(WorldWidth / 2, WorldHeight / 2, 60, 50), _playerSprite,
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

        _spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

        foreach (Entity e in _entities)
        {
            e.Sprite.Draw(_spriteBatch);
            //Used to show hitboxes
            //_spriteBatch.Draw(Content.Load<Texture2D>("hitboxDebug"), e.Hitbox, Color.White);
        }

        if (isPaused)
        {
            foreach (Button button in _buttons)
            {
                button.Draw(_spriteBatch);
            }
        }

        player.XpBar.Draw(_spriteBatch);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}