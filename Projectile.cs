using System;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Projectile : Entity
{
    private double damage;
    private Vector2 direction;
    private bool smart;
    private bool friendly;

    public Projectile(Rectangle hitbox,
        Sprite sprite,
        Vector2 pos,
        Vector2 spd,
        Vector2 direction,
        int hp,
        double dmg,
        bool smart,
        bool friendly) : base(hitbox, sprite, pos, spd, hp)
    {
        this.damage = dmg;
        this.direction = direction;
        this.smart = smart;
        this.friendly = friendly;
    }

    public override void Move(GameTime gameTime)
    {
        Position += direction * Speed;
    }
}