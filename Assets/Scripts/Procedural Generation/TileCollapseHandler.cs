using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileCollapseHandler
{
    private WFCGenerator wfcGenerator; // Reference to WFCGenerator for accessing constraints and tile data
    private GridManager gridManager;   // Reference to GridManager to handle the grid state

    public TileCollapseHandler(WFCGenerator generator, GridManager manager)
    {
        wfcGenerator = generator;
        gridManager = manager;
    }
public int CollapseTileAt(int x, int y)
{
    List<TileConstraint> validTiles = new List<TileConstraint>();

    // Get neighboring tile types using GridManager's GetTileTypeAt
    TileType aboveNeighborType = gridManager.GetTileTypeAt(x, y + 1);
    TileType belowNeighborType = gridManager.GetTileTypeAt(x, y - 1);
    TileType leftNeighborType = gridManager.GetTileTypeAt(x - 1, y);
    TileType rightNeighborType = gridManager.GetTileTypeAt(x + 1, y);

    // Loop through all tile constraints and check compatibility
    foreach (var constraint in wfcGenerator.tileConstraints)
    {
        bool isCompatible = IsTileCompatible(constraint.tileType, aboveNeighborType, TileDirectionType.NORTH)
                            && IsTileCompatible(constraint.tileType, belowNeighborType, TileDirectionType.SOUTH)
                            && IsTileCompatible(constraint.tileType, leftNeighborType, TileDirectionType.WEST)
                            && IsTileCompatible(constraint.tileType, rightNeighborType, TileDirectionType.EAST);

        if (isCompatible)
        {
            validTiles.Add(constraint);
        }
    }

    // Select a tile using weights
    if (validTiles.Count > 0)
    {
        TileConstraint selectedTile = SelectTileWithWeight(validTiles);
        int selectedIndex = wfcGenerator.tileConstraints.IndexOf(selectedTile);
        gridManager.SetTileIndex(x, y, selectedIndex);
        return selectedIndex;
    }

    return -1; // No valid tile found
}

// Helper method to select a tile with weight
    private TileConstraint SelectTileWithWeight(List<TileConstraint> validTiles)
    {
        float totalWeight = validTiles.Sum(tile => tile.weight);
        Debug.Log($"Total weight for tile selection: {totalWeight}");

        float randomValue = Random.Range(0f, totalWeight);
        Debug.Log($"Random value for selection: {randomValue}");

        float cumulativeWeight = 0f;
        foreach (var tile in validTiles)
        {
            cumulativeWeight += tile.weight;
            Debug.Log($"Tile: {tile.tileType}, Cumulative Weight: {cumulativeWeight}, RandomValue: {randomValue}");

            if (randomValue <= cumulativeWeight)
            {
                Debug.Log($"Selected Tile: {tile.tileType}");
                return tile;
            }
        }

        return validTiles[validTiles.Count - 1]; // Default to the last tile in case of rounding errors
    }
    private bool IsTileCompatible(TileType currentTileType, TileType neighborTileType, TileDirectionType direction)
    {
        // If the neighbor tile is 'None', treat it as compatible
        if (neighborTileType == TileType.None)
        {
            return true;
        }

        // Retrieve TileConstraints for the current and neighboring tiles
        TileConstraint currentTile = wfcGenerator.GetTileConstraintByType(currentTileType);
        TileConstraint neighborTile = wfcGenerator.GetTileConstraintByType(neighborTileType);

        if (currentTile == null || neighborTile == null)
        {
            Debug.Log($"Null TileConstraint: currentTileType={currentTileType}, neighborTileType={neighborTileType}");
            return false;
        }

        // Find the adjacency rule for the specified direction
        var rule = currentTile.adjacencyRules.Find(r => r.direction == direction);

        if (rule == null)
        {
            Debug.Log($"No adjacency rule for direction {direction} on tile {currentTileType}");
            return false;
        }

        // Check if the neighbor tile is in the list of allowed tiles for this direction
        bool isCompatible = rule.allowedTiles.Exists(allowedTile => allowedTile.tileType == neighborTileType);
        return isCompatible;
    }
}



