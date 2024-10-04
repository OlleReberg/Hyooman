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
        List<float> tileWeights = new List<float>(); // For weighted random selection

        // Get neighboring tile types using public methods from WFCGenerator
        TileType aboveNeighborType = wfcGenerator.GetNeighborTileType(x, y + 1);
        TileType belowNeighborType = wfcGenerator.GetNeighborTileType(x, y - 1);
        TileType leftNeighborType = wfcGenerator.GetNeighborTileType(x - 1, y);
        TileType rightNeighborType = wfcGenerator.GetNeighborTileType(x + 1, y);

        // Dictionary to count neighboring biome types
        Dictionary<TileType, int> neighborBiomeCounts = new Dictionary<TileType, int>();

        // Helper method to count neighboring tiles
        void CountNeighbor(TileType neighborType)
        {
            if (neighborType == TileType.None) return; // Ignore empty spaces
            if (!neighborBiomeCounts.ContainsKey(neighborType))
            {
                neighborBiomeCounts[neighborType] = 0;
            }
            neighborBiomeCounts[neighborType]++;
        }

        // Count neighbors
        CountNeighbor(aboveNeighborType);
        CountNeighbor(belowNeighborType);
        CountNeighbor(leftNeighborType);
        CountNeighbor(rightNeighborType);

        // Loop through all tile constraints and check compatibility
        foreach (var constraint in wfcGenerator.tileConstraints)
        {
            if (constraint == null) continue;

            bool isCompatible = IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                                 aboveNeighborType, TileDirectionType.NORTH)
                                && IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                                    belowNeighborType, TileDirectionType.SOUTH)
                                && IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                                    leftNeighborType, TileDirectionType.WEST)
                                && IsTileCompatible(constraint.tileType, constraint.tileDirection, constraint.transitionType,
                                                    rightNeighborType, TileDirectionType.EAST);

            if (isCompatible)
            {
                // Add valid tiles and adjust weights based on biome influence
                if (wfcGenerator.tileTypeToIndex.TryGetValue(constraint.tileType, out List<int> indices))
                {
                    foreach (int index in indices)
                    {
                        validTiles.Add(index);

                        // Adjust weight: prioritize matching the most common neighboring biome
                        float weight = 1.0f; // Default weight
                        if (neighborBiomeCounts.ContainsKey(constraint.tileType))
                        {
                            weight += neighborBiomeCounts[constraint.tileType] * 0.5f; // Adjust the multiplier as needed
                        }

                        tileWeights.Add(weight);
                    }
                }
            }
        }

        // Select a tile randomly based on the calculated weights
        if (validTiles.Count > 0)
        {
            int selectedIndex = WeightedRandom(validTiles, tileWeights);
            return validTiles[selectedIndex];
        }

        // Fallback to a default tile if no valid tiles found
        return 0;
    }

    private int WeightedRandom(List<int> validTiles, List<float> weights)
    {
        float totalWeight = weights.Sum();
        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return i;
            }
        }
        return 0; // Fallback
    }

    private bool IsTileCompatible(TileType tileType, TileDirection tileDirection, TransitionType transitionType,
                                  TileType neighborType, TileDirectionType direction)
    {
        if (neighborType == TileType.None) return true; // No neighbor means compatibility by default

        // Find the current tile constraint
        TileConstraint currentTile = wfcGenerator.tileConstraints.FirstOrDefault(tc => tc.tileType == 
            tileType && tc.transitionType == transitionType);
        if (currentTile == null) return false;

        // Find the adjacency rule for the specified direction
        AdjacencyRule rule = currentTile.adjacencyRules.FirstOrDefault(r => r.direction == direction);
        if (rule == null) return false;

        // Check if the neighbor type, direction, and transition type are in the allowed list for this direction
        return rule.allowedTiles.Any(pair => pair.tileType == neighborType && pair.tileDirection == 
            tileDirection && pair.transitionType == transitionType);
    }
}


