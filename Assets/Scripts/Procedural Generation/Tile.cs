using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile
{
    public List<TileConstraint> Possibilities; // Changed to use TileConstraint instead of strings
    public int Entropy;
    private Dictionary<Direction, Tile> neighbours;

    public Tile(int x, int y)
    {
        // Initialize possibilities with all available TileConstraints
        Possibilities = new List<TileConstraint>(WFCGenerator.Instance.tileConstraints);
        Entropy = Possibilities.Count;
        neighbours = new Dictionary<Direction, Tile>();
    }

    public void AddNeighbour(Direction direction, Tile tile)
    {
        neighbours[direction] = tile;
    }

    public Tile GetNeighbour(Direction direction)
    {
        return neighbours.ContainsKey(direction) ? neighbours[direction] : null;
    }

    public List<Direction> GetDirections()
    {
        return new List<Direction>(neighbours.Keys);
    }

    public List<TileConstraint> GetPossibilities()
    {
        return Possibilities;
    }

    public void Collapse()
    {
        // Select a random TileConstraint based on custom logic, such as weights
        int selectedIndex = Random.Range(0, Possibilities.Count);
        TileConstraint selectedTile = Possibilities[selectedIndex];
        Possibilities = new List<TileConstraint> { selectedTile };
        Entropy = 0; // Mark this tile as collapsed
    }

    public bool Constrain(List<TileConstraint> neighbourPossibilities, Direction direction)
    {
        bool reduced = false;

        if (Entropy > 0) // If this tile hasn't collapsed yet
        {
            // Get possible connections from neighbors
            List<TileConstraint> validConnections = new List<TileConstraint>();
            foreach (TileConstraint neighborTile in neighbourPossibilities)
            {
                // Find the adjacency rules for the specified direction
                var rule = neighborTile.adjacencyRules.Find(r => r.direction == direction);

                if (rule != null)
                {
                    // Use WFCGenerator.Instance to get tile constraints based on the tileType
                    validConnections.AddRange(
                        rule.allowedTiles.Select(pair => WFCGenerator.Instance.GetTileConstraintByType(pair.tileType))
                    );
                }
            }

            // Filter current possibilities based on neighbor constraints
            for (int i = Possibilities.Count - 1; i >= 0; i--)
            {
                TileConstraint possibility = Possibilities[i];
                if (!validConnections.Contains(possibility))
                {
                    Possibilities.RemoveAt(i);
                    reduced = true;
                }
            }

            Entropy = Possibilities.Count;
        }

        return reduced;
    }


    private Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH: return Direction.SOUTH;
            case Direction.EAST: return Direction.WEST;
            case Direction.SOUTH: return Direction.NORTH;
            case Direction.WEST: return Direction.EAST;
            default: return Direction.NORTH; // Default case, should not happen
        }
    }
}

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

