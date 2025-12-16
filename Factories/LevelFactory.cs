using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;

namespace SofEngeneering_project.Factories
{
    public static class LevelFactory
    {
        // Parameter toegevoegd: sourceRect
        public static List<IGameObject> CreateLevel(int levelIndex, Texture2D blockTex, Rectangle blockSourceRect, Texture2D powerUpTex, Rectangle powerUpRect, Texture2D CoinTex, Rectangle coinRect, List<Rectangle> coinFrames)
        {
            var objects = new List<IGameObject>();

            // Vloer = in beide levels
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

            //level 1
            if(levelIndex == 1)
            {
                // Platformen
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(200, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(264, 300)));

                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(800, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1000, 250)));

                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1500, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1564, 236)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1300, 80)));


                // Voeg een appel toe op een platform (bijv. op X=264, Y=250)
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(216, 260)));
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1594, 220)));

                //coins toevoegen
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(296, 260), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(1320, 60), 2, coinFrames));
            }

            // LEvel 2
            if(levelIndex == 2)
            {
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(296, 260), 2, coinFrames));

            }




            return objects;
        }
    }
}