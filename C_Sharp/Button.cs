using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor.C_Sharp;

public class Button
{
    private readonly Texture2D _texture;
    private Rectangle _hitbox;
    private Action _action;
    private bool _newClick;
    private bool _startClick;

    public Button(Texture2D texture, Vector2 pos)
    {
        this._texture = texture;
        _hitbox = new Rectangle((int)pos.X, (int)pos.Y, World.WorldWidth / 5,
            (texture.Height * World.WorldWidth / 5) / World.WorldHeight);
    }
    
    private bool IsInButton()
    {
        return _hitbox.Contains(Mouse.GetState().Position);
    }

    //Link an action to a button
    public void AddAction(Action action)
    {
        this._action = action;
    }
    
    public void Update(GameTime gameTime)
    {
        if (World.IsPaused)
        {
            if (IsInButton() && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _startClick = true;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                if (_newClick && _startClick && IsInButton())
                {
                    _newClick = false;
                    _action();
                    World.Unpause();
                }

                if (World.IsPaused)
                    _newClick = true;
                _startClick = false;
            }
        }
    }

    private void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(_texture, _hitbox, color);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Draw(spriteBatch, IsInButton() ? Color.Gray : Color.White);
    }
}