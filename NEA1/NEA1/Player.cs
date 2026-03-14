using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NEA1 
{
    class Player
    {
        public int X { get; set; } 
        public int Y { get; set; }

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
        }

        public void Update(KeyboardState keyboardState, Map gameMap)
        {
            int newX = X;
            int newY = Y;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) newY--;
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) newY++;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) newX--;
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) newX++;


            if (newX >= 0 && newY >= 0 && 
                newY < gameMap.map.Count && newX < gameMap.map[newY].Count && 
                gameMap.map[newY][newX].Type == "Road") 
            {
                X = newX;
                Y = newY;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}