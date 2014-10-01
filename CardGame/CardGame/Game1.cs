using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace CardGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont font2;
        Texture2D backgroundTexture;
        Texture2D menuTexture;
        int screenWidth = 500;
        int screenHeight = 600;
        CardSprite[] deck = new CardSprite[52];
        List<CardSprite> shuffledDeck = new List<CardSprite>();
        Square[] squares = new Square[16];
        MouseState currentMouseState;
        MouseState previousMouseState;
        List<int> csl = new List<int>();
        Stack<CardSprite> unplayedDeck = new Stack<CardSprite>();
        CardSprite cardBack;
        int numberOfOpenSquares = 16;
        enum GameState { Menu, Game, Credits}
        Boolean drawingPhase;
        Boolean youWin;
        GameState gameState;
        Rectangle playButton;
        Rectangle continueButton;
        CardSprite previousCard;
        int numberOfHighlightedCards;
        int numberOfPairs;
        Boolean pairMade;
        int numberOfCards;
        int winCount;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 500;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            this.IsMouseVisible = true;
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menuTexture = Content.Load<Texture2D>(@"menu_v1");
            backgroundTexture = Content.Load<Texture2D>(@"bg_v2");
            font = Content.Load<SpriteFont>("SpriteFont1");
            font2 = Content.Load<SpriteFont>("SpriteFont2");
            gameState = new GameState();
            gameState = GameState.Menu;
            playButton = new Rectangle(301, 24, 180, 86);
            continueButton = new Rectangle(75, 530, 400, 100);
            drawingPhase = true;
            numberOfHighlightedCards = 0;
            numberOfPairs = 0;
            numberOfCards = 52;
            pairMade = false;
            winCount = 0;
            int count = 0;
            int x = 0;
            int y = 0;
            int xLoc = 9999;
            int yLoc = 500;
            int cardNumber = 1;
            youWin = true;
            //card back
            cardBack = new CardSprite(Content.Load<Texture2D>(@"cards"),
                        0, 469, 81, 117, 1, false, count, 0);
            cardBack.X = 10;
            cardBack.Y = 10;
            previousCard = cardBack;
            //create card objects
            for (int i = 0; i < 4; i++)
            {
                if (i == 2)
                    y = y + 1;
                
                for (int p = 0; p < 13; p++)
                {
                    cardNumber++;
                    if (cardNumber == 14)
                        cardNumber = 1;
                    Debug.WriteLine(cardNumber);
                    deck[count] = new CardSprite(Content.Load<Texture2D>(@"cards"),
                        x, y, 81, 117, 1, true, count, cardNumber);
                    deck[count].X = xLoc;
                    deck[count].Y = yLoc;
                    xLoc = xLoc + 20;
                    
                    count++;
                    x = x + 81;
                }
                cardNumber = 1;
                xLoc = 9999;
                x = 0;
                y = y + 117;
                
            }
            //create square objects
            int xSquare = 133;
            int ySquare = 10;
            int xRow = 1;
            int yColumn = 1;
            int squareCount = 0;

            for (int p = 0; p < 4; p++)
            {
                if (p == 0)
                    ySquare = ySquare + 1;
                if (p == 1)
                    ySquare = ySquare - 1;
                for (int i = 0; i < 4; i++)
                {
                    squares[squareCount] = new Square(new Rectangle(xSquare, ySquare, 81, 117), xRow, yColumn, 0, true);
                    squareCount++;
                    xRow++;
                    xSquare = xSquare + 91;
                }
                xSquare = 133;
                ySquare = ySquare + 128;
                xRow = 1;
                yColumn++;
            }

            squares[0].cardKind = 13;
            squares[1].cardKind = 12;
            squares[2].cardKind = 12;
            squares[3].cardKind = 13;

            squares[4].cardKind = 11;
            squares[7].cardKind = 11;

            squares[8].cardKind = 11;
            squares[11].cardKind = 11;

            squares[12].cardKind = 13;
            squares[13].cardKind = 12;
            squares[14].cardKind = 12;
            squares[15].cardKind = 13;
            
            for (int i = 0; i < 52; i++)
            {
                shuffledDeck.Add(deck[i]);
            }
            Shuffle.ShuffleCards<CardSprite>(shuffledDeck);


            shuffledDeck.Reverse();
            foreach(CardSprite c in shuffledDeck)
            {
                unplayedDeck.Push(c);
            }
            
            NextCard();
            // TODO: use this.Content to load your game content here
        }
        public void NextCard()
        {
            if (unplayedDeck.Count > 0)
            {
                DrawCard(unplayedDeck.Pop());
            }
            else
            {
                Debug.WriteLine("Stack empty");
            }
        }
        public void DrawCard(CardSprite cs)
        {
            cs.X = 10;
            cs.Y = 10;
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            //mouse
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            if (gameState == GameState.Game)
            {
                winCount = 0;
                foreach (Square s in squares)
                {
                    if (s.cardKind != 0)
                    {
                        if (s.cardKind == s.SquareCardNumber)
                        {
                            winCount++;
                        }
                    }
                }
                if (winCount == 12)
                {
                    gameState = GameState.Credits;
                }
            }
            if (gameState == GameState.Game && drawingPhase == false)
            {
                //CHECK WIN CONDITIONS HERE
                
                numberOfPairs = 0;
                for (int i = 0; i < 15; i++)
                {
                    numberOfPairs += AddUpCards(i);
                }
                
                Debug.WriteLine(numberOfPairs + " PAIRS");
                //player loses
                if (numberOfPairs == 0)
                {
                    youWin = false;
                    gameState = GameState.Credits;
                }
            }
            if (drawingPhase == true)
            {
                pairMade = false;
            }

            
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                //NEW GAME
                if (gameState == GameState.Credits)
                {
                    if (continueButton.Contains(new Point((int)currentMouseState.X, (int)currentMouseState.Y)))
                    {
                        drawingPhase = true;
                        pairMade = false;
                        numberOfOpenSquares = 16;
                        foreach (CardSprite c in deck)
                        {
                            c.X = 9999;
                            c.IsMoveable = true;
                            c.cardTexture = Content.Load<Texture2D>(@"cards");
                            c.Highlighted = false;
                                
                        }
                        foreach (Square s in squares)
                        {
                            s.open = true;
                            
                        }
                        numberOfCards = 52;
                        gameState = GameState.Game;
                        shuffledDeck.Clear();
                        unplayedDeck.Clear();
                        for (int i = 0; i < 52; i++)
                        {
                            shuffledDeck.Add(deck[i]);
                        }
                        Shuffle.ShuffleCards<CardSprite>(shuffledDeck);


                        shuffledDeck.Reverse();
                        foreach (CardSprite c in shuffledDeck)
                        {
                            unplayedDeck.Push(c);
                        }
                        youWin = true;
                        NextCard();
                    }
                }
                if (gameState == GameState.Menu)
                {
                    if (playButton.Contains(new Point((int)currentMouseState.X, (int)currentMouseState.Y)))
                    {
                        gameState = GameState.Game;
                    }
                }
                else if (gameState == GameState.Game)
                {
                    var cardSelectedQuery =
                        from CardSprite cs in deck
                        where (currentMouseState.X > cs.X && currentMouseState.X < cs.X + 81 &&
                        currentMouseState.Y > cs.Y && currentMouseState.Y < cs.Y + 117)
                        select cs.cardInd;
                    if (csl.Count > 0)
                    {

                        if (deck[csl[0]].IsMoveable == true)
                        {
                            deck[csl[0]].X = currentMouseState.X - 40;
                            deck[csl[0]].Y = currentMouseState.Y - 40;
                        }

                    }

                    foreach (int c in cardSelectedQuery)
                    {
                        csl.Add(c);
                    }

                    //highlight the cards
                    if (drawingPhase == false)
                    {
                        //CLICK CONTINUE HERE
                        if (drawingPhase == false && pairMade == true)
                        {
                            if (continueButton.Contains(new Point((int)currentMouseState.X, (int)currentMouseState.Y)))
                            {
                                drawingPhase = true;
                                NextCard();
                            }
                        }
                        
                            if (csl.Count > 0)
                            {
                                if (deck[csl[0]].CardNumber < 11)
                                {
                                    if (previousMouseState.LeftButton == ButtonState.Released
                                        && currentMouseState.LeftButton == ButtonState.Pressed)
                                    {
                                        if (deck[csl[0]].Highlighted == false)
                                        {
                                            if (numberOfHighlightedCards == 0)
                                            {
                                                deck[csl[0]].cardTexture = Content.Load<Texture2D>(@"cardsHighlighted_V3");
                                                deck[csl[0]].Highlighted = true;
                                                previousCard = deck[csl[0]];
                                                numberOfHighlightedCards++;
                                            }
                                            if (numberOfHighlightedCards == 1)
                                            {
                                                if (deck[csl[0]].CardNumber + previousCard.CardNumber == 11)
                                                {
                                                    deck[csl[0]].cardTexture = Content.Load<Texture2D>(@"cardsHighlighted_V3");
                                                    deck[csl[0]].Highlighted = true;
                                                    numberOfHighlightedCards++;
                                                }
                                            }

                                        }
                                        else if (deck[csl[0]].Highlighted == true)
                                        {
                                            deck[csl[0]].cardTexture = Content.Load<Texture2D>(@"cards");
                                            deck[csl[0]].Highlighted = false;
                                            numberOfHighlightedCards--;
                                        }
                                        //MAKE SQUARE OPEN
                                        if (numberOfHighlightedCards == 2)
                                        {
                                            foreach (CardSprite c in deck)
                                            {
                                                if (c.Highlighted == true)
                                                {
                                                    c.X = 9999;
                                                    c.CurrentSquare.open = true;
                                                }
                                            }
                                            pairMade = true;
                                            numberOfHighlightedCards = 0;
                                            numberOfOpenSquares = numberOfOpenSquares + 2;
                                        }
                                    }

                                }
                            }
                    }
                }
            }
            

            if (currentMouseState.LeftButton == ButtonState.Released)
            {
                if (gameState == GameState.Game)
                {
                    foreach (Square s in squares)
                    {

                        if (csl.Count > 0)
                        {
                            if (drawingPhase == true)
                            {
                                if (deck[csl[0]].IsMoveable == true)
                                {
                                    if (s.squareDim.Contains(new Point((int)currentMouseState.X, (int)currentMouseState.Y)))
                                    {



                                        //card locked in square
                                        if (s.open == true)
                                        {
                                            deck[csl[0]].X = s.squareDim.X;
                                            deck[csl[0]].Y = s.squareDim.Y;
                                            deck[csl[0]].IsMoveable = false;
                                            deck[csl[0]].CurrentSquare = s;
                                            numberOfCards--;
                                            s.SquareCardNumber = deck[csl[0]].CardNumber;
                                            if (numberOfOpenSquares > 1)
                                            {
                                                NextCard();
                                            }
                                            s.open = false;
                                            numberOfOpenSquares--;
                                            //face card in wrong slot
                                            
                                            //is card face card?
                                            if (deck[csl[0]].CardNumber > 10)
                                            {
                                                //wrong spot; player loses
                                                if (deck[csl[0]].CardNumber != s.cardKind)
                                                {
                                                    gameState = GameState.Credits;
                                                    youWin = false;
                                                }
                                                
                                            }
                                            if (numberOfOpenSquares == 0)
                                            {
                                                drawingPhase = false;

                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        deck[csl[0]].X = 10;
                                        deck[csl[0]].Y = 10;

                                    }

                                }
                            }

                            //highlight cards
                            else if (drawingPhase == false)
                            {

                            }
                        }
                    }

                    csl.Clear();
                }
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (gameState == GameState.Menu)
            {
                spriteBatch.Begin();
                DrawMenu();
                spriteBatch.End();
            }
            else if (gameState == GameState.Game)
            {
                // TODO: Add your drawing code here
                cardBack.Update(gameTime);
                
                foreach (CardSprite c in deck)
                {
                    c.Update(gameTime);
                }
                spriteBatch.Begin();

                DrawScenery();
                cardBack.Draw(spriteBatch, 0, 0, false);
                DrawNumberOfCards();
                for (int i = 51; i > 0; i--)
                {
                    shuffledDeck[i].Draw(spriteBatch, 0, 0, false);
                }

                //pairing phase
                if (drawingPhase == false && pairMade == true)
                {
                    DrawContinue();
                }

                spriteBatch.End();
                base.Draw(gameTime);

                
            }
            else if (gameState == GameState.Credits)
            {
                spriteBatch.Begin();
                DrawScenery();
                cardBack.Draw(spriteBatch, 0, 0, false);

                for (int i = 51; i > 0; i--)
                {
                    shuffledDeck[i].Draw(spriteBatch, 0, 0, false);
                }
                DrawNumberOfCards();
                DrawEnding();
                spriteBatch.End();
            }
        }
        private void DrawEnding()
        {
            string message;
            if (youWin == true)
            {
                message = "You win! \nClick here to play again.";
            }
            else
            {
                message = "You lose. \nClick here to play again.";
            }
            spriteBatch.DrawString(font, message, new Vector2(75, 530), Color.White);
        }
        private void DrawNumberOfCards()
        {
            spriteBatch.DrawString(font2, numberOfCards + " Cards", new Vector2(10, 135), Color.White);
        }
        private void DrawScenery()
        {

            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
        }
        private void DrawMenu()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(menuTexture, screenRectangle, Color.White);
            
        }
        private void DrawContinue()
        {
            spriteBatch.DrawString(font, "Click here to continue", new Vector2(75, 530), Color.White);
        }
        private int AddUpCards(int x)
        {
            int count = 0;
            for (int i = 0; i < 15; i++)
            {
                if (i == x)
                {
                }
                else if(squares[x].SquareCardNumber + squares[i].SquareCardNumber == 11)
                {
                    count++;
                }
            }
                return count;
        }
    }
}
