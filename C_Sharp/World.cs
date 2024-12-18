using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

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
    List<EnemyData> enemyDataList;


    public static Texture2D xpBarBackground;
    public static Texture2D xpBarForeground;
    public static Texture2D defaultProjectileTexture;

    private ArrayList _enemyTextureList = new ArrayList();
    private ArrayList _playerTextureList = new ArrayList();

    public static Player player;
    private Texture2D backgroundTexture;
    public static Random random;
    private static bool isPaused;
    private static bool isGameOver;

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

    private ArrayList ConstructSpriteSheet(ArrayList textureList, int size)
    {
        ArrayList spriteSheet = new ArrayList();
        foreach (var t in textureList)
        {
            spriteSheet.Add(new Sprite((Texture2D)t, Vector2.Zero, size));
        }

        return spriteSheet;
    }

    
    private void spawnEnemy(GameTime gameTime)
    {
    
        // Si le timer atteint un certain seuil, spawn un ennemi
        if ((int)gameTime.TotalGameTime.Ticks % (4 * (40 - player.level)) == 0)
        {
    
            // Sélectionner un ennemi aléatoire parmi les données
            EnemyData data = enemyDataList[random.Next(enemyDataList.Count)];
    
            int x = random.Next(0, WorldWidth);
            int y = random.Next(0, WorldHeight);
    
            // Déterminer le comportement de l'ennemi en fonction de son type
            Behavior behavior = data.Type == "Corps à corps" ? Behavior.HAND_TO_HAND : Behavior.DISTANCE;
    
            // Créer un nouvel ennemi avec les données récupérées
            Enemy enemy = new Enemy(
                new Rectangle(x, y, data.Rectangle_X, data.Rectangle_Y), // Hitbox de l'ennemi
                ConstructSpriteSheet(_enemyTextureList, data.Size), // Sprite de l'ennemi
                new Vector2(x, y), // Position initiale de l'ennemi
                new Vector2(data.Speed+player.level/5, data.Speed+player.level/5), // Vitesse de l'ennemi
                data.HP+player.level, // Points de vie
                data.Name, // Nom de l'ennemi
                data.XPValue+data.XPValue*player.level/10, // Valeur XP
                behavior // Comportement de l'ennemi
            );
    
            // Ajouter l'ennemi à la liste des entités
            _entities.Add(enemy);
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
        // Charger les données des ennemis depuis le fichier XML
        enemyDataList = EnemyLoader.LoadEnemiesFromXML("XML/Enemies.xml");

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

        player = new Player(new Rectangle(WorldWidth / 2, WorldHeight / 2, 60, 50),
            ConstructSpriteSheet(_playerTextureList, 100),
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