using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Behaviors;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Factories
{
    public static class LevelFactory
    {
        // Blokgrootte (standaard 64px)
        private const int BS = 64;

        public static List<IGameObject> CreateLevel(int levelIndex,Texture2D blockTex, Rectangle blockSourceRect,Texture2D powerUpTex, Rectangle powerUpRect,Texture2D CoinTex, Rectangle coinRect, List<Rectangle> coinFrames,Texture2D greenEnemyTex, List<Rectangle> greenSlimeFrames,Texture2D purpleEnemyTex, List<Rectangle> purpleSlimeFrames,Texture2D SurikenTex, Rectangle SurikenRect)
        {
            var objects = new List<IGameObject>();

            // =============================================================
            // LEVEL 1
            // =============================================================
            if (levelIndex == 1)
            {
                // Vloer
                for (int i = 0; i < 25; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                for (int i = 30; i < 40; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));

                // Platformen voor de jump
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(200, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(264, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(800, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1000, 250)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1200, 175)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1500, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1564, 236)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1300, 80)));

                // Platformen na de jump
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2150, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2250, 200)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2150, 100)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2560, 336)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2624, 272)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2688, 208)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2752, 144)));

                for (int i = 43; i < 55; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2816, 144)));

                // Powerups 
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(260, 260)));
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1594, 200)));

                // Coins 
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(830, 260), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(1320, 60), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(2170, 80), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(2860, 100), 2, coinFrames));

                // Patrol slime
                var slimePatrol = new PatrolEnemyBehavior(1.5f);
                var slime = new Enemy(greenEnemyTex, new Vector2(350, 355), greenSlimeFrames, slimePatrol, 3f, objects);
                objects.Add(slime);

                // Trap
                var trapBehavior1 = new TrapBehavior(2568, 2650, 2);
                objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(2568, 300), trapBehavior1));
                var trapBehavior2 = new TrapBehavior(2632, 2714, 2);
                objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(2714, 236), trapBehavior2));
            }

            // =============================================================
            // LEVEL 2
            // =============================================================
            if (levelIndex == 2)
            {
                // vloer
                for (int i = -10; i < 24; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                for (int i = 30; i < 50; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));

                //voor vallende muur
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(350, 336)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(414, 272)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(478, 208)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1100, 300)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1536, 336)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1856, 250)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1856, 90)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2200, 285)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2800, 285)));

                //na vallende muur
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3200, 336)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3264, 272)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3328, 208)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3392, 144)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3456, 144)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3520, 144)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3584, 144)));


                // eerste muur
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1920, 336)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1920, 272)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1920, 208)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1920, 144)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1920, 80)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1920, 16)));
                objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1920, -48)));

                // tweede muur
                List<BigWall> gateBlocks = new List<BigWall>();

                // Helper functie: Voegt blok toe aan de game-wereld EN aan de lijst voor de GateKeeper
                void AddGateBlock(float x, float y)
                {
                    var gate = new BigWall(blockTex, blockSourceRect, new Vector2(x, y));
                    objects.Add(gate);
                    gateBlocks.Add(gate);
                }

                AddGateBlock(3000, 336);
                AddGateBlock(3000, 272);
                AddGateBlock(3000, 208);
                AddGateBlock(3000, 144);
                AddGateBlock(3000, 80);
                AddGateBlock(3000, 16);
                AddGateBlock(3000, -48);

                // powerups 
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1130, 260)));
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1568, 300)));
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1875, 220)));


                // powerups die respawnen elke 5 sec
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(2230, 250), true));
                objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(2830, 250), true));

                // coins
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(420, 355), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(1950, -85), 2, coinFrames));
                objects.Add(new Coin(CoinTex, coinRect, new Vector2(3600, 110), 2, coinFrames));


                // groene slimes

                // slimes dat niet getracked moeten worden voor vallende muur
                var slimePatrol = new PatrolEnemyBehavior(1.5f);
                objects.Add(new Enemy(greenEnemyTex, new Vector2(600, 355), greenSlimeFrames, slimePatrol, 3f, objects));

                var slimePatrol2 = new PatrolEnemyBehavior(1.5f);
                objects.Add(new Enemy(greenEnemyTex, new Vector2(1200, 355), greenSlimeFrames, slimePatrol2, 3f, objects));

                // slimes die wel getracked moeten worden voor muur
                List<Enemy> slimesToWatch = new List<Enemy>();

                var slimePatrol3 = new PatrolEnemyBehavior(1.5f);
                var slime3 = new Enemy(greenEnemyTex, new Vector2(2000, 355), greenSlimeFrames, slimePatrol3, 3f, objects);
                objects.Add(slime3);
                slimesToWatch.Add(slime3);

                var slimePatrol4 = new PatrolEnemyBehavior(1.5f);
                var slime4 = new Enemy(greenEnemyTex, new Vector2(2400, 355), greenSlimeFrames, slimePatrol4, 3f, objects);
                objects.Add(slime4);
                slimesToWatch.Add(slime4);

                var slimePatrol5 = new PatrolEnemyBehavior(1.5f);
                var slime5 = new Enemy(greenEnemyTex, new Vector2(2600, 355), greenSlimeFrames, slimePatrol5, 3f, objects);
                objects.Add(slime5);
                slimesToWatch.Add(slime5);

                var slimePatrol6 = new PatrolEnemyBehavior(1.5f);
                var slime6 = new Enemy(greenEnemyTex, new Vector2(2800, 355), greenSlimeFrames, slimePatrol6, 3f, objects);
                objects.Add(slime6);
                slimesToWatch.Add(slime6);

                // gatekeeper
                var keeper = new GateKeeper(gateBlocks, slimesToWatch);
                objects.Add(keeper);

                // 8 Grote Paarse slime
                var chaseLogic = new ConstantMoveBehavior(1);
                float megaScale = 28f;
                Vector2 startPos = new Vector2(-500, -20);
                var megaSlime = new Enemy(purpleEnemyTex, startPos, purpleSlimeFrames, chaseLogic, megaScale, new List<IGameObject>());

                objects.Add(megaSlime);
            }

            return objects;
        }
    }
}