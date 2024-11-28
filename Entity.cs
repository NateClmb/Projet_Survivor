using System ;
using Microsoft . Xna . Framework ;
using Microsoft . Xna . Framework . Graphics ;
using Microsoft . Xna . Framework . Input ;

namespace Projet_Survivor ;

public class Entity {

    private Hitbox hitbox { get; init; }
    private Sprite sprite { get; set; }
    private Position position { get; set; }
    
    public Entity (Hitbox hitbox, Sprite sprite, Position position){
        this.hitbox=hitbox;
        this.sprite=sprite;
        this.position=position;
    }
    
    private void gestionAnimation(){
    }
}