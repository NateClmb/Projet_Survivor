using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Enemy : Entity
{
    private String name;
    private int xpValue { get; init; }
    private Behavior behavior;
    private readonly Player player = (Player) World.GetEntities()[0];

    public Enemy(Rectangle hitbox,
        Sprite sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        String name,
        int xpValue,
        Behavior behavior) : base(hitbox, sprite, pos, speed, hp)
    {
        this.name = name;
        this.xpValue = xpValue;
        this.behavior = behavior;
    }

    //l'ennemi se rapproche en permanence du joueur
     
    public override void Move(GameTime gameTime)
    {
        if (Position.X > player.Position.X)
        {
            Position.X -= Speed.X;
        }
        else
        {
            Position.X += Speed.X;
        }

        if (Position.Y > player.Position.Y)
        {
            Position.Y -= Speed.Y;
        }
        else
        {
            Position.Y += Speed.Y;
        }
        
        setHitboxPosition();
        testOverlapseWithEnemy();
        die();
    }
    
    private void testOverlapseWithEnemy()
    {
        foreach (Entity e in World.GetEntities())
        {
            if (e.Hitbox.Intersects(Hitbox) && e.GetType() == typeof(Enemy))
            {
                Vector2 stepAside = new Vector2(Position.X - e.Position.X, Position.Y - e.Position.Y);
                Position.X += 0.05f * stepAside.X;
                Position.Y += 0.05f * stepAside.Y;
            }
        }
    }
    
    private void die()
    {
        if (_hp <= 0)
            World.RemoveEntity(this);
    }
}