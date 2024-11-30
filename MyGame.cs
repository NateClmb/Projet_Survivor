using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public class MyGame : Game {
    private GraphicsDeviceManager _graphics ;
    private SpriteBatch _spriteBatch ;
    protected Sprite _ship ; // instance de Sprite
    protected Sprite _enemy ; // instance de Sprite
    protected Sprite _missile ; // instance de Sprite

    public MyGame () {
        _graphics = new GraphicsDeviceManager ( this ) ;
        Content.RootDirectory = "Content/images";
        IsMouseVisible = true ;
    }

    protected override void Initialize () {
        base.Initialize();
    }

    protected override void LoadContent () {
        _spriteBatch = new SpriteBatch ( GraphicsDevice ) ;
        Texture2D shipTexture = Content . Load < Texture2D >("ship1") ;
        _ship = new Sprite ( shipTexture , new Vector2 (150 , 150), 60 ) ;
        Texture2D enemyTexture = Content . Load < Texture2D >("virus1") ;
        _enemy = new Sprite ( enemyTexture , new Vector2 (250 , 250), 40 ) ;
        Texture2D missileTexture = Content . Load < Texture2D >("missile1") ;
        _missile = new Sprite ( missileTexture , new Vector2 (-100 , -100), 30 ) ;
    }

    protected override void Update ( GameTime gameTime ) {
        if ( GamePad.GetState ( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed ||
             Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit () ;
        _ship . Update ( gameTime ) ;
        _enemy.enemyPattern(gameTime, _ship);
        _missile.Moktar(gameTime, _ship, _missile, _enemy);
        base . Update ( gameTime ) ;
    }

    protected override void Draw ( GameTime gameTime ) {
        GraphicsDevice . Clear ( Color . CornflowerBlue ) ;
        _spriteBatch . Begin ( samplerState : SamplerState . PointClamp ) ;
        _ship . Draw ( _spriteBatch ) ;
        _enemy.Draw(_spriteBatch);
        _missile.Draw(_spriteBatch);
        _spriteBatch . End () ;
        base . Draw ( gameTime ) ;
    }
}