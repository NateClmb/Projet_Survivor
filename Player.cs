#nullable enable
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Player : Entity
{
    private double attackSpd { get; set; }
    private int level;
    private int exp;
    private Weapon weapon { get; set; }
    private readonly float MAX_SPEED = 6.0f;
    private readonly float ACCELERATION = 1.1f;

    public Player(Rectangle hitbox,
        Sprite sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        double attackSpd) : base(hitbox, sprite, pos, speed, hp)
    {
        this.attackSpd = attackSpd;
        this.weapon = new Weapon(World.defaultProjectileTexture , 1, 1, 1, this, 1.0f);
    }

    public void heal(int heal)
    {
        return;
    }

    public override void Move(GameTime gameTime)
    {
        //déplacements aux flèches du clavier
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            if (Speed.X < MAX_SPEED)
                Speed.X += ACCELERATION;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            if (Speed.X > -MAX_SPEED)
            {
                Speed.X -= ACCELERATION;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            if (Speed.Y < MAX_SPEED)
            {
                Speed.Y += ACCELERATION;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            if (Speed.Y > -MAX_SPEED)
            {
                Speed.Y -= ACCELERATION;
            }
        }

        Position.X += Speed.X;
        Position.Y += Speed.Y;

        //limite dans la box (sûrement à retirer plus tard)
        if (Position.X < 25) Position.X = 25;
        if (Position.X > 775) Position.X = 775;
        if (Position.Y < 25) Position.Y = 25;
        if (Position.Y > 450) Position.Y = 450;

        //décélération avec le temps
        if (Speed.X > 0) Speed.X -= 0.1f;
        if (Speed.X < 0) Speed.X += 0.1f;
        if (Speed.Y > 0) Speed.Y -= 0.1f;
        if (Speed.Y < 0) Speed.Y += 0.1f;

        //Fin déplacement

        //Attaque
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            World.AddEntity(weapon.fire());
        }
        //Fin attaque
    }

    private class Weapon
    {
        private Player player;
        private readonly Rectangle projectileHitbox = new Rectangle(0, 0, 100, 100);
        private Texture2D projectileTexture;
        private int piercePotential;
        private int damage;
        private float baseFireRate;
        private bool smart;
        private float projectileSpeed;

        public Weapon(Texture2D projectileTexture, int piercePotential, int damage, float baseFireRate, Player player, float projectileSpeed)
        {
            this.player = player;
            this.projectileTexture = projectileTexture;
            this.piercePotential = piercePotential;
            this.damage = damage;
            this.baseFireRate = baseFireRate;
            this.projectileSpeed = projectileSpeed;
            this.smart = true;
        }

        public Projectile fire()
        {
            return new Projectile(projectileHitbox, new Sprite(projectileTexture, player.Position, 30), player.Position, calculateSpeed(), piercePotential,
                damage, smart, true);
        }

        private Vector2 calculateSpeed()
        {
            Vector2 speed = new Vector2(projectileSpeed, 0);
            if (smart)
            {
                Entity? nearestEnemy = NearestTarget();
                if (nearestEnemy != null)
                {
                    Vector2.Rotate(speed, (float)Math.Atan2(nearestEnemy.Position.Y - player.Position.Y,
                        nearestEnemy.Position.X - player.Position.X));
                }
            }
            else
            {
                Vector2.Rotate(speed, (float)Math.Atan2(Mouse.GetState().Y - player.Position.Y,
                    Mouse.GetState().Position.X - player.Position.X));
            }

            return speed;
        }

        private Enemy? NearestTarget()
        {
            Enemy? nearest = null;
            float minDist = float.MaxValue;
            foreach (Entity e in World.GetEntities())
            {
                if (e.GetType() == typeof(Enemy))
                {
                    float dist = Vector2.Distance(e.Position, player.Position);
                    if (dist < minDist)
                        minDist = dist;
                }
            }
            Console.Write(nearest);
            return nearest;
        }
    }
}