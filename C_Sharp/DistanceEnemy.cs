using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor.C_Sharp;

public class DistanceEnemy : Enemy
{
    private readonly float MIN_DISTANCE_FROM_PLAYER = 400;
    private readonly float FIRING_DISTANCE = 600;
    private double _lastTimeFired;
    private readonly double FIRE_RATE = 3000;
    
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

    protected override void EnemyMove(GameTime gameTime)
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
        
        var time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= _lastTimeFired + FIRE_RATE && Vector2.Distance(Position, _player.Position) <= FIRING_DISTANCE)
            World.AddEntity(Fire(time));
    }
    
    public Projectile Fire(double time)
    {
        _lastTimeFired = time;
        return new Projectile(new Rectangle((int)Position.X, (int)Position.Y, 32, 32),
            new Sprite(World.DefaultProjectileTexture, Position, 32), Position, new Vector2(5.0f, 5.0f),
            Vector2.Normalize(_player.Position - Position), 1, Damage, false, false);
    }
    
}