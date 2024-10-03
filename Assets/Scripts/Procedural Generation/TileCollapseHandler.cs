using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileCollapseHandler
{
    private WFCGenerator wfcGenerator; // Reference to the WFCGenerator for accessing tile constraints and utility functions

    public TileCollapseHandler(WFCGenerator generator)
    {
        wfcGenerator = generator;
    }

    public int CollapseTileAt(int x, int y)
    {
        List<int> validTiles = new List<int>();

        // Get neighboring tile types, directions, and transition types using public methods from WFCGenerator
        TileType aboveNeighborType = wfcGenerator.GetNeighborTileType(x, y + 1);
        TileDirection aboveNeighborDirection = wfcGenerator.GetNeighborTileDirection(x, y + 1);
        TransitionType aboveNeighborTransition = wfcGenerator.GetNeighborTransitionType(x, y + 1);

        TileType belowNeighborType = wfcGenerator.GetNeighborTileType(x, y - 1);
        TileDirection belowNeighborDirection = wfcGenerator.GetNeighborTileDirection(x, y - 1);
        TransitionType belowNeighborTransition = wfcGenerator.GetNeighborTransitionType(x, y - 1);

        TileType leftNeighborType = wfcGenerator.GetNeighborTileType(x - 1, y);
        TileDirection leftNeighborDirection = wfcGenerator.GetNeighborTileDirection(x - 1, y);
        TransitionType leftNeighborTransition = wfcGenerator.GetNeighborTransitionType(x - 1, y);

        TileType rightNeighborType = wfcGenerator.GetNeighborTileType(x + 1, y);
        TileDirection rightNeighborDirection = wfcGenerator.GetNeighborTileDirection(x + 1, y);
        TransitionType rightNeighborTransition = wfcGenerator.GetNeighborTransitionType(x + 1, y);

        // Loop through all tile constraints and check compatibility
        foreach (var constraint in wfcGenerator.tileConstraints)
        {
            if (constraint == null)
                continue;

            bool isCompatible = true;

            // Check compatibility with all four directions
            if (!IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                  aboveNeighborType, aboveNeighborDirection, aboveNeighborTransition, Direction.NORTH))
            {
                isCompatible = false;
            }
            else if (!IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                       belowNeighborType, belowNeighborDirection, belowNeighborTransition, Direction.SOUTH))
            {
                isCompatible = false;
            }
            else if (!IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                       leftNeighborType, leftNeighborDirection, leftNeighborTransition, Direction.WEST))
            {
                isCompatible = false;
            }
            else if (!IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                       rightNeighborType, rightNeighborDirection, rightNeighborTransition, Direction.EAST))
            {
                isCompatible = false;
            }

            // If all checks passed, add all indices of the compatible tileType to the list of valid tiles
            if (isCompatible)
            {
                if (wfcGenerator.tileTypeToIndex != null && wfcGenerator.tileTypeToIndex.TryGetValue(constraint.tileType, out List<int> indices))
                {
                    validTiles.AddRange(indices);
                }
            }
        }

        // If we have valid tiles, select one
        if (validTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, validTiles.Count);
            return validTiles[randomIndex];
        }

        // Fallback to a default tile if no valid tiles found
        return 0;
    }

    private bool IsTileCompatible(TileType tileType, TileDirection tileDirection, TransitionType transitionType,
                                  TileType neighborType, TileDirection neighborDirection, TransitionType neighborTransitionType, Direction direction)
    {
        if (neighborType == TileType.None) return true; // No neighbor means compatibility by default

        // Find the current tile constraint
        TileConstraint currentTile = wfcGenerator.tileConstraints.FirstOrDefault(tc => tc.tileType == tileType && tc.tileDirection == tileDirection && tc.transitionType == transitionType);
        if (currentTile == null) return false;

        // Find the adjacency rule for the specified direction
        AdjacencyRule rule = currentTile.adjacencyRules.FirstOrDefault(r => r.direction == direction);
        if (rule == null) return false;

        // Check if the neighbor type, direction, and transition type are in the allowed list for this direction
        return rule.allowedTiles.Any(pair => pair.tileType == neighborType && pair.tileDirection == neighborDirection && pair.transitionType == neighborTransitionType);
    }
}
