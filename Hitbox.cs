using Microsoft.Xna.Framework;

namespace Projet_Survivor;

public class Hitbox
{
    private int _size;
    private Vector2 _position;

    public Hitbox(int size, Vector2 position)
    {
        _size = size;
        _position = position;
    }
    
}