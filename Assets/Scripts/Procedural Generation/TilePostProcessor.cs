using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePostProcessor : MonoBehaviour
{
    [SerializeField] private WFCGenerator wfcGenerator;
    public GridManager gridManager; // Direct reference to GridManager
    [SerializeField] private Tilemap backgroundTilemap; // Reference to the tilemap for background tiles
    [SerializeField] private List<DecorativeTile> decorativeTiles; // List of decorative tiles

    private void Start()
    {
        // Cache grid dimensions and tile type index from GridManager
        int width = gridManager.gridWidth;
        int height = gridManager.gridHeight;

        // Post-processing steps after WFC generation
        ProcessTiles();
    }
    
    // Main method to process all tiles, including decorations
    public void ProcessTiles()
    {
        ProcessDecorations(); // Process decorations after tile placement
    }

    // Main method to process decorative tiles
    private void ProcessDecorations()
    {
        for (int y = 0; y < gridManager.gridHeight; y++)
        {
            for (int x = 0; x < gridManager.gridWidth; x++)
            {
                TileType currentTileType = gridManager.GetTileTypeAt(x, y);

                if (currentTileType == TileType.Grass)
                {
                    float totalWeight = decorativeTiles.Where(tile => tile.tileType == TileType.Grass).Sum(tile => tile.weight);
                    float randomValue = Random.Range(0f, totalWeight + 1.0f); // Slightly increase randomness, some tiles may not have decoration

                    float cumulativeWeight = 0f;
                    foreach (var decoTile in decorativeTiles.Where(tile => tile.tileType == TileType.Grass))
                    {
                        cumulativeWeight += decoTile.weight;
                        if (randomValue <= cumulativeWeight)
                        {
                            PlaceDecorativeTile(x, y, decoTile);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void PlaceDecorativeTile(int x, int y, DecorativeTile selectedTile)
    {
        Vector3Int position = new Vector3Int(x, y, 0);
        backgroundTilemap.SetTile(position, selectedTile.tile);
    }


    public void ClearTiles()
    {
        if (backgroundTilemap != null)
        {
            backgroundTilemap.ClearAllTiles();
        }
        else
        {
            Debug.LogError("Background Tilemap is not assigned in TilePostProcessor!");
        }
    }
}







