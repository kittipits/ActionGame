using System.Diagnostics;
using System.IO;

namespace ActionGame
{
    public class Map
    {
        public const int None = -1;     // 何も無いマス
        //public int[] Wall = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };      // 壁
        //public int[] Goal = { 11, 12, 13 };
        //public const int Needle = 1;    // 針、トゲ
        //public const int Brick = 2;     // レンガ
        //public const int Floor = 3;     // 下から抜けられる床

        public const int Width = 200;   // 横のマス数
        public const int Height = 15;   // 縦のマス数
        public const int CellSize = 32; // 1マスのピクセル数

        PlayScene playScene;    // 参照
        int[,] terrain;         // 地形データ

        // コンストラクタ
        public Map(PlayScene playScene, string stageName) 
        {
            this.playScene = playScene;
            LoadTerrain("Map/" + stageName + "_terrain.csv");
            LoadObjects("Map/" + stageName + "_object.csv");
        }

        //csvファイルを読み込んで、二次元配列に格納する
        void LoadTerrain(string filePath)
        {
            terrain = new int[Width, Height];       // 2次元配列を生成

            string[] lines = File.ReadAllLines(filePath);   // ファイルを行ごとに読み込む

            Debug.Assert(lines.Length == Height, filePath + "の高さが不正です：" + lines.Length);

            for (int y = 0; y < Height; y++)
            {
                // 行をカンマで分割する
                string[] splitted = lines[y].Split(new char[] { ',' });
                Debug.Assert(splitted.Length == Width, filePath + "の" + y + "行目の列数が不正です:" + splitted.Length);

                for (int x = 0; x < Width; x++)
                {
                    // 2次元配列に格納する
                    terrain[x, y] = int.Parse(splitted[x]);
                }
            }
        }

        void LoadObjects(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            Debug.Assert(lines.Length == Height, filePath + "の高さが不正です：" + lines.Length);

            for (int y = 0; y < Height; y++)
            {
                string[] splitted = lines[y].Split(new char[] { ',' });
                Debug.Assert(splitted.Length == Width, filePath + "の" + y + "行目の列数が不正です:" + splitted.Length);

                for (int x = 0; x < Width; x++)
                {
                    int id  = int.Parse(splitted[x]);

                    // -1（何も配置されていない場所）は何もしない
                    if (id == -1) continue;

                    // オブジェクトを生成・配置する
                    SpawnObject(x, y, id);
                }
            }
        }

        // オブジェクトを生成・配置する
        void SpawnObject(int mapX, int mapY, int objectID)
        {
            // 生成位置
            float spawnX = mapX * CellSize;
            float spawnY = mapY * CellSize;

            if (objectID == 0)
            {
                Player player = new Player(playScene, spawnX, spawnY);
                playScene.gameObjects.Add(player);
                playScene.player = player;
            }
            else if (objectID == 1)
            {
                playScene.gameObjects.Add(new EnemyMushroom(playScene, spawnX, spawnY));
            }
            else if (objectID == 2)
            {
                playScene.gameObjects.Add(new EnemyTurtle(playScene, spawnX, spawnY));
            }
            else if (objectID == 3)
            {
                playScene.gameObjects.Add(new EnemyFlower(playScene, spawnX, spawnY));
            }
            else if (objectID == 4)
            {
                playScene.gameObjects.Add(new EnemySquid(playScene, spawnX, spawnY));
            }
            else if (objectID == 5)
            {
                playScene.gameObjects.Add(new EnemyGhost(playScene, spawnX, spawnY));
            }
            else if (objectID == 6)
            {
                playScene.gameObjects.Add(new EnemyBomber(playScene, spawnX, spawnY));
            }
            else if (objectID == 10)
            {
                playScene.gameObjects.Add(new Boss(playScene, spawnX, spawnY));
            }
            else if (objectID == 11)
            {
                playScene.gameObjects.Add(new FloorObjectOne(playScene, spawnX, spawnY));
            }
            else if (objectID == 12)
            {
                playScene.gameObjects.Add(new FloorObjectTwo(playScene, spawnX, spawnY));
            }
            else if (objectID == 13)
            {
                playScene.gameObjects.Add(new FloorObjectThree(playScene, spawnX, spawnY));
            }
            else if (objectID == 14)
            {
                playScene.gameObjects.Add(new FloorObjectFour(playScene, spawnX, spawnY));
            }
            else if (objectID == 15)
            {
                playScene.gameObjects.Add(new BlockBreakable(playScene, spawnX, spawnY));
            }
            else if (objectID == 16)
            {
                playScene.gameObjects.Add(new BlockSpike(playScene, spawnX, spawnY));
            }
            else if (objectID == 17)
            {
                playScene.gameObjects.Add(new SpikeDrop(playScene, spawnX, spawnY));
            }
            else if (objectID == 20)
            {
                playScene.gameObjects.Add(new BlockBlank(playScene, spawnX, spawnY));
            }
            else if (objectID == 21)
            {
                playScene.gameObjects.Add(new BlockItemCoin(playScene, spawnX, spawnY));
            }
            else if (objectID == 22)
            {
                playScene.gameObjects.Add(new BlockItemLife(playScene, spawnX, spawnY));
            }
            else if (objectID == 23)
            {
                playScene.gameObjects.Add(new BlockItemSpeed(playScene, spawnX, spawnY));
            }
            else if (objectID == 24)
            {
                playScene.gameObjects.Add(new BlockItemShield(playScene, spawnX, spawnY));
            }
            else if (objectID == 31)
            {
                playScene.gameObjects.Add(new ItemCoin(playScene, spawnX, spawnY));
            }
            else if (objectID == 32)
            {
                playScene.gameObjects.Add(new ItemLifeBoost(playScene, spawnX, spawnY));
            }
            else if (objectID == 33)
            {
                playScene.gameObjects.Add(new ItemShieldBoost(playScene, spawnX, spawnY));
            }
            else if (objectID == 34)
            {
                playScene.gameObjects.Add(new ItemSpeedBoost(playScene, spawnX, spawnY));
            }
            else if (objectID == 40)
            {
                playScene.gameObjects.Add(new AI(playScene, spawnX, spawnY));
            }
            else
            {
                Debug.Assert(false, "オブジェクトID" + objectID + "番の生成処理は未実装です。");
            }
        }

