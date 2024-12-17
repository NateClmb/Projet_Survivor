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
        hitTest(gameTime);
        autoDestruct();
    }

    private void hitTest(GameTime gameTime)
    {
        foreach (Entity e in World.GetEntities())
        {
            if (e.Hitbox.Intersects(Hitbox) && (this.friendly && e.GetType() == typeof(Enemy) ||
                                                !friendly && e.GetType() == typeof(Player)))
            {
                e.hit(this.damage, gameTime);
                this._hp--;
            }
        }
    }

    private void autoDestruct()
    {
        if (Position.X < 0 || Position.X > World.WorldWidth || Position.Y < 0 || Position.Y > World.WorldHeight ||
            _hp <= 0)
            World.RemoveEntity(this);
    }

    protected override void GestionAnimation(GameTime gameTime)
    {
        return;
    }
}