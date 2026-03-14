using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Tile
{
    public int X;
    public int Y;
    public string Type;
    public bool Visited;

    protected Tile(int x, int y, string type)
    {
        this.X = x;
        this.Y = y;
        this.Type = type;
        this.Visited = false;
    }


    public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture)
    {
        if (spriteBatch is null)
            throw new ArgumentNullException(nameof(spriteBatch));
        if (texture is null)
            throw new ArgumentNullException(nameof(texture));

        int tileSize = 50;
        Rectangle destinationRectangle = new Rectangle(
            X * tileSize,
            Y * tileSize,
            tileSize,
            tileSize
        );


        Color tint = Type switch
        {
            "Road" => Color.Gray,
            "Grass" => Color.LightGreen,
            _ => Color.White
        };

        spriteBatch.Draw(texture, destinationRectangle, tint);
    }
}