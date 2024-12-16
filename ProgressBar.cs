using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor;

public class ProgressBar
{
    private Texture2D background;
    private Texture2D foreground;
    private Vector2 position;
    private readonly int maxValue;
    private int currentValue;
    private Rectangle fraction;

    public ProgressBar(Texture2D background, Texture2D foreground, Vector2 position, int maxValue)
    {
        this.background = background;
        this.foreground = foreground;
        this.position = position;
        this.maxValue = maxValue;
        this.currentValue = 0;
        this.fraction = new Rectangle(0, 0, foreground.Width, foreground.Height);
    }

    public void Update(int value)
    {
        currentValue = value;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(background, position, Color.White);
        spriteBatch.Draw(foreground, position, fraction, Color.White);
    }
}