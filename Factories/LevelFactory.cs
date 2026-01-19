using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Behaviors;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;

namespace SofEngeneering_project.Factories
{
    public static class LevelFactory
    {
        // Blokgrootte (standaard 64px)
        private const int BS = 64;
        // De hoogte van de vloer (Y-positie)
        private const int FLOOR_Y = 420;
        // Parameter toegevoegd: sourceRect
        public static List<IGameObject> CreateLevel(int levelIndex, Texture2D blockTex, Rectangle blockSourceRect, Texture2D powerUpTex, Rectangle powerUpRect, Texture2D CoinTex, Rectangle coinRect, List<Rectangle> coinFrames, Texture2D greenEnemyTex, List<Rectangle> greenSlimeFrames, Texture2D purpleEnemyTex, List<Rectangle> purpleSlimeFrames, Texture2D SurikenTex, Rectangle SurikenRect)
        {
            var objects = new List<IGameObject>();





            //level 1
            if(levelIndex == 1)
            {
                // Vloer
                for (int i = 0; i < 25; i++)
                {
                    // Geef de sourceRect mee aan de constructor
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                }

                for (int i = 30; i < 40; i++)
                {
                    // Geef de sourceRect mee aan de constructor
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                }

                // Platformen voor de jump
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(200, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(264, 300)));

                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(800, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1000, 250)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1200, 175)));

                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1500, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1564, 236)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1300, 80)));

                //platformen na de jump
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2150, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2250, 200)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2150, 100)));

                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2560, 336)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2624, 272)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2688, 208)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2752, 144)));

                for (int i = 43; i < 55; i++)
                {
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2816, 144)));
                }

                // powerups 
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(260, 260)));
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1594, 220)));


                //coins 
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(830, 260), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(1320, 60), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(2170, 80), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(2860, 100), 2, coinFrames));


                //patrol slime
                var slimePatrol = new PatrolEnemyBehavior(1.5f);
                var slime = new Enemy(greenEnemyTex, new Vector2(350, 355), greenSlimeFrames, slimePatrol, 3f, objects);
                objects.Add(slime);

                //trap
                var trapBehavior1 = new TrapBehavior(2568, 2650,2);
                objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(2568, 300), trapBehavior1));
                var trapBehavior2 = new TrapBehavior(2632, 2714, 2);
                objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(2714, 236), trapBehavior2));

            }

            // LEvel 2
            if(levelIndex == 2)
            {

                // Vloer
                for (int i = -10; i < 25; i++)
                {
                    // Geef de sourceRect mee aan de constructor
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                }

                for (int i = 30; i < 50; i++)
                {
                    // Geef de sourceRect mee aan de constructor
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                }

                //coins
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(296, 260), 2, coinFrames));

                //Big enemy 
                var chaseLogic = new ConstantMoveBehavior(0.8f);
                float megaScale = 28f;
                Vector2 startPos = new Vector2(-500, -20);
                var megaSlime = new Enemy(purpleEnemyTex, startPos, purpleSlimeFrames, chaseLogic, megaScale, new List<IGameObject>());

                objects.Add(megaSlime);



            }




            return objects;
        }
    }
}