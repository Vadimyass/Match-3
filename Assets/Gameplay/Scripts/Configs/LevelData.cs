using System;
using MatchEngine;

namespace Gameplay.Scripts.Configs
{
    [Serializable]
    public class LevelData
    {
        public int width = 8;
        public int height = 8;
        public int hiddenLayers = 2;
        public TileType[] initialLayout;
        
        public void InitializeGrid()
        {
            initialLayout = new TileType[width * height];
        }
    }
}

