using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public abstract class Creature : Entity
{
    private double hp;
    protected Vector2 _speed;

    protected Creature(Hitbox h, Sprite sprite, Position pos, double hp, Vector2 speed) : base(h, sprite, pos)
    {
        this.hp = hp;
        this._speed = speed;
    }

    public void hit(int damage)
    {
        hp -= damage;
    }
}