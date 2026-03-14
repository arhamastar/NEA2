using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace NEA1
{
    class Enemy
    {
        public int X { get; set; }
        public int Y { get; set; }
        private Player _player;
        private List<Tile> _currentPath;
        private int _pathIndex;
        private double _moveTimer;
        private const double MoveDelay = 0.5;

        public Enemy(int startX, int startY, Player player)
        {
            X = startX;
            Y = startY;
            _player = player;
            _currentPath = null;
            _pathIndex = 0;
            _moveTimer = 0;
        }

        public void Update(GameTime gameTime, Map gameMap)
        {
            _moveTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentPath == null || _pathIndex >= _currentPath.Count ||
                _currentPath[_currentPath.Count - 1].X != _player.X ||
                _currentPath[_currentPath.Count - 1].Y != _player.Y)
            {
                _currentPath = gameMap.FindPath(X, Y, _player.X, _player.Y);
                _pathIndex = 0;
            }

            if (_moveTimer >= MoveDelay)
            {
                _moveTimer = 0;

                if (_currentPath != null && _pathIndex < _currentPath.Count)
                {
                    var nextTile = _currentPath[_pathIndex];
                    X = nextTile.X;
                    Y = nextTile.Y;
                    _pathIndex++;
                }
            }

            if ((X == _player.X || Y == _player.Y) && HasClearPathToPlayer(gameMap))
            {
                _currentPath = gameMap.FindPath(X, Y, _player.X, _player.Y);
                _pathIndex = 0;
                _moveTimer = MoveDelay;
            }
        }

        private bool HasClearPathToPlayer(Map gameMap)
        {
            if (X == _player.X) 
            {
                int startY = Math.Min(Y, _player.Y);
                int endY = Math.Max(Y, _player.Y);
                for (int y = startY + 1; y < endY; y++)
                {
                    if (gameMap.map[y][X].Type != "Road") return false;
                }
                return true;
            }
            else if (Y == _player.Y)
            {
                int startX = Math.Min(X, _player.X);
                int endX = Math.Max(X, _player.X);
                for (int x = startX + 1; x < endX; x++)
                {
                    if (gameMap.map[Y][x].Type != "Road") return false;
                }
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            int tileSize = 50;
            Rectangle destinationRectangle = new Rectangle(
                X * tileSize,
                Y * tileSize,
                tileSize,
                tileSize
            );

            spriteBatch.Draw(texture, destinationRectangle, Color.DarkRed);
        }
    }
}