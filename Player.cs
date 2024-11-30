using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Player : Creature
{
    private double attackSpd { get; set; }
    private int level;
    private int exp;
    private Weapon weapon { get; init; }
    private readonly float MAX_SPEED = 6.0f;
    private readonly float ACCELERATION = 1.1f;

    public Player(Hitbox h,
        Sprite sprite,
        Position pos,
        double hp,
        Vector2 speed,
        double attackSpd) : base(h, sprite, pos, hp, speed)
    {
        this.attackSpd = attackSpd;
    }

    public void heal(int heal)
    {
        return;
    }

    public void Move(GameTime gameTime)
    {
        //déplacements aux flèches du clavier
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            if (_speed.X < MAX_SPEED)
                _speed.X += ACCELERATION;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            if (_speed.X > -MAX_SPEED)
            {
                _speed.X -= ACCELERATION;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            if (_speed.Y < MAX_SPEED)
            {
                _speed.Y += ACCELERATION;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            if (_speed.Y > -MAX_SPEED)
            {
                _speed.Y -= ACCELERATION;
            }
        }

        _Position.X += _speed.X;
        _Position.Y += _speed.Y;

        //limite dans la box (sûrement à retirer plus tard)
        if (_Position.X < 25) _Position.X = 25;
        if (_Position.X > 775) _Position.X = 775;
        if (_Position.Y < 25) _Position.Y = 25;
        if (_Position.Y > 450) _Position.Y = 450;

        //décélération avec le temps
        if (_speed.X > 0) _speed.X -= 0.05f;
        if (_speed.X < 0) _speed.X += 0.1f;
        if (_speed.Y > 0) _speed.Y -= 0.1f;
        if (_speed.Y < 0) _speed.Y += 0.1f;
    }

    private class Weapon()
    {
    }
}