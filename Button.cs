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
    private bool newClick = false;
    private bool startClick = false;

    public Button(Texture2D texture, Vector2 pos)
    {
        this.texture = texture;
        this.position = pos;
        hitbox = new Rectangle((int)this.position.X, (int)this.position.Y, World.WorldWidth / 5,
            (texture.Height * World.WorldWidth / 5) / World.WorldHeight);
    }

    /**
     * @return true: If a player enters the button with mouse
     */
    public bool inButton()
    {
        return hitbox.Contains(Mouse.GetState().Position);
    }

    public void addAction(Action action)
    {
        this.action = action;
    }

    public void Update(GameTime gameTime)
    {
        if (World.IsPaused)
        {
            if (inButton() && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                startClick = true;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                if (newClick && startClick && inButton())
                {
                    newClick = false;
                    Console.Out.WriteLine($"{action}");
                    action();
                    World.Unpause();
                }
                
                if (World.IsPaused)
                    newClick = true;
                startClick = false;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Color color)
    {
        spriteBatch.Draw(texture, hitbox, color);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (inButton())
            Draw(spriteBatch, Color.Gray);
        else
            Draw(spriteBatch, Color.White);
    }
}