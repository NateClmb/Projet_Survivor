using System;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Projet_Survivor.C_Sharp;

public abstract class Entity
{
    public Rectangle Hitbox;
    public Sprite Sprite;
    public Vector2 Position;
    protected int Hp;
    protected Vector2 Speed;
    protected readonly ArrayList SpriteSheet = new ArrayList();
    protected double LastTimeHit = 0;
    protected int Damage;

    protected Entity(Rectangle hitbox, Sprite sprite, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.Sprite = sprite;
        this.Position = position;
        this.Speed = speed;
        Damage = 1;
        SetHp(hp);
    }

    protected Entity(Rectangle hitbox, ArrayList spriteSheet, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.SpriteSheet = spriteSheet;
        this.Sprite = (Sprite)spriteSheet[World.Random.Next(0, spriteSheet.Count)];
        this.Position = position;
        this.Speed = speed;
        Damage = 1;
        SetHp(hp);
    }

    public abstract void Move(GameTime gameTime);

    protected void SetHitboxPosition()
    {
        this.Hitbox.X = (int)this.Position.X - Hitbox.Width / 2;
        this.Hitbox.Y = (int)this.Position.Y - Hitbox.Height / 2;
    }

    protected abstract void IsHit(int damage, GameTime gameTime);

    protected void HitTest(GameTime gameTime, Func<Entity, bool> test)
    {
        foreach (Entity e in World.GetEntities())
        {
            if (test(e))
            {
                IsHit(e.Damage, gameTime);
            }
        }
    }

    protected abstract void GestionAnimation(GameTime gameTime);

    private void SetHp(int hp)
    {
        if (hp <= 0)
        {
            hp = 1;
        }

        Hp = hp;
    }
}
