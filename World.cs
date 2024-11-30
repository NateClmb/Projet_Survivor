using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace Projet_Survivor;

public class World : Game
{
    //liste des entités présentes à un instant t
    private ArrayList entities = new ArrayList();

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    protected Sprite _shipSprite; // instance de Sprite
    protected Sprite _enemySprite; // instance de Sprite
    protected Sprite _missileSprite; // instance de Sprite
    protected Enemy _enemy;
    protected Player _ship;
    protected Projectile _missile;

    public World()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content/images";
        IsMouseVisible = true;
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

        Hitbox hitbox = new Hitbox();
        _enemy = new Enemy(hitbox, _enemySprite, new Position(), 100, new Vector2(), "virus", 10, Behavior.HAND_TO_HAND);
        _ship = new Player(hitbox, _shipSprite, new Position(), 100, new Vector2(), 1.0);
        _missile = new Projectile(hitbox, _missileSprite, new Position(), 100, new Vector2(), 10, false, true);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        _shipSprite.Update(gameTime);
        _enemySprite.enemyPattern(gameTime, _shipSprite);
        _missileSprite.Moktar(gameTime, _shipSprite, _missileSprite, _enemySprite);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _shipSprite.Draw(_spriteBatch);
        _enemySprite.Draw(_spriteBatch);
        _missileSprite.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}