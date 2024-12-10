using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public abstract class Entity
{
    public Rectangle Hitbox;
    public Sprite Sprite;
    public Vector2 Position;
    protected int _hp;
    protected Vector2 Speed;
    private double lastTimeHit = 0;
    private static readonly double HIT_COUNTDOWN = 10;

    public Entity(Rectangle hitbox, Sprite sprite, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.Sprite = sprite;
        this.Position = position;
        this.Speed = speed;
        SetHp(hp);
    }

    public abstract void Move(GameTime gameTime);

    protected void setHitboxPosition()
    {
        this.Hitbox.X = (int)this.Position.X - Hitbox.Width / 2;
        this.Hitbox.Y = (int)this.Position.Y - Hitbox.Height / 2;
    }

    public void hit(int damage, GameTime gameTime)
    {
        double time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= lastTimeHit + HIT_COUNTDOWN)
        {
            lastTimeHit = time;
            _hp -= damage;
        }
    }

    private void GestionAnimation()
    {
    }

    private void SetHp(int hp)
    {
        if (hp <= 0)
        {
            hp = 1;
        }

        _hp = hp;
    }
}