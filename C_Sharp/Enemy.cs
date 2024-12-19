using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor.C_Sharp;

public abstract class Enemy : Entity
{
    protected String Name;
    protected bool Hit;
    private int XpValue { get; init; }
    protected readonly Player _player = World.Player;

    private int _frameCounter;
    private double _lastTimeFrame;
    private readonly double FRAME_INTERVAL = 250;

    private static readonly double HIT_COUNTDOWN = 100;

    protected Enemy(Rectangle hitbox,
        ArrayList sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        String name,
        int xpValue,
        int damage) : base(hitbox, sprite, pos, speed, hp)
    {
        this.Damage = damage;
        this.Name = name;
        this.XpValue = xpValue;
    }

    //l'ennemi se rapproche en permanence du joueur

    public override void Move(GameTime gameTime)
    {
        EnemyMove(gameTime);
        SetHitboxPosition();
        GestionAnimation(gameTime);
        HitTest(gameTime,
            e => e.Hitbox.Intersects(Hitbox) && e is Projectile projectile && projectile.IsFriendly());
        TestOverlapseWithEnemy();
        if (gameTime.TotalGameTime.TotalMilliseconds >= LastTimeHit + HIT_COUNTDOWN)
            Sprite.Color = Color.White;
    }

    protected abstract void EnemyMove(GameTime gameTime);

    private void TestOverlapseWithEnemy()
    {
        foreach (Entity e in World.GetEntities())
        {
            if (e.Hitbox.Intersects(Hitbox) && e is Enemy)
            {
                Vector2 stepAside = new Vector2(Position.X - e.Position.X, Position.Y - e.Position.Y);
                Position.X += 0.05f * stepAside.X;
                Position.Y += 0.05f * stepAside.Y;
            }
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
            //Update Sprite's position because the entity may have moved 
            Sprite.Position = this.Position;
            Sprite.Flipped = flip;
        }
    }

    protected override void IsHit(int damage, GameTime gameTime)
    {
        double time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= LastTimeHit + HIT_COUNTDOWN)
        {
            Sprite.Color = Color.Red;
            LastTimeHit = time;
            Hp -= damage;
        }
        
        if (Hp <= 0)
        {
            _player.GainXp(XpValue);
            World.RemoveEntity(this);
        }
    }
}