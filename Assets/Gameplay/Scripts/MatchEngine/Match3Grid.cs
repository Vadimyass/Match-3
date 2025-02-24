using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Configs;
using Gameplay.Scripts.MatchEngine;
using UnityEngine;
using UnityEngine.Serialization;

namespace MatchEngine
{
    public class Match3Grid : MonoBehaviour
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private TileType[] _tileTypes;
        
        private LevelData _levelData;
        private Tile[,] _grid;
        private Queue<Tile> _tilePool = new();
        private MatchFinder _matchFinder;
        private TileSpawner _tileSpawner;
        private int _hiddenLayers = 2;

        private async void Start()
        {
            _grid = new Tile[_levelData.width, _levelData.height + _levelData.hiddenLayers];
            _matchFinder = new MatchFinder();
            _tileSpawner = new TileSpawner(_tilePrefab, transform, _tileTypes, _tilePool);

            await InitializeGrid();
        }

        private async UniTask InitializeGrid()
        {
            for (int x = 0; x < _levelData.height; x++)
            {
                for (int y = _levelData.height; y < _levelData.height + _hiddenLayers; y++)
                {
                    _grid[x, y] = _tileSpawner.SpawnTile(x, y, this);
                }
            }
            
            for (int x = 0; x < _levelData.width; x++)
            {
                for (int y = 0; y < _levelData.height; y++)
                {
                    int index = y * _levelData.width + x;
                    if (index < _levelData.initialLayout.Length && _levelData.initialLayout[index] != default)
                    {
                        _grid[x, y] = _tileSpawner.SpawnTile(x, y, this, _levelData.initialLayout[index]);
                    }
                    else
                    {
                        _grid[x, y] = _tileSpawner.SpawnTile(x, y, this);
                    }
                }
            }

            await UniTask.Yield();
        }
        
        public async UniTask SwapTiles(Tile a, Tile b)
        {
            Vector2Int posA = a.GridPosition;
            Vector2Int posB = b.GridPosition;

            
            _grid[posA.x, posA.y] = b;
            _grid[posB.x, posB.y] = a;
            
            List<Tile> matchedTiles = _matchFinder.FindMatches(_grid, _levelData.width, _levelData.height);

            if (matchedTiles.Count == 0)
            {
                _grid[posA.x, posA.y] = a;
                _grid[posB.x, posB.y] = b;
                
                await UniTask.WhenAll(
                    a.RequestMove(posA),
                    b.RequestMove(posB)
                );
            }
            else
            {
                await UniTask.WhenAll(
                    a.RequestMove(posB),
                    b.RequestMove(posA)
                );

                await CheckMatches();
            }
        }

        private void LogGridState(string message)
        {
            Debug.LogError(message);
            for (int y = _levelData.height - 1; y >= 0; y--)
            {
                string row = "";
                for (int x = 0; x < _levelData.width; x++)
                {
                    row += _grid[x, y] != null ? "X " : "O ";
                }
                Debug.LogError(row);
            }
        }


        public async UniTask CheckMatches()
        {
            while (true)
            {
                List<Tile> matchedTiles = _matchFinder.FindMatches(_grid, _levelData.width, _levelData.height);
                if (matchedTiles.Count == 0) break; 

                
                foreach (Tile tile in matchedTiles)
                {
                    tile.ResetTile();
                    _tilePool.Enqueue(tile);
                    _grid[tile.GridPosition.x, tile.GridPosition.y] = null;
                    tile.RequestMatchAnimation();
                }

                
                LogGridState("Before Gravity");
                await ApplyGravity();
                LogGridState("After Gravity");
            }
        }

        private async UniTask ApplyGravity()
        {
            List<UniTask> tasks = new();

            for (int x = 0; x < _levelData.width; x++)
            {
                tasks.Add(ApplyGravityToColumn(x));
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask ApplyGravityToColumn(int x)
        {
            List<UniTask> moveTasks = new List<UniTask>();
            
            for (int y = 0; y < _levelData.height; y++)
            {
                if (_grid[x, y] == null)
                {
                    for (int y2 = y + 1; y2 < _levelData.height + _hiddenLayers; y2++) 
                    {
                        if (_grid[x, y2] != null)
                        {
                            Tile tile = _grid[x, y2];
                            _grid[x, y] = tile; 
                            _grid[x, y2] = null; 
                            moveTasks.Add(tile.RequestMove(new Vector2Int(x, y)));
                            break;
                        }
                    }
                }
            }

            await UniTask.WhenAll(moveTasks); 

            
            for (int y = 0; y < _levelData.height; y++)
            {
                if (_grid[x, y] == null)
                {
                    
                    int spawnY = _levelData.height + _hiddenLayers - 1;
                    Tile newTile = _tileSpawner.SpawnTile(x, spawnY, this);
                    _grid[x, spawnY] = newTile;
                    
                    _grid[x, y] = newTile;
                    _grid[x, spawnY] = null;
                    await newTile.RequestMove(new Vector2Int(x, y));
                }
            }
        }
    }
}
