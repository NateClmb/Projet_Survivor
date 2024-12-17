using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

namespace Projet_Survivor;

public class World : Game
{
    //liste des entités présentes à un instant t
    //Le joueur est toujours l'entité à l'indice 0
    private static ArrayList _entities = new ArrayList();
    List<EnemyData> enemyDataList;
    List<Enemy> enemies = new List<Enemy>();

    private GraphicsDeviceManager _graphics;
    public static int WorldWidth;
    public static int WorldHeight;
    private SpriteBatch _spriteBatch;
    public Sprite _shipSprite; // instance de Sprite
    public Sprite _enemySprite; // instance de Sprite
    public static Texture2D defaultProjectileTexture;
    public static Texture2D _enemyTexture;

    private Player player;
    private Random random;

    public World()
    {
        _graphics = new GraphicsDeviceManager(this);
        WorldWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        WorldHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        Content.RootDirectory = "Content/images";
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

    private int enemySpawnTimer = 0; // Compteur pour gérer le spawn des ennemis
    
    private void spawnEnemy(GameTime gameTime)
    {
        // Augmenter le timer avec le temps écoulé
        enemySpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
    
        // Si le timer atteint un certain seuil, spawn un ennemi
        if (enemySpawnTimer >= (6000 - player.level * 500)) // Le spawn devient plus rapide avec le niveau du joueur
        {
            enemySpawnTimer = 0; // Réinitialiser le timer
    
            // Sélectionner un ennemi aléatoire parmi les données
            EnemyData data = enemyDataList[random.Next(enemyDataList.Count)];
    
            int x = random.Next(0, WorldWidth);
            int y = random.Next(0, WorldHeight);
    
            // Déterminer le comportement de l'ennemi en fonction de son type
            Behavior behavior = data.Type == "Corps à corps" ? Behavior.HAND_TO_HAND : Behavior.DISTANCE;
    
            // Créer un nouvel ennemi avec les données récupérées
            Enemy enemy = new Enemy(
                new Rectangle(x, y, 50, 50), // Hitbox de l'ennemi
                new Sprite(_enemyTexture, new Vector2(x, y), 50), // Sprite de l'ennemi
                new Vector2(x, y), // Position initiale de l'ennemi
                new Vector2(data.Speed, data.Speed), // Vitesse de l'ennemi
                data.HP, // Points de vie
                data.Name, // Nom de l'ennemi
                data.XPValue, // Valeur XP
                behavior // Comportement de l'ennemi
            );
    
            // Ajouter l'ennemi à la liste des entités
            _entities.Add(enemy);
        }
    }

    protected override void Initialize()
    {
        // Charger les données des ennemis depuis le fichier XML
        enemyDataList = EnemyLoader.LoadEnemiesFromXML("Content/XML/Enemies.xml");

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
    
        player = new Player(new Rectangle(WorldWidth / 2, WorldHeight / 2, 30, 30), _shipSprite,
            new Vector2(WorldWidth / 2, WorldHeight / 2), new Vector2(),
            100, 1.0);
        _entities.Add(player);
        
    }

    protected override void Update(GameTime gameTime)
    {
        // Vérification de la sortie du jeu
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
    
        // Faire apparaître des ennemis
        spawnEnemy(gameTime);
    
        Entity[] copyEntities = new Entity[_entities.Count];
            _entities.CopyTo(copyEntities);
            foreach (Entity e in copyEntities)
            {
                e.Sprite.Update(gameTime, e);
            }
    
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
    
        // Dessiner le joueur
        Player player = (Player)GetEntities()[0]; // Accéder au joueur dans la liste des entités
        player.Sprite.Draw(_spriteBatch); // Dessiner le joueur
    
        // Dessiner tous les ennemis présents dans la liste _entities
        foreach (Entity e in _entities)
                {
                    e.Sprite.Draw(_spriteBatch);
                }
    
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
