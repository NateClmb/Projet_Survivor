#nullable enable
using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor.C_Sharp;

public class Player : Entity
{
    private double _attackSpd;
    public int Level;
    private int _xpObjective = 50;
    private int _currentXp;
    private int _maxHp;

    private int _frameCounter;
    private double _lastTimeFrame;
    private readonly double FRAME_INTERVAL = 50;

    private bool _hit;
    private static readonly double HIT_COUNTDOWN = 1000;
    private static readonly int NB_FRAME_MOVEMENT = 3;
    private static readonly int NB_FRAME_HIT = 3;

    private float _maxSpeed;
    private float _acceleration = 1.3f;

    private Weapon _weapon;
    public ProgressBar XpBar;

    public Player(Rectangle hitbox,
        ArrayList sprite,
        Vector2 pos,
        Vector2 speed,
        int hp,
        double attackSpd) : base(hitbox, sprite, pos, speed, hp)
    {
        _maxSpeed = 6.0f;
        this._attackSpd = attackSpd;
        this._weapon = new Weapon(this, World.DefaultProjectileTexture, 1, 1, 500, new Vector2(10.0f, 10.0f));
        XpBar = new ProgressBar(World.XpBarBackground, World.XpBarForeground, new Vector2(30, 30));
        _maxHp = hp;
    }

    public String GetHp()
    {
        return Hp.ToString() + " / " + _maxHp.ToString();
    }

    public void IncreaseMaxHp()
    {
        _maxHp++;
        Heal(1);
    }

    public void IncreaseMaxSpeed()
    {
        _acceleration += _acceleration * (_maxSpeed + 0.2f) / _maxSpeed;
        _maxSpeed += 0.5f;
    }

    public void IncreaseDamage()
    {
        _weapon.IncreaseDamage();
    }

    public void IncreaseAttackSpeed()
    {
        _attackSpd -= 0.2;
    }

    public void Heal(int heal)
    {
        Hp += heal;
        if (Hp > _maxHp)
        {
            Hp = _maxHp;
        }
    }

    public void GainXp(int xp)
    {
        _currentXp += xp;
        if (_currentXp >= _xpObjective)
            LevelUp();
        XpBar.Update(_currentXp * 100 / _xpObjective);
    }

    private void LevelUp()
    {
        Level++;
        Heal(2);
        _currentXp -= _xpObjective;
        _xpObjective = (int)(_xpObjective * 1.5);
        World.Pause();
    }

    protected override void IsHit(int damage, GameTime gameTime)
    {
        double time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= LastTimeHit + HIT_COUNTDOWN)
        {
            _hit = true;
            LastTimeHit = time;
            Hp -= damage;
        }

        if (Hp <= 0)
        {
            World.GameOver();
        }
    }

    public override void Move(GameTime gameTime)
    {
        //déplacements aux flèches du clavier
        if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
        {
            Speed.X += _acceleration;
            if (Speed.X > _maxSpeed) Speed.X = _maxSpeed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Q))
        {
            Speed.X -= _acceleration;
            if (Speed.X < -_maxSpeed) Speed.X = -_maxSpeed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
        {
            Speed.Y += _acceleration;
            if (Speed.Y > _maxSpeed) Speed.Y = _maxSpeed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Z))
        {
            Speed.Y -= _acceleration;
            if (Speed.Y < -_maxSpeed) Speed.Y = -_maxSpeed;
        }

        Position.X += Speed.X;
        Position.Y += Speed.Y;

        //limite dans la box
        float halfSpriteSize = Sprite.Size / 2.0f;
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
        SetHitboxPosition();
        GestionAnimation(gameTime);

        //Fin déplacement

        //Attaque
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            double time = gameTime.TotalGameTime.TotalMilliseconds;
            if (time >= _weapon.LastTimeFired + _weapon.BaseFireRate * _attackSpd)
                World.AddEntity(_weapon.Fire(time));
        }
        //Fin attaque

        HitTest(gameTime,
            e => e.Hitbox.Intersects(Hitbox) &&
                 (e is Enemy || (e is Projectile projectile && !projectile.IsFriendly())));
        if (gameTime.TotalGameTime.TotalMilliseconds >= LastTimeHit + HIT_COUNTDOWN)
            _hit = false;
    }

    protected override void GestionAnimation(GameTime gameTime)
    {
        if (!_hit && Math.Abs(Speed.X) < 0.1f && Math.Abs(Speed.Y) < 0.1f)
        {
            Sprite = (Sprite)SpriteSheet[0];
        }
        else if (gameTime.TotalGameTime.TotalMilliseconds > _lastTimeFrame + FRAME_INTERVAL)
        {
            _lastTimeFrame = gameTime.TotalGameTime.TotalMilliseconds;
            if (_hit)
            {
                _frameCounter = (_frameCounter + 1) % (SpriteSheet.Count - NB_FRAME_MOVEMENT);
                Sprite = (Sprite)SpriteSheet[_frameCounter + NB_FRAME_MOVEMENT];
            }
            else
            {
                _frameCounter = (_frameCounter + 1) % (NB_FRAME_MOVEMENT);
                Sprite = (Sprite)SpriteSheet[_frameCounter];
            }

            bool flip = Sprite.Flipped;
            //Update Sprite's position because the entity may have moved 
            Sprite.Position = this.Position;
            Sprite.Flipped = flip;
            Sprite.Flipped = Speed.X > 0.1f;
        }
    }

    private class Weapon
    {
        private Player _player;
        private Texture2D _projectileTexture;
        private int _piercePotential;
        private int _damage;
        private bool _smart;
        private Vector2 _projectileSpeed;
        public double BaseFireRate;
        public double LastTimeFired;

        public Weapon(Player player, Texture2D projectileTexture, int piercePotential, int damage, float baseFireRate,
            Vector2 projectileSpeed)
        {
            this._player = player;
            this._projectileTexture = projectileTexture;
            this._piercePotential = piercePotential;
            this._damage = damage;
            this.BaseFireRate = baseFireRate;
            this._projectileSpeed = projectileSpeed;
            this._smart = false;
        }

        public Projectile Fire(double time)
        {
            LastTimeFired = time;
            return new Projectile(new Rectangle((int)_player.Position.X, (int)_player.Position.Y, 32, 32),
                new Sprite(_projectileTexture, _player.Position, 32), _player.Position, _projectileSpeed,
                CalculateSpeed(), _piercePotential, _damage, _smart, true);
        }

        private Vector2 CalculateSpeed()
        {
            Vector2 direction;
            if (_smart)
            {
                direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _player.Position;
                Entity? nearestEnemy = NearestTarget();
                if (nearestEnemy != null)
                {
                    direction = nearestEnemy.Position - _player.Position;
                }
            }
            else
            {
                direction = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _player.Position;
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
                    float dist = Vector2.Distance(e.Position, _player.Position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearest = (Enemy)e;
                    }
                }
            }

            return nearest;
        }

        public void IncreaseDamage()
        {
            _damage++;
        }
    }
}
