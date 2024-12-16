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
    public int level;
    private int xpObjective = 100;
    private int currentXp;
    private Weapon weapon { get; set; }
    private readonly float MAX_SPEED = 6.0f;
    private readonly float ACCELERATION = 1.1f;

    public ProgressBar XpBar;

    public Player(Rectangle hitbox,
        Sprite sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        double attackSpd) : base(hitbox, sprite, pos, speed, hp)
    {
        this.attackSpd = attackSpd;
        this.weapon = new Weapon(World.defaultProjectileTexture, 1, 10, 500, this, new Vector2(10.0f, 10.0f));
        XpBar = new ProgressBar(World.xpBarBackground, World.xpBarForeground, new Vector2(30, 30));
    }

    public void heal(int heal)
    {
        
    }

    public void gainXp(int xp)
    {
        currentXp += xp;
        levelUp();
        XpBar.Update(currentXp * 100 /xpObjective);
    }
        
    private void levelUp()
    {
        if (currentXp >= xpObjective)
        {
            level++;
            currentXp -= xpObjective;
            xpObjective = (int) (xpObjective * 1.5);
        }
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

        //limite dans la box
        float halfSpriteSize = Sprite._Size / 2.0f;
        if (Position.X < halfSpriteSize) Position.X = halfSpriteSize;
        if (Position.X > World.WorldWidth - halfSpriteSize) Position.X = World.WorldWidth - halfSpriteSize;
        if (Position.Y < halfSpriteSize) Position.Y = halfSpriteSize;
        if (Position.Y > World.WorldHeight - halfSpriteSize) Position.Y = World.WorldHeight - halfSpriteSize;

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
            return new Projectile(new Rectangle((int)player.Position.X, (int)player.Position.Y, 32, 32),
                new Sprite(projectileTexture, player.Position, 32), player.Position, projectileSpeed,
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