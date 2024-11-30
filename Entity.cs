using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_Survivor;

public class Entity
{
    private Hitbox _hitbox;

    public Hitbox _Hitbox
    {
        get => _hitbox;
        init => _hitbox = value;
    }

    private Sprite sprite { get; set; }

    private Position _position;

    public Position _Position
    {
        get => _position;
        set => _position = value;
    }

    public Entity(Hitbox hitbox, Sprite sprite, Position position)
    {
        this._Hitbox = hitbox;
        this.sprite = sprite;
        this._Position = position;
    }

    private void gestionAnimation()
    {
    }
}