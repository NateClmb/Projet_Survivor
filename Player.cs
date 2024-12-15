#nullable enable
using System;
using System.Runtime.CompilerServices;
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
        this.weapon = new Weapon(World.defaultProjectileTexture, 1, 10, 500, this, new Vector2(10.0f, 10.0f));
    }

    public void heal(int heal)
    {
        return;
    }

    public override void Move(GameTime gameTime)
    {
        //déplacements aux flèches du clavier
        if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
        {
            if (Speed.X < MAX_SPEED)
                Speed.X += ACCELERATION;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Q))
        {
            if (Speed.X > -MAX_SPEED)
            {
                Speed.X -= ACCELERATION;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
        {
            if (Speed.Y < MAX_SPEED)
            {
                Speed.Y += ACCELERATION;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Z))
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

        //réalignement de la hitbox sur le sprite
        setHitboxPosition();

        //Fin déplacement

        //Attaque
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            double time = gameTime.TotalGameTime.TotalMilliseconds;
            if (time >= weapon.lastTimeFired + weapon.baseFireRate)
                World.AddEntity(weapon.fire(time));
        }
        //Fin attaque
    }

    private class Weapon
    {
        private Player player;
        private Texture2D projectileTexture;
        private int piercePotential;
        private int damage;
        private bool smart;
        private Vector2 projectileSpeed;
        public double baseFireRate;
        public double lastTimeFired = 0;

        public Weapon(Texture2D projectileTexture, int piercePotential, int damage, float baseFireRate, Player player,
            Vector2 projectileSpeed)
        {
            this.player = player;
            this.projectileTexture = projectileTexture;
            this.piercePotential = piercePotential;
            this.damage = damage;
            this.baseFireRate = baseFireRate;
            this.projectileSpeed = projectileSpeed;
            this.smart = false;
        }

        public Projectile fire(double time)
        {
            lastTimeFired = time;
            return new Projectile(new Rectangle((int)player.Position.X, (int)player.Position.Y, 30, 30),
                new Sprite(projectileTexture, player.Position, 30), player.Position, projectileSpeed,
                calculateSpeed(), piercePotential, damage, smart, true);
        }

        private Vector2 calculateSpeed()
        {
            Vector2 direction;
            if (smart)
            {
                direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - player.Position;
                Entity? nearestEnemy = NearestTarget();
                if (nearestEnemy != null)
                {
                    direction = nearestEnemy.Position - player.Position;
                }
            }
            else
            {
                direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - player.Position;
            }

            direction.Normalize();
            return direction;
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
                    {
                        minDist = dist;
                        nearest = (Enemy)e;
                    }
                }
            }

            return nearest;
        }
    }
}
