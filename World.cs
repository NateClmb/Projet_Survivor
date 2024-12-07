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
    private SpriteBatch _spriteBatch;
    public Sprite _shipSprite; // instance de Sprite
    public Sprite _enemySprite; // instance de Sprite
    public Sprite _missileSprite; // instance de Sprite

    public World()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content/images";
        IsMouseVisible = true;
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

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Texture2D shipTexture = Content.Load<Texture2D>("ship2");
        _shipSprite = new Sprite(shipTexture, new Vector2(150, 150), 60);
        Texture2D enemyTexture = Content.Load<Texture2D>("virus_tiles");
        _enemySprite = new Sprite(enemyTexture, new Vector2(250, 250), 200);
        Texture2D missileTexture = Content.Load<Texture2D>("missile1");
        _missileSprite = new Sprite(missileTexture, new Vector2(-100, -100), 30);

        Rectangle hitbox = new Rectangle(0, 0, 100, 100);
        Player player = new Player(hitbox, _shipSprite, new Vector2(), new Vector2(), 100, 1.0);
        _entities.Add(player);
        Enemy enemy = new Enemy(hitbox, _enemySprite, new Vector2(), new Vector2(), 100, "virus", 10, Behavior.HAND_TO_HAND);
        _entities.Add(enemy);
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        foreach (Entity e in _entities)
        {
                e.Sprite.Update(gameTime, e);
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        foreach (Entity e in _entities)
        {
            e.Sprite.Draw(_spriteBatch);
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}