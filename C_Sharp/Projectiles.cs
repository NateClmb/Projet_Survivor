using Microsoft.Xna.Framework;

namespace Projet_Survivor.C_Sharp;

public class Projectile : Entity
{
    private readonly Vector2 _direction;
    private bool _smart;
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
    }

    public override void Move(GameTime gameTime)
    {
        Position += _direction * Speed;
        SetHitboxPosition();
        AutoDestruct();
        HitTest(gameTime, e => e.Hitbox.Intersects(Hitbox) && (this._friendly && e.GetType() == typeof(Enemy) ||
                                                               !_friendly && e.GetType() == typeof(Player)));
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

    private void AutoDestruct()
    {
        if (Position.X < 0 || Position.X > World.WorldWidth || Position.Y < 0 || Position.Y > World.WorldHeight ||
            Hp <= 0)
            World.RemoveEntity(this);
    }

    protected override void GestionAnimation(GameTime gameTime)
    {
    }
}
