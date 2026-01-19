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
        // Parameter toegevoegd: sourceRect
        public static List<IGameObject> CreateLevel(int levelIndex, Texture2D blockTex, Rectangle blockSourceRect, Texture2D powerUpTex, Rectangle powerUpRect, Texture2D CoinTex, Rectangle coinRect, List<Rectangle> coinFrames, Texture2D greenEnemyTex, List<Rectangle> greenSlimeFrames, Texture2D purpleEnemyTex, List<Rectangle> purpleSlimeFrames)
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

                for (int i = 30; i < 50; i++)
                {
                    // Geef de sourceRect mee aan de constructor
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                }

                // Platformen
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(200, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(264, 300)));

                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(800, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1000, 250)));

                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1500, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1564, 236)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1300, 80)));


                // powerups 
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(216, 260)));
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1594, 220)));

                //coins 
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(296, 260), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(1320, 60), 2, coinFrames));

                //enemy
                // 1. DE SLIME (Patrol)
                var slimePatrol = new PatrolEnemyBehavior(1.5f);
                var slime = new Enemy(greenEnemyTex, new Vector2(350, 355), greenSlimeFrames, slimePatrol, 3f, objects);
                objects.Add(slime);

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