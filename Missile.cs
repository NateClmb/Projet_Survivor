using System ;
using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public class Missile{    
    
    protected Vector2 _position ;
    protected Vector2 _speed;
    private int _size = 200;

    
    public Rectangle _Rect { get => new Rectangle (( int ) _position .X , ( int ) _position .Y , _size , _size ) ; }
    

        
}