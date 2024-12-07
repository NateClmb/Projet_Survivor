using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public abstract class Entity
{
    public Rectangle Hitbox { init; get; }
    public Sprite Sprite;
    public Vector2 Position;
    private int _hp;
    protected Vector2 Speed;

    public Entity(Rectangle hitbox, Sprite sprite, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.Sprite = sprite;
        this.Position = position;
        this.Speed = speed;
        SetHp(hp);
    }

    public abstract void Move(GameTime gameTime);

    public void hit(int damage)
    {
        _hp -= damage;
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