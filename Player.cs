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
    public int level = 0;
    private int xpObjective = 100;
    public int totalXp { get; set; } = 0;
    private Weapon weapon { get; set; }
    private float MAX_SPEED = 6.0f;
    private readonly float ACCELERATION = 1.1f;
    
    //points pour les stats
    private int ptsAttributs;
    protected int ptsHP;
    protected int ptsAtkSpd;
    protected int ptsAtk;
    protected int ptsSpd;

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
        
    }

    /*
    public void gainXp(GameTime gameTime)
    {
        if (totalXp>=xpObjective)
        {
            totalXp-=xpObjective;
            levelUp();
        }
    }
    */
        
    private void levelUp()
    {
        level++;
        ptsAttributs++;
        xpObjective = (int) (xpObjective * 1.2);
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
        
        
        
        ////////////////////////////////////// lié au niveau, à faire valider
        if (Keyboard.GetState().IsKeyDown(Keys.V)){
            Console.WriteLine("\n"+totalXp+"\n"+xpObjective+"\n"+level+"\nHP:"+ptsHP+"\natk spd: "+ptsAtkSpd+"\nspeed: "+ptsSpd+"\natk: "+ptsAtk);
        }
        
        if (totalXp>=xpObjective)
        {
            totalXp-=xpObjective;
            levelUp();
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.U)){
            if (ptsAttributs>=1){
                ptsAttributs--;
                ptsHP++;
                _hp+=50;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.I)){
            if (ptsAttributs>=1){
                ptsAttributs--;
                ptsAtkSpd++;
                weapon.baseFireRate-=1;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.O)){
            if (ptsAttributs>=1){
                ptsAttributs--;
                ptsAtk++;
                weapon.damage+=15;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.P)){
            if (ptsAttributs>=1){
                ptsAttributs--;
                ptsSpd++;
                MAX_SPEED += 1.5f;
            }
        }
    //////////////////////////////////////////////////////////////////
    
    
    
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
        public int damage;
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
