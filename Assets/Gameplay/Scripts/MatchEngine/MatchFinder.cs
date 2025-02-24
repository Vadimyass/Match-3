using System.Collections.Generic;
using System.Linq;
using Gameplay.Scripts.MatchEngine;

public class MatchFinder
{
    public List<Tile> FindMatches(Tile[,] grid, int width, int height)
    {
        List<Tile> matchedTiles = new List<Tile>();
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                Tile tile1 = grid[x, y];
                Tile tile2 = grid[x + 1, y];
                Tile tile3 = grid[x + 2, y];

                if (tile1 != null && tile2 != null && tile3 != null &&
                    tile1.Type == tile2.Type && tile2.Type == tile3.Type)
                {
                    matchedTiles.Add(tile1);
                    matchedTiles.Add(tile2);
                    matchedTiles.Add(tile3);
                }
            }
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                Tile tile1 = grid[x, y];
                Tile tile2 = grid[x, y + 1];
                Tile tile3 = grid[x, y + 2];

                if (tile1 != null && tile2 != null && tile3 != null &&
                    tile1.Type == tile2.Type && tile2.Type == tile3.Type)
                {
                    matchedTiles.Add(tile1);
                    matchedTiles.Add(tile2);
                    matchedTiles.Add(tile3);
                }
            }
        }

        return matchedTiles.Distinct().ToList();
    }
}