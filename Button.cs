using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Button
{
    private Vector2 position;
    private Texture2D texture;
    private Rectangle hitbox;
    private Action action;

    public Button(Texture2D texture, Vector2 pos)
    {
        this.texture = texture;
        this.position = position;
        hitbox = new Rectangle((int)this.position.X, (int)this.position.Y, World.WorldWidth/5,
            (texture.Height * World.WorldWidth / 5) / World.WorldHeight);
    }

    /**
     * @return true: If a player enters the button with mouse
     */
    public bool enterButton()
    {
        if (hitbox.Contains(Mouse.GetState().Position))
        {
            return true;
        }

        return false;
    }

    public void addAction(Action action)
    {
        this.action = action;
    }

    public void Update(GameTime gameTime)
    {
        if (enterButton() && Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            action();
            World.Unpause();
        }
    }

    public void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(texture, hitbox, color);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (enterButton())
            Draw(spriteBatch, Color.Gray);
        else
            Draw(spriteBatch, Color.White);
    }
}