using System;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Projet_Survivor.C_Sharp;

public class Projectile : Entity
{
    private readonly Vector2 _direction;
    private bool _smart;
    //If true, will only hurt enemies. Else will only hurt player
    private readonly bool _friendly;

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
        this.Damage = dmg;
        this._direction = direction;
        this._smart = smart;
        this._friendly = friendly;
        AdjustSprite(_direction);
    }

    //Rotate the sprite according to its firing angle
    private void AdjustSprite(Vector2 direction)
    {
        Sprite.Rotation = 2 * Math.Atan2(direction.Y - 0, direction.X + 1);
    }

    public override void Move(GameTime gameTime)
    {
        Position += _direction * Speed;
        SetHitboxPosition();
        AutoDestruct();
        HitTest(gameTime, e => e.Hitbox.Intersects(Hitbox) && (this._friendly && e is Enemy ||
                                                               !_friendly && e is Player));
    }

    protected override void IsHit(int damage, GameTime gameTime)
    {
        double time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= LastTimeHit + HIT_COUNTDOWN)
        {
            LastTimeHit = time;
            Hp -= damage;
        }
    }

    public bool IsFriendly()
    {
        return _friendly;
    }

    //Projectile disappear if it has no hp left or if it hits the window border
    private void AutoDestruct()
    {
        if (Position.X < 0 || Position.X > World.WorldWidth || Position.Y < 0 || Position.Y > World.WorldHeight ||
            Hp <= 0)
            World.RemoveEntity(this);
    }

    //Projectile aren't animated
    protected override void GestionAnimation(GameTime gameTime)
    {
    }
}