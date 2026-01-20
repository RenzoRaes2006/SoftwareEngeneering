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
            // AANGEPAST: GraphicsDevice parameter toegevoegd voor de Boss HP balk
            public static List<IGameObject> CreateLevel(
                int levelIndex,
                Texture2D blockTex, Rectangle blockSourceRect, Rectangle wallRect,
                Texture2D powerUpTex, Rectangle powerUpRect,
                Texture2D CoinTex, Rectangle coinRect, List<Rectangle> coinFrames,
                Texture2D greenEnemyTex, List<Rectangle> greenSlimeFrames,
                Texture2D purpleEnemyTex, List<Rectangle> purpleSlimeFrames,
                Texture2D SurikenTex, Rectangle SurikenRect,
                GraphicsDevice graphicsDevice)
            {
                var objects = new List<IGameObject>();

                // --- LEVEL 1 (Blijft hetzelfde) ---
                if (levelIndex == 1)
                {
                    //vloer
                    for (int i = 0; i < 25; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                    for (int i = 30; i < 40; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));

                    //blokken voor jump
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(200, 300)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(264, 300)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(800, 300)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1000, 250)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1200, 175)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1500, 300)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1564, 236)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1300, 80)));

                    //blokken na jump
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2150, 300)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2250, 200)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2150, 100)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2560, 336)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2624, 272)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2688, 208)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2752, 144)));

                    //einde vloer
                    for (int i = 43; i < 55; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(2816, 144)));

                    //powerups
                    objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(260, 260)));
                    objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1594, 200)));

                    //coins
                    objects.Add(new Coin(CoinTex, coinRect, new Vector2(830, 260), 2, coinFrames));
                    objects.Add(new Coin(CoinTex, coinRect, new Vector2(1320, 60), 2, coinFrames));
                    objects.Add(new Coin(CoinTex, coinRect, new Vector2(2170, 80), 2, coinFrames));
                    objects.Add(new Coin(CoinTex, coinRect, new Vector2(2860, 100), 2, coinFrames));

                    //green slime
                    var slimePatrol = new PatrolEnemyBehavior(1.5f, 42, 36, objects);
                    var slime = new Enemy(greenEnemyTex, new Vector2(350, 355), greenSlimeFrames, slimePatrol, 3f, objects);
                    objects.Add(slime);

                    //traps
                    var trapBehavior1 = new TrapBehavior(2568, 2650, 2);
                    objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(2568, 300), trapBehavior1));
                    var trapBehavior2 = new TrapBehavior(2632, 2714, 2);
                    objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(2714, 236), trapBehavior2));
                }

                // --- LEVEL 2 (MET BOSS) ---
                if (levelIndex == 2)
                {
                    int bossWidth = 14 * 10;
                    int bossHeight = 12 * 10;
                    //vloer
                    for (int i = -10; i < 24; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));
                    for (int i = 30; i < 50; i++) objects.Add(new Block(blockTex, blockSourceRect, new Vector2(i * 64, 400)));

                    //voor muur
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(350, 336)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(414, 272)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(478, 208)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1100, 300)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1536, 336)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1856, 250)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(1856, 60)));


                    //gewone muur
                    objects.Add(new BigWall(blockTex, wallRect, new Vector2(1920, 304)));
                    objects.Add(new BigWall(blockTex, wallRect, new Vector2(1920, 208)));
                    objects.Add(new BigWall(blockTex, wallRect, new Vector2(1920, 112)));
                    objects.Add(new BigWall(blockTex, wallRect, new Vector2(1920, 16)));

                    //na gevallen muur
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3200, 336)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3264, 272)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3328, 208)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3392, 144)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3456, 144)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3520, 144)));
                    objects.Add(new Block(blockTex, blockSourceRect, new Vector2(3584, 144)));



                    //traps
                    var trapBehavior1 = new TrapBehavior(3264, 3392, 2);
                    objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(3264, 220), trapBehavior1));
                    var trapBehavior2 = new TrapBehavior(3328, 3456, 2);
                    objects.Add(new Trap(SurikenTex, SurikenRect, new Vector2(3456, 160), trapBehavior2));

                    //muur die weg valt
                    List<BigWall> gateBlocks = new List<BigWall>();
                    void AddGateBlock(float x, float y)
                    {
                        var gate = new BigWall(blockTex, wallRect, new Vector2(x, y));
                        objects.Add(gate);
                        gateBlocks.Add(gate);
                    }
                    AddGateBlock(3000, 304);
                    AddGateBlock(3000, 208);
                    AddGateBlock(3000, 112);
                    AddGateBlock(3000, 16);


                    //powerups
                    objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(1568, 300)));
                    objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(2230, 300), true));
                    objects.Add(new PowerUp(powerUpTex, powerUpRect, new Vector2(2830, 300), true));

                    //coins
                    objects.Add(new Coin(CoinTex, coinRect, new Vector2(490, 190), 2, coinFrames));
                    objects.Add(new Coin(CoinTex, coinRect, new Vector2(1950, -80), 2, coinFrames));
                    objects.Add(new Coin(CoinTex, coinRect, new Vector2(3600, 120), 2, coinFrames));



                    //green slimes
                    var slimePatrol = new PatrolEnemyBehavior(1.5f, 42, 36, objects);
                    objects.Add(new Enemy(greenEnemyTex, new Vector2(600, 355), greenSlimeFrames, slimePatrol, 3f, objects));
                    var slimePatrol2 = new PatrolEnemyBehavior(1.5f, 42, 36, objects);
                    objects.Add(new Enemy(greenEnemyTex, new Vector2(1200, 355), greenSlimeFrames, slimePatrol2, 3f, objects));

                    var bossStrategy = new PatrolEnemyBehavior(2f, bossWidth, bossHeight, objects, 20);
                    // 2. Maak de Boss aan (HP op 100, schaal op 10f)
                    var boss = new Boss(
                        purpleEnemyTex,
                        new Vector2(2500, 100), // Positie (pas aan naar wens)
                        purpleSlimeFrames,
                        bossStrategy,
                        10f, // DE SCHAAL: 10 keer groter
                        objects,
                        100,
                        graphicsDevice
                    );

                    objects.Add(boss);

                    // 3. De GateKeeper
                    List<Enemy> enemiesToWatch = new List<Enemy> { boss };
                    objects.Add(new GateKeeper(gateBlocks, enemiesToWatch));


                }

                return objects;
            }
        }
    }