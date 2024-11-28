using System ;
using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public class Enemy : Creature {
    
    private String name;
    private int xpValue { get; init; }
    private Behavior behavior;
    
    public Enemy(Hitbox h, 
        Sprite sprite, 
        Position pos, 
        double hp, 
        double speed, 
        String name, 
        int xpValue, 
        Behavior behavior) : base(h, sprite, pos, hp, speed) 
    {
        this.name=name;
        this.xpValue=xpValue;
        this.behavior=behavior;
    }
    
    //l'ennemi se rapproche en permanence du joueur
    public void move(GameTime gameTime, Sprite player, Sprite enemy){
        if (enemy._position.X > joueur.player.X)
        {
            enemy._position.X -= enemy.speed;
        }
        else
        {
            enemy._position.X += enemy.speed;
        }
        if (enemy._position.Y > player._position.Y)
        {
            enemy._position.Y -= enemy.speed;
        }
        else
        {
            enemy._position.Y += enemy.speed;
        }
    }
    
}