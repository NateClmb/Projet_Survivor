#nullable enable
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Player : Entity
{
    private double attackSpd { get; set; }
    public int level;
    private int xpObjective = 50;
    private int currentXp;

    private int maxHp;

    private int frameCounter = 0;
    private double lastTimeFrame = 0;
    private readonly double FRAME_INTERVAL = 50;

    private static readonly double HIT_COUNTDOWN = 1000;

    private Weapon weapon { get; set; }

    private float MAX_SPEED { get; set; }

    private float ACCELERATION = 1.3f;

    public ProgressBar XpBar;

    public Player(Rectangle hitbox,
        ArrayList sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        double attackSpd) : base(hitbox, sprite, pos, speed, hp)
    {
        MAX_SPEED = 6.0f;
        this.attackSpd = attackSpd;
        this.weapon = new Weapon(World.defaultProjectileTexture, 1, 1, 500, this, new Vector2(10.0f, 10.0f));
        XpBar = new ProgressBar(World.xpBarBackground, World.xpBarForeground, new Vector2(30, 30));
        maxHp = hp;
    }

    public void increaseMaxHp()
    {
        maxHp++;
    }

    public void increaseMaxSpeed()
    {
        ACCELERATION += ACCELERATION * (MAX_SPEED + 0.2f) / MAX_SPEED;
        MAX_SPEED += 0.2f;
    }

    public void increaseDamage()
    {
        weapon.increaseDamage();
    }

    public void increaseAttackSpeed()
    {
        attackSpd -= 0.05;
    }

    public void heal(int heal)
    {
        _hp += heal;
        if (_hp > maxHp)
        {
            _hp = maxHp;
        }
    }

    public void gainXp(int xp)
    {
        currentXp += xp;
        if (currentXp >= xpObjective)
            levelUp();
        XpBar.Update(currentXp * 100 / xpObjective);
    }

    private void levelUp()
    {
        level++;
        currentXp -= xpObjective;
        xpObjective = (int)(xpObjective * 1.5);
        World.Pause();
    }

    protected override void IsHit(int damage, GameTime gameTime)
    {
        double time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= lastTimeHit + HIT_COUNTDOWN)
        {
            lastTimeHit = time;
            _hp -= damage;
        }

        if (_hp <= 0)
        {
            World.Pause();
        }
    }

    public override void Move(GameTime gameTime)
    {
        //déplacements aux flèches du clavier
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            Speed.X += ACCELERATION;
            if (Speed.X > MAX_SPEED) Speed.X = MAX_SPEED;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            Speed.X -= ACCELERATION;
            if (Speed.X < -MAX_SPEED) Speed.X = -MAX_SPEED;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            Speed.Y += ACCELERATION;
            if (Speed.Y > MAX_SPEED) Speed.Y = MAX_SPEED;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            Speed.Y -= ACCELERATION;
            if (Speed.Y < -MAX_SPEED) Speed.Y = -MAX_SPEED;
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
        if (Speed.X > 0) Speed.X -= 0.5f;
        if (Speed.X < 0) Speed.X += 0.5f;
        if (Speed.Y > 0) Speed.Y -= 0.5f;
        if (Speed.Y < 0) Speed.Y += 0.5f;

        //arrêt net si vitesse basse
        if (Math.Abs(Speed.X) < 0.5f) Speed.X = 0;
        if (Math.Abs(Speed.Y) < 0.5f) Speed.Y = 0;

        //réalignement de la hitbox sur le sprite
        setHitboxPosition();
        GestionAnimation(gameTime);

        //Fin déplacement

        //Attaque
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            double time = gameTime.TotalGameTime.TotalMilliseconds;
            if (time >= weapon.lastTimeFired + weapon.baseFireRate * attackSpd)
                World.AddEntity(weapon.fire(time));
        }
        //Fin attaque

        HitTest(gameTime,
            e => e.Hitbox.Intersects(Hitbox) && (e.GetType() == typeof(Enemy) ||
                                                 (e.GetType() == typeof(Projectile) && !((Projectile)e).isFriendly())));
    }

    protected override void GestionAnimation(GameTime gameTime)
    {
        if (Math.Abs(Speed.X) < 0.1f && Math.Abs(Speed.Y) < 0.1f)
        {
            Sprite = (Sprite)spriteSheets[0];
        }
        else if (gameTime.TotalGameTime.TotalMilliseconds > lastTimeFrame + FRAME_INTERVAL)
        {
            lastTimeFrame = gameTime.TotalGameTime.TotalMilliseconds;
            frameCounter = ((frameCounter + 1) % spriteSheets.Count);
            bool flip = Sprite.Flipped;
            Sprite = (Sprite)spriteSheets[frameCounter];
            //On met à jour la position du Sprite car l'entité a possiblement bougé entre temps
            Sprite._position = this.Position;
            Sprite.Flipped = flip;
            if (Speed.X > 0.1f)
            {
                Sprite.Flipped = true;
            }
            else
            {
                Sprite.Flipped = false;
            }
        }
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

        public void increaseDamage()
        {
            damage++;
        }
    }
}