using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor.C_Sharp;

public class HandToHandEnemy : Enemy
{
    public HandToHandEnemy(
        Rectangle hitbox,
        ArrayList sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        String name,
        int xpValue,
        int damage) : base(hitbox, sprite, pos, speed, hp, name, xpValue, damage)
    {
    }

    protected override void EnemyMove()
    {
        Vector2 direction = Vector2.Normalize(_player.Position - Position);
        Position += direction * Speed;
    }
}