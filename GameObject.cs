using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class GameObject
{
    protected Texture2D _texture;
    protected Vector2 _position;
    protected int _size = 100;
    protected static readonly int _sizeMin = -200;
    protected static readonly int _sizeMax = 200;
    protected Vector2 _speed;

    public int _Size
    {
        get => _size;
        set => _size = value >= _sizeMin ? (value <= _sizeMax ? value : _sizeMax) : _sizeMin;
    }


    public void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            if (_speed.X < 12.0f)
            {
                _speed.X += 1.1f;
            }

            if (_Size < 0)
            {
                _Size = -_Size;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            if (_speed.X > -12.0f)
            {
                _speed.X -= 1.1f;
            }

            if (_Size > 0)
            {
                _Size = -_Size;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            if (_speed.Y < 4.0f)
            {
                _speed.Y += 0.5f;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            if (_speed.Y > -4.0f)
            {
                _speed.Y -= 0.5f;
            }
        }

        _position.X = _position.X + _speed.X;
        _position.Y = _position.Y + _speed.Y;
        if (_speed.X > 0) _speed.X -= 0.05f;
        if (_speed.X < 0) _speed.X += 0.1f;
        if (_speed.Y > 0) _speed.Y -= 0.1f;
        if (_speed.Y < 0) _speed.Y += 0.1f;
        else if (Keyboard.GetState().IsKeyDown(Keys.T))
        {
            _Size += 2;
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.U))
        {
            _Size -= 2;
        }
        /*
        if (Keyboard.GetState().IsKeyDown(Keys.Left)) if(_Size>0) {_Size = -_Size; }
        if (Keyboard.GetState().IsKeyDown(Keys.Right)) if(_Size<0) {_Size = -_Size; }
        //...
        */
    }
}