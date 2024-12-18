using System;
using System.Collections;
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
    protected ArrayList spriteSheets = new ArrayList(); 
    protected double lastTimeHit = 0;
    protected int damage;

    public Entity(Rectangle hitbox, Sprite sprite, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.Sprite = sprite;
        this.Position = position;
        this.Speed = speed;
        damage = 1;
        SetHp(hp);
    }
    
    public Entity(Rectangle hitbox, ArrayList spriteSheets, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.spriteSheets = spriteSheets;
        this.Sprite = (Sprite) spriteSheets[World.random.Next(0, spriteSheets.Count)];
        this.Position = position;
        this.Speed = speed;
        damage = 1;
        SetHp(hp);
    }

    public abstract void Move(GameTime gameTime);

    protected void setHitboxPosition()
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
                IsHit(e.damage, gameTime);
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

        _hp = hp;
    }
}