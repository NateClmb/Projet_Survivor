using System;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Projet_Survivor.C_Sharp;

public class Enemy : Entity
{
    private String _name;
    private int XpValue { get; init; }
    private Behavior _behavior;
    private readonly Player _player = World.Player;

    private int _frameCounter;
    private double _lastTimeFrame;
    private readonly double FRAME_INTERVAL = 250;

    private static readonly double HIT_COUNTDOWN = 100;

    public Enemy(Rectangle hitbox,
        ArrayList sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        String name,
        int xpValue,
        Behavior behavior) : base(hitbox, sprite, pos, speed, hp)
    {
        this._name = name;
        this.XpValue = xpValue;
        this._behavior = behavior;
    }

    //l'ennemi se rapproche en permanence du joueur

    public override void Move(GameTime gameTime)
    {
        if (Position.X > _player.Position.X)
        {
            Position.X -= Speed.X;
        }
        else
        {
            Position.X += Speed.X;
        }

        if (Position.Y > _player.Position.Y)
        {
            Position.Y -= Speed.Y;
        }
        else
        {
            Position.Y += Speed.Y;
        }

        SetHitboxPosition();
        GestionAnimation(gameTime);
        HitTest(gameTime,
            e => e.Hitbox.Intersects(Hitbox) && e.GetType() == typeof(Projectile) && ((Projectile)e).IsFriendly());
        TestOverlapseWithEnemy();
        Die();
    }

    private void TestOverlapseWithEnemy()
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

    private void Die()
    {
        if (Hp <= 0)
        {
            _player.GainXp(XpValue);
            World.GetEntities().Remove(this);
        }
    }

    protected override void GestionAnimation(GameTime gameTime)
    {
        Sprite.Flipped = this.Position.X > World.Player.Position.X - 10;

        if (gameTime.TotalGameTime.TotalMilliseconds > _lastTimeFrame + FRAME_INTERVAL)
        {
            _lastTimeFrame = gameTime.TotalGameTime.TotalMilliseconds;
            _frameCounter = (_frameCounter + 1) % SpriteSheet.Count;
            bool flip = Sprite.Flipped;
            Sprite = (Sprite)SpriteSheet[_frameCounter];
            //On met à jour la position du Sprite car l'entité a possiblement bougé entre temps
            Sprite.Position = this.Position;
            Sprite.Flipped = flip;
        }
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
}