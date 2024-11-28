using System ;
using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public class Projectile : Creature {
    
    private double damage;
    private bool smart;
    private bool friendly;
    
    public Projectile(Hitbox hitbox, 
        Sprite sprite, 
        Position pos, 
        double hp, 
        double spd, 
        double dmg, 
        bool smart, 
        bool friendly) : base(hitbox, sprite, pos, hp, spd)
    {
        this.damage=dmg;
        this.smart=smart;
        this.friendly=friendly;
    }
}