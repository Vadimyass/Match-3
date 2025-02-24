using System.Collections.Generic;
using Gameplay.Scripts.MatchEngine;
using MatchEngine.MatchAnimation;
using UnityEngine;

namespace MatchEngine
{
    public class TileSpawner
    {
        private readonly GameObject tilePrefab;
        private readonly Transform parent;
        private readonly TileType[] tileTypes;
        private readonly Queue<Tile> tilePool;

        public TileSpawner(GameObject prefab, Transform parentTransform, TileType[] types, Queue<Tile> pool)
        {
            tilePrefab = prefab;
            parent = parentTransform;
            tileTypes = types;
            tilePool = pool;
        }

        public Tile SpawnTile(int x, int y, Match3Grid grid, TileType type = default)
        {
            Tile newTile;

            // Берём тайл из пула или создаём новый
            if (tilePool.Count > 0)
            {
                newTile = tilePool.Dequeue();
                newTile.gameObject.SetActive(true);
            }
            else
            {
                GameObject obj = Object.Instantiate(tilePrefab, parent);
                newTile = obj.GetComponent<Tile>();
            }

            // Если тип не указан, выбираем случайный
            if (type == default)
            {
                type = tileTypes[Random.Range(1, tileTypes.Length)];
            }
            
            newTile.SetAlpha(1);

            // Инициализируем тайл
            newTile.Initialize(type, new Vector2Int(x, y));
            newTile.transform.position = new Vector3(x, y, 0);
            newTile.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

            return newTile;
        }
    }
}