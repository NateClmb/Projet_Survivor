using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor.C_Sharp;

public class ProgressBar
{
    private readonly Texture2D _background;
    private readonly Texture2D _foreground;
    private readonly Vector2 _position;
    private float _percentage;
    private Rectangle _fraction;

    public ProgressBar(Texture2D background, Texture2D foreground, Vector2 position)
    {
        this._background = background;
        this._foreground = foreground;
        this._position = position;
        this._percentage = 0;
        this._fraction = new Rectangle(0, 0, 0, foreground.Height);
    }

    public void Update(int value)
    {
        _percentage = value / 100.0f;
        _fraction.Width = (int)(_percentage * _foreground.Width);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_background, _position, Color.White);
        spriteBatch.Draw(_foreground, _position, _fraction, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
    }

    public int Width => _background.Width;
}