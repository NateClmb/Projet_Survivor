using System ;
using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public class Sprite {

private Texture2D _texture ;
public Vector2 _position ;
private int _size = 200;
private static readonly int _sizeMin = -200;
private static readonly int _sizeMax = 200;
protected Vector2 _speed;


private Color _color = Color . Pink ;

public Texture2D _Texture { get => _texture ; init => _texture = value ; }
public int _Size { get => _size ; set => _size = value >= _sizeMin ? (value <= _sizeMax ? value:_sizeMax) : _sizeMin ; }
public Rectangle _Rect { get => new Rectangle (( int ) _position .X , ( int ) _position .Y , _size , _size ) ; }

public Sprite ( Texture2D texture , Vector2 position , int size ) {
    _Texture = texture ;
    _position = position ;
    _Size = size ;
    }

    public void Moktar(GameTime gameTime, Sprite player, Sprite missile, Sprite enemy){
        
         float spd_lat = 15f;
         float spd_ver = 15f;
         
         if (Keyboard.GetState().IsKeyDown(Keys.M))
         {
             if (_position.X < -50 || _position.X > 850)
             {
                 _position.X = player._position.X;
                 _position.Y = player._position.Y;
             }
         }
         
         if (_position.X > 0 && _position.X < 800){
             if (_position.X+spd_lat>enemy._position.X || _position.X-spd_lat<enemy._position.X) spd_lat/=2;
             if (_position.Y+spd_ver>enemy._position.Y || _position.Y-spd_ver<enemy._position.Y) spd_ver/=2;
         
         
             if (_position.X > enemy._position.X)
             {
                 _position.X -= spd_lat;
             }
             else
             {
                 _position.X += spd_lat;
             }
             
             if (_position.Y > enemy._position.Y)
             {
                 _position.Y -= spd_ver;
             }
             else
             {
                 _position.Y += spd_ver;
             }
             
             if (enemy._position.X-10f<_position.X && enemy._position.X+10f>_position.X && enemy._position.Y-10f<_position.Y && enemy._position.Y+10f>_position.Y) {
                 _position.X=-100;
                 _position.Y=-100;
             }
         }

     }

          
    public void enemyPattern(GameTime gameTime, Sprite joueur)
    {
        if (_position.X > joueur._position.X)
        {
            _position.X -= 0.5f;
        }
        else
        {
            _position.X += 0.5f;
        }
        if (_position.Y > joueur._position.Y)
        {
            _position.Y -= 0.5f;
        }
        else
        {
            _position.Y += 0.5f;
        }
    }
    

public void Update ( GameTime gameTime ) {
    if ( Keyboard . GetState () . IsKeyDown ( Keys.Right ) ) {
        if (_speed.X<12.0f)
        {_speed.X +=1.1f ;}
        if (_Size < 0)
        {_Size = -_Size;}
    }
    if ( Keyboard . GetState () . IsKeyDown ( Keys.Left ) ) {
        if (_speed.X>-12.0f)
        {_speed.X -=1.1f ;}
        if (_Size > 0)
        {_Size = -_Size;}
    }
    if ( Keyboard . GetState () . IsKeyDown ( Keys.Down ) ) {
        if (_speed.Y<6.0f)
        {_speed.Y +=0.5f ;}
    }
    if ( Keyboard . GetState () . IsKeyDown ( Keys.Up ) ) {
        if (_speed.Y>-6.0f)
        {_speed.Y -=0.5f ;}
    }

    

    _position.X += _speed.X;
    _position.Y += _speed.Y;

    
    if (_position.X <25) _position.X = 25;
    if (_position.X>775) _position.X = 775;
    if (_position.Y < 25) _position.Y = 25;
    if (_position.Y > 450) _position.Y = 450;

    if ( _speed . X > 0) _speed.X -= 0.05f ;
    if ( _speed . X < 0) _speed.X += 0.1f ;
    if ( _speed . Y > 0) _speed.Y -= 0.1f ;
    if ( _speed . Y < 0) _speed.Y += 0.1f ;

    /*
    if (Keyboard.GetState().IsKeyDown(Keys.Left)) if(_Size>0) {_Size = -_Size; }
    if (Keyboard.GetState().IsKeyDown(Keys.Right)) if(_Size<0) {_Size = -_Size; }
    //...
    */
    }

public void Draw ( SpriteBatch spriteBatch ) {
    var origin = new Vector2 ( _texture . Width / 2f , _texture . Height / 2f ) ;
    spriteBatch . Draw ( _texture , // Texture2D ,
        _Rect , // Rectangle destinationRectangle ,
        null , // Nullable < Rectangle > sourceRectangle ,
        _color , // Color ,
        0.0f , // float rotation ,
        origin , // Vector2 origin ,
        SpriteEffects . None , // SpriteEffects effects ,
        0f ) ; // float layerDepth
    }

}