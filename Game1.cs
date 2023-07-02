using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Shooter
{
    public enum GameState
    {
        MainMenu,
        Game,
        GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteBatchFont;
        private SpriteFont _font;
        private Texture2D _backgroundTexture;
        private Texture2D _pigeonTexture;
        private Vector2 _pigeonPosition;
        private Rectangle _pigeonRectangle;

        private GameState _currentState;

        private Texture2D _crosshairTexture;
        private Vector2 _crosshairPosition;

        private Random _random = new Random();
        private int _score;

        private int _frame = 0;


        private const float PigeonSpeed = 5f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            _currentState = GameState.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatchFont = Content.Load<SpriteFont>("font");
            _font = Content.Load<SpriteFont>("menu");

            _backgroundTexture = Content.Load<Texture2D>("background");
            _pigeonTexture = Content.Load<Texture2D>("pigeon");
            _crosshairTexture = Content.Load<Texture2D>("crosshair");

            _pigeonPosition = new Vector2(400, 300);
            _pigeonRectangle = new Rectangle(_pigeonPosition.ToPoint(), _pigeonTexture.Bounds.Size);

            _crosshairPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_currentState)
            {
                case GameState.MainMenu:
                    UpdateMainMenu();
                    break;
                case GameState.Game:
                    UpdateGame();
                    break;
                case GameState.GameOver:
                    UpdateGameOver();
                    break;
            }



            base.Update(gameTime);
        }


        private void UpdateMainMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                // Commencez une nouvelle partie lorsque la touche Entrée est enfoncée
                StartNewGame();
            }
        }

        private void UpdateGame()
        {
            UpdatePigeon();
            UpdateCrosshair();
            CheckCollision();
        }

        private void UpdateGameOver()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                // Rejouez une nouvelle partie lorsque la touche Entrée est enfoncée
                StartNewGame();
            }
        }

        private void StartNewGame()
        {
            // Réinitialisez les paramètres du jeu pour commencer une nouvelle partie
            _currentState = GameState.Game;
        }

        private void UpdatePigeon()
        {
            // Déplace le pigeon de manière aléatoire
            if (_frame == 30)
            {
                _pigeonPosition = new Vector2(_random.Next(0, _graphics.PreferredBackBufferWidth - _pigeonTexture.Width), _random.Next(0, _graphics.PreferredBackBufferHeight - _pigeonTexture.Height));
                _pigeonRectangle = new Rectangle(_pigeonPosition.ToPoint(), _pigeonTexture.Bounds.Size);
                _frame = 0;
            }
            else
            {
                _frame++;
            }
        }

        private void UpdateCrosshair()
        {
            _crosshairPosition.X = Mouse.GetState().X - _crosshairTexture.Width / 2;
            _crosshairPosition.Y = Mouse.GetState().Y - _crosshairTexture.Height / 2;
        }

        private void CheckCollision()
        {

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _pigeonRectangle.Contains(Mouse.GetState().Position)
)
            {
                // Le pigeon a été touché !
                _score++;
                // Réinitialisez la position du pigeon
                // Détruisez le pigeon en le marquant comme mort
                UpdatePigeon();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (_currentState)
            {
                case GameState.MainMenu:
                    DrawMainMenu();
                    break;
                case GameState.Game:
                    DrawGame();
                    break;
                case GameState.GameOver:
                    DrawGameOver();
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawMainMenu()
        {
            // Dessinez le menu principal à l'écran

            _spriteBatch.DrawString(_font, "Appuyez sur Entree pour commencer", new Vector2(240, 300), Color.White);
        }

        private void DrawGame()
        {
            // Dessinez le jeu en cours...
            _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            _spriteBatch.Draw(_pigeonTexture, _pigeonPosition, Color.White);
            _spriteBatch.Draw(_crosshairTexture, _crosshairPosition, Color.White);

            _spriteBatch.DrawString(
                _spriteBatchFont,
                "Score: " + _score,
                new Vector2(10, 10),
                Color.White);

        }

        private void DrawGameOver()
        {
            // Dessinez l'écran de fin de partie
            _spriteBatch.DrawString(_font, "Game Over", new Vector2(340, 200), Color.White);
            _spriteBatch.DrawString(_font, "Appuyez sur Entree pour rejouer", new Vector2(240, 300), Color.White);
        }
    }
}