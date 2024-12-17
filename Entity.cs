using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public abstract class Entity
{
    public Rectangle Hitbox;
    public Sprite Sprite;
    public Vector2 Position;
    protected int _hp;
    protected Vector2 Speed;
    private ArrayList spriteSheets = new ArrayList(); 
    private double lastTimeHit = 0;
    private static readonly double HIT_COUNTDOWN = 10;
    private int frameCounter = 0;
    private double lastTimeFrame = 0;
    private readonly double FRAME_INTERVAL = 250;

    public Entity(Rectangle hitbox, Sprite sprite, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.Sprite = sprite;
        this.Position = position;
        this.Speed = speed;
        SetHp(hp);
    }
    
    public Entity(Rectangle hitbox, ArrayList spriteSheets, Vector2 position, Vector2 speed, int hp)
    {
        this.Hitbox = hitbox;
        this.spriteSheets = spriteSheets;
        this.Sprite = (Sprite) spriteSheets[0];
        this.Position = position;
        this.Speed = speed;
        SetHp(hp);
    }

    public abstract void Move(GameTime gameTime);

    protected void setHitboxPosition()
    {
        this.Hitbox.X = (int)this.Position.X - Hitbox.Width / 2;
        this.Hitbox.Y = (int)this.Position.Y - Hitbox.Height / 2;
    }

    public void hit(int damage, GameTime gameTime)
    {
        double time = gameTime.TotalGameTime.TotalMilliseconds;
        if (time >= lastTimeHit + HIT_COUNTDOWN)
        {
            lastTimeHit = time;
            _hp -= damage;
        }
    }

    protected void GestionAnimation(GameTime gameTime)
    {
        if (this.Position.X > World.player.Position.X - 10)
        {
            Sprite.Flipped = true;
        }
        else
        {
            Sprite.Flipped = false;
        }
        if (gameTime.TotalGameTime.TotalMilliseconds > lastTimeFrame + FRAME_INTERVAL)
        {
            lastTimeFrame = gameTime.TotalGameTime.TotalMilliseconds;
            frameCounter = (frameCounter + 1) % spriteSheets.Count;
            bool flip = Sprite.Flipped;
            Sprite = (Sprite) spriteSheets[frameCounter];
            //On met à jour la position du Sprite car l'entité a possiblement bougé entre temps
            Sprite._position = this.Position;
            Sprite.Flipped = flip;
        }
    }

    private void SetHp(int hp)
    {
        if (hp <= 0)
        {
            hp = 1;
        }

        _hp = hp;
    }
}