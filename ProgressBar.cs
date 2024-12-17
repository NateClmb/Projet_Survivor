using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor;

public class ProgressBar
{
    private Texture2D background;
    private Texture2D foreground;
    private Vector2 position;
    private float percentage;
    private Rectangle fraction;

    public ProgressBar(Texture2D background, Texture2D foreground, Vector2 position)
    {
        this.background = background;
        this.foreground = foreground;
        this.position = position;
        this.percentage = 0;
        this.fraction = new Rectangle(0, 0, 0, foreground.Height);
    }

    public void Update(int value)
    {
        percentage = value / 100.0f;
        fraction.Width = (int)(percentage * foreground.Width);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background, position, Color.White);
        spriteBatch.Draw(foreground, position, fraction, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
    }

    public int Width => background.Width;
}