using System ;
using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public class Player : Creature {
    
    private double attackSpd { get; set; }
    private int level;
    private int exp;
    private Weapon weapon { get; init; }
    
    public Player(Hitbox h, 
        Sprite sprite, 
        Position pos, 
        double hp, 
        double speed, 
        double attackSpd) : base(h, sprite, pos, hp, speed)
    {
        this.attackSpd=attackSpd;
    }
    
    public void heal(int heal){
        return;
    }
    
    public void Move ( GameTime gameTime ) {
        //déplacements aux flèches du clavier
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
        
        //limite dans la box (sûrement à retirer plus tard)
        if (_position.X <25) _position.X = 25;
        if (_position.X>775) _position.X = 775;
        if (_position.Y < 25) _position.Y = 25;
        if (_position.Y > 450) _position.Y = 450;
        
        //décélération avec le temps
        if ( _speed . X > 0) _speed.X -= 0.05f ;
        if ( _speed . X < 0) _speed.X += 0.1f ;
        if ( _speed . Y > 0) _speed.Y -= 0.1f ;
        if ( _speed . Y < 0) _speed.Y += 0.1f ;
    }

    private class Weapon()
    {
        
    }
}