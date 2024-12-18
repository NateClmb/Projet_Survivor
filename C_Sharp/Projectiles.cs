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
    
    private static readonly double HIT_COUNTDOWN = 100;

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
        autoDestruct();
        HitTest(gameTime, e => e.Hitbox.Intersects(Hitbox) && (this.friendly && e.GetType() == typeof(Enemy) ||
                                                               !friendly && e.GetType() == typeof(Player)));
    }

    protected override void IsHit(int damage, GameTime gameTime)
    {
        double time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= lastTimeHit + HIT_COUNTDOWN)
        {
            lastTimeHit = time;
            _hp -= damage;
        }
    }

    public bool isFriendly()
    {
        return friendly;
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