using System;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Projectile : Entity
{
    private int damage;
    private Vector2 direction;
    private bool smart;
    private bool friendly;

    public Projectile(Rectangle hitbox,
        Sprite sprite,
        Vector2 pos,
        Vector2 spd,
        Vector2 direction,
        int piercePotential,
        int dmg,
        bool smart,
        bool friendly) : base(hitbox, sprite, pos, spd, piercePotential)
    {
        this.damage = dmg;
        this.direction = direction;
        this.smart = smart;
        this.friendly = friendly;
    }

    public override void Move(GameTime gameTime)
    {
        Position += direction * Speed;
        setHitboxPosition();
        hitTest();
        autoDestruct();
    }

    private void hitTest()
    {
        foreach (Entity e in World.GetEntities())
        {
            Console.WriteLine(e.Hitbox + ", " + this.Hitbox);
            if (e.Hitbox.Intersects(Hitbox) && (this.friendly && e.GetType() == typeof(Enemy) ||
                                                !friendly && e.GetType() == typeof(Player))) 
            {
                Console.WriteLine("Hit! " + e);
                e.hit(this.damage);
                this._hp--;
            }
        }
    }

    private void autoDestruct()
    {
        //TODO change Position condition to adapt to all screen sizes
        if (Position.X < 25 || Position.X > 775 || Position.Y < 25 || Position.Y > 450 || _hp <= 0)
            World.RemoveEntity(this);
    }
}