using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePostProcessor : MonoBehaviour
{
    [SerializeField] private WFCGenerator wfcGenerator;
    [SerializeField] private Tilemap backgroundTilemap; // Reference to the tilemap for background tiles
    [SerializeField] private List<TileConstraint> transitionTiles; // List of transition tiles
    [SerializeField] private List<DecorativeTile> decorativeTiles; // List of decorative tiles

    private void Start()
    {
        // Post-processing steps after WFC generation
        ProcessTransitions();
        ProcessDecorations();
    }
    
    // Method to clear all tiles from the tilemap
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
    // Main method to process all tiles, including transitions and decorations
    public void ProcessTiles()
    {
        ProcessTransitions();
        ProcessDecorations();
    }
    
    private void ProcessTransitions()
    {
        // Iterate over the grid to find spots where transition tiles are needed
        for (int y = 0; y < wfcGenerator.gridHeight; y++)
        {
            for (int x = 0; x < wfcGenerator.gridWidth; x++)
            {
                // Get the current tile type and its neighbors
                TileType currentTileType = wfcGenerator.GetTileTypeAt(x, y);
                TileType rightNeighborType = wfcGenerator.GetTileTypeAt(x + 1, y);
                TileType bottomNeighborType = wfcGenerator.GetTileTypeAt(x, y - 1);

                // Place transition tiles between different biome types
                if (currentTileType == TileType.Grass && rightNeighborType == TileType.Sand)
                {
                    PlaceTransitionTile(x, y, TileType.Grass, TransitionType.Sand);
                }
                else if (currentTileType == TileType.Grass && bottomNeighborType == TileType.Sand)
                {
                    PlaceTransitionTile(x, y, TileType.Grass, TransitionType.Sand);
                }
                // Add more conditions for other transitions as needed
            }
        }
    }

    private void PlaceTransitionTile(int x, int y, TileType type1, TransitionType transitionType)
    {
        int transitionTileIndex = GetTransitionTileIndex(type1, transitionType);
        if (transitionTileIndex >= 0)
        {
            wfcGenerator.SetTileAt(x, y, transitionTileIndex);
        }
    }

    // Retrieve the transition tile index for a biome pair
    private int GetTransitionTileIndex(TileType type1, TransitionType transitionType)
    {
        foreach (var constraint in transitionTiles)
        {
            if (constraint.tileType == type1 && constraint.transitionType == transitionType)
            {
                return wfcGenerator.tileTypeToIndex[constraint.tileType][0]; // Assuming one transition tile per type pair
            }
        }
        return -1; // Return -1 if no matching transition tile is found
    }

    private void ProcessDecorations()
    {
        // Iterate over the grid to place decorative elements
        for (int y = 0; y < wfcGenerator.gridHeight; y++)
        {
            for (int x = 0; x < wfcGenerator.gridWidth; x++)
            {
                TileType currentTileType = wfcGenerator.GetTileTypeAt(x, y);

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
            wfcGenerator.backgroundTilemap.SetTile(position, selectedTile.tile);
        }
    }
}



