using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor.C_Sharp;

public class DistanceEnemy : Enemy
{
    private readonly float MIN_DISTANCE_FROM_PLAYER = 200;
    
    public DistanceEnemy(
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
        float distanceToPlayer = Vector2.Distance(Position, _player.Position);
        Vector2 direction = Vector2.Normalize(_player.Position - Position);

        if (distanceToPlayer < MIN_DISTANCE_FROM_PLAYER)
        {
            // Move away from the player
            Position -= direction * Speed;
        }
        else 
        {
            // Move closer to the player
            Position += direction * Speed;
        }
        // If within the desired range, do not move
    }
}