using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Enemy : Creature
{
    private String name;
    private int xpValue { get; init; }
    private Behavior behavior;

    public Enemy(Hitbox h,
        Sprite sprite,
        Position pos,
        double hp,
        Vector2 speed,
        String name,
        int xpValue,
        Behavior behavior) : base(h, sprite, pos, hp, speed)
    {
        this.name = name;
        this.xpValue = xpValue;
        this.behavior = behavior;
    }

    //l'ennemi se rapproche en permanence du joueur
    public void move(GameTime gameTime, Player player)
    {
        if (_Position.X > player._Position.X)
        {
            _Position.X -= _speed.X;
        }
        else
        {
            _Position.X += _speed.X;
        }

        if (_Position.Y > player._Position.Y)
        {
            _Position.Y -= _speed.Y;
        }
        else
        {
            _Position.Y += _speed.Y;
        }
    }
}