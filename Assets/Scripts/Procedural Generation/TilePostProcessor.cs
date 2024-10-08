using System.Collections.Generic;
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
    public void ProcessDecorations()
    {
        // Iterate over the grid to place decorative elements
        for (int y = 0; y < gridManager.gridHeight; y++)
        {
            for (int x = 0; x < gridManager.gridWidth; x++)
            {
                TileType currentTileType = gridManager.GetTileTypeAt(x, y);

                // Example: Place decorations only on grass tiles
                if (currentTileType == TileType.Grass)
                {
                    PlaceDecorativeTile(x, y);
                }
            }
        }
    }
    private void PlaceDecorativeTile(int x, int y)
    {
        // Choose a random decorative tile
        if (decorativeTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, decorativeTiles.Count);
            DecorativeTile selectedTile = decorativeTiles[randomIndex];

            // Place the decorative tile
            Vector3Int position = new Vector3Int(x, y, 0);
            backgroundTilemap.SetTile(position, selectedTile.tile);
        }
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







