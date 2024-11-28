using System ;
using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public abstract class Creature : Entity {
    
    private double hp;
    private double speed { get; set; }
    
    protected Creature(Hitbox h, Sprite sprite, Position pos, double hp, double speed) : base(h, sprite, pos)
    {
        this.hp=hp;
        this.speed=speed;
    }
    
    public void hit(int damages){
        hp-=damages;
    }
    
    
    
}