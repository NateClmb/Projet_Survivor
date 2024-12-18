using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Sprite
{
    private Texture2D _texture;
    public Vector2 _position;
    private int _size;
    private static readonly int _sizeMax = 256;
    private static readonly int _sizeMin = -_sizeMax;
    public bool Flipped = false;


    private Color _color = Color.White;

    public Texture2D _Texture
    {
        get => _texture;
        init => _texture = value;
    }

    public int _Size
    {
        get => _size;
        set => _size = value >= _sizeMin ? (value <= _sizeMax ? value : _sizeMax) : _sizeMin;
    }

    public Rectangle _Rect
    {
        get => new Rectangle((int)_position.X, (int)_position.Y, _size, _size);
    }

    public Sprite(Texture2D texture, Vector2 position, int size)
    {
        _Texture = texture;
        _position = position;
        _Size = size;
    }

    public void Flip()
    {
        Flipped = !Flipped;
    }

    public void Update(GameTime gameTime, Entity caller)
    {
        caller.Move(gameTime);
        _position = caller.Position;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        SpriteEffects effects;
        var origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        if (Flipped)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        else
        {
            effects = SpriteEffects.None;
        }
        spriteBatch.Draw(_texture, 
            _Rect, 
            null, 
            _color, 
            0.0f,
            origin, 
            effects, 
            0f); 
    }
}