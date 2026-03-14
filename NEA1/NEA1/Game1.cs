using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;



namespace NEA1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Map gameMap;
        private Texture2D pixelTexture;

        private Player player;

        private Enemy enemy;

        private int _score = 100;
        private double _scoreTimer = 0;

        private Texture2D _sewerTexture;
        private Texture2D _brickTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameMap = new Map();
            base.Initialize();

            player = new Player(0, 0);
            enemy = new Enemy(3, 3, player);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });

        }
        

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            player.Update(Keyboard.GetState(), gameMap);

            if (player.X == 14 && player.Y == 14)
            {
                System.Diagnostics.Debug.WriteLine("Level Complete! Combat level starting...");
            }

            enemy.Update(gameTime, gameMap);
            
            if (player.X == enemy.X && player.Y == enemy.Y)
            {
                System.Diagnostics.Debug.WriteLine("Caught! Game over!");
            }

            _scoreTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_scoreTimer >= 1.0)
            {
                _score--; 
                _scoreTimer = 0; 
                System.Diagnostics.Debug.WriteLine("Time penalty! Score: " + _score);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            var path = gameMap.FindPath(enemy.X, enemy.Y, player.X, player.Y);

            gameMap.Draw(_spriteBatch, _sewerTexture, _brickTexture, _sewerTexture, path);


            player.Draw(_spriteBatch);
            enemy.Draw(_spriteBatch, pixelTexture);

            Rectangle scoreBackground = new Rectangle(5, 5, 100, 30);
            _spriteBatch.Draw(pixelTexture, scoreBackground, new Color(0, 0, 0, 128));

            int scoreSquares = _score / 10;
            for (int i = 0; i < scoreSquares && i < 10; i++) 
            {
                Rectangle scoreSquare = new Rectangle(10 + (i * 8), 10, 6, 20);
                Color scoreColor = Color.Lerp(Color.Red, Color.Green, _score / 100f); 
                _spriteBatch.Draw(pixelTexture, scoreSquare, scoreColor);
            }

            string scoreText = _score.ToString();

            _spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
