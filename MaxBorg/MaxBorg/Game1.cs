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

namespace MaxBorg
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D turret;
        Texture2D tubeDown;
        Texture2D tubeLeft;
        Texture2D tubeRight;
        Texture2D tubeUp;
        //Texture2D torpedoDown;
        Texture2D badguys;
        Texture2D torpedoUp;

        Rectangle turretRect;
        Rectangle tubeDownRect;
        Rectangle tubeLeftRect;
        Rectangle tubeRightRect;
        Rectangle tubeUpRect;
        Rectangle torpedoUpRect;
        Rectangle badGuyTorpedo;

        KeyboardState oldKB;

        Color colorUp;
        Color colorDown;
        Color colorLeft;
        Color colorRight;

        int xPos;
        int yPos;

        int badX = 350;
        int badY = 0;
        String directionOfBad;
        int timerForBad = 0;

        bool isFired;
        bool isBadFired;
        String facingDirection;
        String goingDirection;
        Rectangle badGuyRectangle;
        Texture2D badGuyTorpedoTexture;
        int tickForBadGuy = 240;

        int MJ = 100;
        float storedJ = -1;

        int torpWidth = 50;
        int torpLength = 50;

        int sizeToAdd = 0;

        int tickForCharge = 0;

        int xBadTorp = 400;
        int yBadTorp = 30;

        int xToBreakAt;
        int yToBreakAt;

        float propelJ;
        MouseState oldMouse;
        SpriteFont font1;

        Texture2D redBox;
        Texture2D greenBox;
        Rectangle propelRectangle;
        Rectangle explodeRectangle;

        Rectangle propelFilledRectangle;
        Rectangle explodeFilledRectangle;
        Rectangle totalEnergyRectangle;
        Rectangle totalEnergyFilledRectangle;

        int numberForLazer = 0;

        Texture2D yellowBeam;
        Texture2D blueBeam;
        Texture2D greeenBeam;
        Texture2D redBeam;
        Rectangle laserRectangle;
        Boolean laserFired = false;

        SoundEffect laserSound;
        SoundEffect turretSound;

        int delay = 180;
        Boolean isDelayed = false;
        GamePadState oldPad;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            oldMouse = Mouse.GetState();
            turretRect = new Rectangle(230, 160, 300, 100);
            //
            tubeLeftRect = new Rectangle(250, 190, 100, 50);
            tubeRightRect = new Rectangle(460, 196, 100, 50);
            tubeUpRect = new Rectangle(380, 110, 50, 100);
            tubeDownRect = new Rectangle(380, 255, 50, 100);
            badGuyRectangle = new Rectangle(0, 0, 100, 93);
            badGuyTorpedo = new Rectangle(xBadTorp, yBadTorp, 50, 50);
            propelRectangle = new Rectangle(40, 40, 126, 35);
            explodeRectangle = new Rectangle(240, 40, 126, 35);
            propelFilledRectangle = new Rectangle(40, 40, 0, 35);
            explodeFilledRectangle = new Rectangle(240, 40, 0, 35);

            totalEnergyRectangle = new Rectangle(440, 40, 126, 35);
            totalEnergyFilledRectangle = new Rectangle(440, 40, 0, 35);
            xPos = 380;
            yPos = 100;


            torpedoUpRect = new Rectangle(xPos, yPos, torpWidth, torpLength);

            facingDirection = "up";

            colorLeft = Color.Red;
            colorRight = Color.Red;
            colorDown = Color.Red;
            colorUp = Color.Green;


            isFired = false;
            isBadFired = false;
            sizeToAdd = 0;
            torpLength = 50;
            torpWidth = 50;

            Boolean laserShot = false;
            laserRectangle = new Rectangle(150,0,80,400);
            oldPad = GamePad.GetState(PlayerIndex.One);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            turret = this.Content.Load<Texture2D>("Turret");
            tubeLeft = this.Content.Load<Texture2D>("Launch Tube Left");
            tubeRight = this.Content.Load<Texture2D>("Launch Tube Right");
            tubeUp = this.Content.Load<Texture2D>("Launch Tube Up");
            tubeDown = this.Content.Load<Texture2D>("Launch Tube Down");
            badGuyTorpedoTexture = this.Content.Load<Texture2D>("Torpedo Down");
            torpedoUp = this.Content.Load<Texture2D>("Torpedo Up");
            badguys = this.Content.Load<Texture2D>("badguyship");
            redBox = this.Content.Load<Texture2D>("redsquare");
            greenBox = this.Content.Load<Texture2D>("greensquare");
            font1 = this.Content.Load<SpriteFont>("SpriteFont1");

            yellowBeam = this.Content.Load<Texture2D>("yellowbeam");
            blueBeam = this.Content.Load<Texture2D>("bluebeam");
            greeenBeam = this.Content.Load<Texture2D>("greenbeam");
            redBeam = this.Content.Load<Texture2D>("redbeam");

            laserSound = this.Content.Load<SoundEffect>("fire3");
            turretSound = this.Content.Load<SoundEffect>("Gun+Luger");
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
            //KeyboardState kb = Keyboard.GetState();
            //MouseState currentMouseState = Mouse.GetState();
            GamePadState pad = GamePad.GetState(PlayerIndex.One);
            propelFilledRectangle = new Rectangle(40, 40, (int)propelJ * 14, 35);
            explodeFilledRectangle = new Rectangle(240, 40, (int)storedJ * 14, 35);
            totalEnergyFilledRectangle = new Rectangle(440, 40, (int)(MJ * 1.26), 35);
            if (isDelayed) {
                delay--;
                if (delay == 0) {
                    isDelayed = false;
                    delay = 180;
                }
            }
            if (isBadFired)
            {
                if (directionOfBad.Equals("up"))
                {
                    yBadTorp += 6;
                }
                else if (directionOfBad.Equals("down"))
                {
                    yBadTorp -= 6;
                }
                else if (directionOfBad.Equals("left"))
                {
                    xBadTorp += 6;
                }
                else if (directionOfBad.Equals("right"))
                {
                    xBadTorp -= 6;
                }
            }
            if (pad.Buttons.A == ButtonState.Pressed && oldPad.Buttons.A == ButtonState.Released) {
                numberForLazer = 25;
            }else if (pad.Buttons.B == ButtonState.Pressed && oldPad.Buttons.B == ButtonState.Released)
            {
                numberForLazer = 50;
            }else if (pad.Buttons.X == ButtonState.Pressed && oldPad.Buttons.X == ButtonState.Released)
            {
                numberForLazer = 75;
            }else if (pad.Buttons.Y == ButtonState.Pressed && oldPad.Buttons.Y == ButtonState.Released)
            {
                numberForLazer = 100;
            }
            timerForBad++;
            if (timerForBad == tickForBadGuy)
            {
                Random random = new Random();
                int direction = random.Next(4);
                if (direction == 0)
                {
                    badX = 350;
                    badY = random.Next(30);
                    directionOfBad = "up";
                    badGuyTorpedoTexture = this.Content.Load<Texture2D>("Torpedo Down");
                }
                else if (direction == 1)
                {
                    badX = random.Next(150) + 520;
                    badY = 180;
                    directionOfBad = "right";
                    badGuyTorpedoTexture = this.Content.Load<Texture2D>("Torpedo Left");
                }
                else if (direction == 2)
                {
                    badX = 350;
                    badY = random.Next(200) + 300;
                    directionOfBad = "down";
                    badGuyTorpedoTexture = this.Content.Load<Texture2D>("Torpedo Up");
                }
                else if (direction == 3)
                {
                    badY = 180;
                    badX = random.Next(180);
                    directionOfBad = "left";
                    badGuyTorpedoTexture = this.Content.Load<Texture2D>("Torpedo Right");
                }
                xBadTorp = badX + 50;
                yBadTorp = badY + 30;

                tickForBadGuy = random.Next(181) + 240;
                isBadFired = true;
                turretSound.Play();
                timerForBad = 0;
            }
            if (xBadTorp >= 270 && xBadTorp <= 490 && yBadTorp >= 170 && yBadTorp <= 250)
            {
                isBadFired = false;
                xBadTorp = badX + 50;
                yBadTorp = badY + 30;
            }
            badGuyTorpedo = new Rectangle(xBadTorp, yBadTorp, 50, 50);
            badGuyRectangle = new Rectangle(badX, badY, 100, 93);
            torpedoUpRect = new Rectangle(xPos, yPos, torpWidth, torpLength);

            tickForCharge++;
            if (tickForCharge == 60)
            {
                MJ += 3;
                if (MJ > 100)
                {
                    MJ = 100;
                }
                tickForCharge = 0;
            }

                storedJ += pad.ThumbSticks.Left.Y;
                propelJ += pad.ThumbSticks.Left.X;
            if (propelJ > 9 ) { propelJ = 9; }
            if (propelJ < 0) { propelJ = 0; }
            if (storedJ > 9) { storedJ = 9; }
            if (storedJ < 0) { storedJ = 0; }
            // }
            torpedoPosUpdate(pad,oldPad);
            if (isFired)
            {
                if (goingDirection.Equals("up"))
                {
                    torpLength = 50 + sizeToAdd;
                    yPos -= 6;
                    if (yPos <= yToBreakAt)
                    {
                        isFired = false;
                        propelJ = -1;
                        storedJ = -1;
                        
                        resetTorpedo();
                    }
                }
                else if (goingDirection.Equals("down"))
                {
                    torpLength = 50 + sizeToAdd;
                    yPos += 6;
                    if (yPos >= yToBreakAt)
                    {
                        isFired = false;
                        storedJ = -1;
                        propelJ = -1;
                        resetTorpedo();
                    }
                }
                else if (goingDirection.Equals("left"))
                {
                    torpWidth = 50 + sizeToAdd;
                    xPos -= 6;
                    if (xPos <= xToBreakAt)
                    {
                        isFired = false;
                        storedJ = -1;
                        propelJ = -1;
                        resetTorpedo();
                    }
                }
                else if (goingDirection.Equals("right"))
                {
                    torpWidth = 50 + sizeToAdd;
                    xPos += 6;
                    if (xPos >= xToBreakAt)
                    {
                        isFired = false;
                        storedJ = -1;
                        propelJ = -1;
                        resetTorpedo();
                    }
                }
                // torpedoUpRect = new Rectangle(xPos, yPos, torpWidth, torpLength);
                /*if (yPos < -50 || yPos > 480 || xPos < -50 || xPos > 800)
                {
                    isFired = false;
                    storedJ = -1;
                    resetTorpedo();
                }*/

            }
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            oldPad = pad;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(turret, turretRect, Color.White);
            spriteBatch.Draw(tubeLeft, tubeLeftRect, colorLeft);
            spriteBatch.Draw(tubeRight, tubeRightRect, colorRight);
            spriteBatch.Draw(tubeUp, tubeUpRect, colorUp);
            spriteBatch.Draw(tubeDown, tubeDownRect, colorDown);
            spriteBatch.Draw(badguys, badGuyRectangle, Color.White);
            spriteBatch.Draw(badGuyTorpedoTexture, badGuyTorpedo, Color.White);
            spriteBatch.Draw(torpedoUp, torpedoUpRect, Color.White);
            spriteBatch.Draw(redBox, propelRectangle, Color.White);
            spriteBatch.Draw(redBox, explodeRectangle, Color.White);
            spriteBatch.Draw(greenBox, propelFilledRectangle, Color.White);
            spriteBatch.Draw(greenBox, explodeFilledRectangle, Color.White);
            spriteBatch.Draw(redBox, totalEnergyRectangle, Color.White);
            spriteBatch.Draw(greenBox, totalEnergyFilledRectangle, Color.White);

            spriteBatch.DrawString(font1, "Propel Energy", new Vector2(40, 10), Color.Black);
            spriteBatch.DrawString(font1, "Explode Energy", new Vector2(230, 10), Color.Black);

            if (laserFired) {
                if (numberForLazer == 25)
                {
                    spriteBatch.Draw(greeenBeam, laserRectangle, Color.White);
                }
                else if (numberForLazer == 50)
                {
                    spriteBatch.Draw(redBeam, laserRectangle, Color.White);
                }
                else if (numberForLazer == 75)
                {
                    spriteBatch.Draw(blueBeam, laserRectangle, Color.White);
                }
                else if (numberForLazer == 100) {
                    spriteBatch.Draw(yellowBeam, laserRectangle, Color.White);
                }

            }
            spriteBatch.DrawString(font1, "MJ:" + MJ, new Vector2(510, 100), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void torpedoPosUpdate(GamePadState pad,GamePadState oldPad)
        {
            if (pad.Triggers.Right > 0) {
                laserFired = true;
                laserSound.Play();
            }
            if (pad.Triggers.Right == 0) {
                laserFired = false;
            }
            if (pad.DPad.Up == ButtonState.Pressed)
            {
                colorUp = Color.Green;
                colorDown = Color.Red;
                colorLeft = Color.Red;
                colorRight = Color.Red;
                laserRectangle = new Rectangle(365, -50, 80, 250);
                if (!isFired)
                {
                    torpedoUp = this.Content.Load<Texture2D>("Torpedo Up");
                }
                updateTorpedo(isFired, 380, 100, "up");
            }
            if (pad.DPad.Right == ButtonState.Pressed)
            {
                colorRight = Color.Green;
                colorDown = Color.Red;
                colorLeft = Color.Red;
                colorUp = Color.Red;
                laserRectangle = new Rectangle(470, 180, 380, 80);
                if (!isFired)
                {
                    torpedoUp = this.Content.Load<Texture2D>("Torpedo Right");
                }
                updateTorpedo(isFired, 510, 196, "right");
            }

            if (pad.DPad.Down == ButtonState.Pressed)
            {
                colorDown = Color.Green;
                colorUp = Color.Red;
                colorLeft = Color.Red;
                colorRight = Color.Red;
                laserRectangle = new Rectangle(365, 290, 80, 400);
                if (!isFired)
                {
                    torpedoUp = this.Content.Load<Texture2D>("Torpedo Down");
                }
                updateTorpedo(isFired, 380, 325, "down");
            }
            if (pad.DPad.Left == ButtonState.Pressed)
            {
                colorLeft = Color.Green;
                colorDown = Color.Red;
                colorUp = Color.Red;
                colorRight = Color.Red;
                laserRectangle = new Rectangle(-60, 170, 400, 80);
                if (!isFired)
                {
                    torpedoUp = this.Content.Load<Texture2D>("Torpedo Left");
                }
                updateTorpedo(isFired, 250, 190, "left");
            }
            if (pad.Triggers.Left > 0 && oldPad.Triggers.Left == 0 && !isFired)
            {
                isFired = true;
                turretSound.Play();
                goingDirection = facingDirection;
                if (storedJ == -1) { storedJ = 0; }
                if (storedJ > MJ)
                {
                    storedJ = MJ;
                }
                MJ -= (int)storedJ;
                sizeToAdd = 10 * (int)storedJ;
                if (propelJ == -1) { storedJ = 0; }
                if (propelJ > MJ)
                {
                    propelJ = MJ;
                }
                MJ -= (int)propelJ;
                if (goingDirection.Equals("up"))
                {
                    yToBreakAt = yPos - 14 * (int)propelJ;
                }
                else if (goingDirection.Equals("down"))
                {
                    yToBreakAt = yPos + 14 * (int)propelJ;
                }
                else if (goingDirection.Equals("left"))
                {
                    xToBreakAt = xPos - 32 * (int)propelJ;
                }
                else if (goingDirection.Equals("right"))
                {
                    xToBreakAt = xPos + 24 * (int)propelJ;
                }
            }

        }
        public void updateTorpedo(Boolean hasBeenFired, int x, int y, String direction)
        {
            if (!hasBeenFired)
            {
                xPos = x;
                yPos = y;
            }
            torpedoUpRect = new Rectangle(xPos, yPos, torpWidth, torpLength);
            facingDirection = direction;
        }
        public void resetTorpedo()
        {
            torpWidth = 50;
            torpLength = 50;
            if (facingDirection.Equals("right"))
            {
                xPos = 510;
                yPos = 196;
                torpedoUp = this.Content.Load<Texture2D>("Torpedo Right");
            }
            else if (facingDirection.Equals("left"))
            {
                xPos = 250;
                yPos = 190;
                torpedoUp = this.Content.Load<Texture2D>("Torpedo Left");
            }
            else if (facingDirection.Equals("up"))
            {
                xPos = 380;
                yPos = 100;
                torpedoUp = this.Content.Load<Texture2D>("Torpedo Up");
            }
            else if (facingDirection.Equals("down"))
            {
                xPos = 380;
                yPos = 325;
                torpedoUp = this.Content.Load<Texture2D>("Torpedo Down");
            }

        }
     
    }
}