        // 地形を描画する
        public void DrawTerrain()
        {
            // 画面内のマップのみ描画するようにする
            int left = (int)(Camera.x / CellSize);
            int top = (int)(Camera.y / CellSize);
            int right = (int)((Camera.x + Screen.Width - 1) / CellSize);
            int bottom = (int)((Camera.y + Screen.Height - 1) / CellSize);

            if (left < 0) left = 0;
            if (top < 0) top = 0;
            if (right >= Width) right = Width - 1;
            if (bottom >= Height) bottom = Height - 1;

            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                { 
                    int id = terrain[x, y];
                    if (id == None) continue;
                    if (id == 21) Camera.DrawGraph(x * CellSize, y * CellSize, Image.goalOne);
                    else if (id == 23) Camera.DrawGraph(x * CellSize, y * CellSize, Image.goalThree);
                    else Camera.DrawGraph(x * CellSize, y * CellSize, Image.mapchip[id]);            
                }
            }
        }

        // 指定された座標（ワールド座標）の地形データを取得する。
        public int GetTerrain(float worldX, float worldY)
        {
            // 負の座標が指定された場合は、何も無いものとして扱う
            if (worldX < 0 || worldY < 0)
                return None;

            // マップ座標系（二次元配列の行と列）に変換する
            int mapX = (int)(worldX / CellSize);
            int mapY = (int)(worldY / CellSize);

            // 二次元配列の範囲外は、何も無いものとして扱う
            if (mapX >= Width || mapY >= Height)
                return None;
            return terrain[mapX, mapY];
        }

        // 指定された座標（ワールド座標）の地形が壁か調べる
        public bool IsWall(float worldX, float worldY)
        { 
            int terrainID = GetTerrain(worldX, worldY);
            return terrainID == 0 || terrainID == 1 || terrainID == 2 ||
                terrainID == 3 || terrainID == 4 || terrainID == 5 ||
                terrainID == 6 || terrainID == 7 || terrainID == 8 ||
                terrainID == 9 || terrainID == 10 || terrainID == 11 ||
                terrainID == 12 || terrainID == 13 || terrainID == 14 ||
                terrainID == 15 || terrainID == 16 || terrainID == 17 ||
                terrainID == 18 || terrainID == 19 || terrainID == 20;
        }

        public bool IsGoal(float worldX, float worldY)
        {
            int terrainID = GetTerrain(worldX, worldY);
            return terrainID == 21 || terrainID == 22 || terrainID == 23;
        }
    }
}
