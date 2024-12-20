using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_Survivor.C_Sharp;

public class Sprite
{
    private readonly Texture2D _texture;
    public Vector2 Position;
    private readonly int _size;
    private static readonly int SizeMax = 256;
    private static readonly int SizeMin = 0;
    //If true, Sprite is drawn flipped on the horizontal axis to always face the direction it's moving
    public bool Flipped;
    public double Rotation;

    //By default, there's no coloring filter
    public Color Color = Color.White;

    public Texture2D Texture
    {
        get => _texture;
        init => _texture = value;
    }

    public int Size
    {
        get => _size;
        private init => _size = value >= SizeMin ? (value <= SizeMax ? value : SizeMax) : SizeMin;
    }

    private Rectangle Rect => new Rectangle((int)Position.X, (int)Position.Y, _size, _size);

    public Sprite(Texture2D texture, Vector2 position, int size)
    {
        Texture = texture;
        Position = position;
        Size = size;
    }

    public void Flip()
    {
        Flipped = !Flipped;
    }

    public void Update(GameTime gameTime, Entity caller)
    {
        caller.Move(gameTime);
        Position = caller.Position;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        SpriteEffects effects = Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        spriteBatch.Draw(_texture,
            Rect,
            null,
            Color,
            (float) Rotation,
            origin,
            effects,
            0f);
    }
}