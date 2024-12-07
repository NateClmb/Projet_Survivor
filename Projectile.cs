using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Projectile : Entity
{
    private double damage;
    private bool smart;
    private bool friendly;

    public Projectile(Rectangle hitbox,
        Sprite sprite,
        Vector2 pos,
        Vector2 spd,
        int hp,
        double dmg,
        bool smart,
        bool friendly) : base(hitbox, sprite, pos, spd, hp)
    {
        this.damage = dmg;
        this.smart = smart;
        this.friendly = friendly;
    }

    public override void Move(GameTime gameTime)
    {
        Position += Speed;
    }
}