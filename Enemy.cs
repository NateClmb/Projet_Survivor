using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Enemy : Entity
{
    private String name;
    private int xpValue { get; init; }
    private Behavior behavior;
    private Player player = (Player) World.GetEntities()[0];
    private readonly float MinDistance = 100.0f;

    public Enemy(Rectangle hitbox,
        Sprite sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        String name,
        int xpValue,
        Behavior behavior) : base(hitbox, sprite, pos, speed, hp)
    {
        this.name = name;
        this.xpValue = xpValue;
        this.behavior = behavior;
    }

    //l'ennemi se rapproche en permanence du joueur
    
     
    // Main movement method
    public override void Move(GameTime gameTime)
    {
        switch (behavior)
        {
            case Behavior.HAND_TO_HAND:
                MoveTowardsPlayer(player);
                break;

            case Behavior.DISTANCE:
                MaintainDistanceFromPlayer(player);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Moves the enemy directly toward the player's position
    private void MoveTowardsPlayer(Player player)
    {
        Vector2 direction = Vector2.Normalize(player.Position - Position);
        Position += direction * Speed;
        setHitboxPosition();
        die();
    }

    private void MaintainDistanceFromPlayer(Player player)
    {
        float distanceToPlayer = Vector2.Distance(Position, player.Position);
        Vector2 direction = Vector2.Normalize(player.Position - Position);

        if (distanceToPlayer < MinDistance)
        {
            Position -= direction * Speed;
        }
        else 
        {
            Position += direction * Speed;
        }
        setHitboxPosition();
        die();
    }
    
    private void die()
    {
        //TODO change Position condition to adapt to all screen sizes
        if (_hp <= 0)
        {
            World.RemoveEntity(this);
            player.totalXp += xpValue;
        }
    }
}
