using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile
{
    // public List<TileConstraint> Possibilities; // Possible tile constraints for this tile
    // public int Entropy;
    // private Dictionary<TileDirectionType, Tile> neighbours;
    //
    // public Tile(int x, int y, List<TileConstraint> constraints)
    // {
    //     // Initialize possibilities with all available TileConstraints
    //     Possibilities = new List<TileConstraint>(constraints);
    //     Entropy = Possibilities.Count;
    //     neighbours = new Dictionary<TileDirectionType, Tile>();
    // }
    //
    // // Adds a neighbor in a specific direction
    // public void AddNeighbour(TileDirectionType direction, Tile tile)
    // {
    //     neighbours[direction] = tile;
    // }
    //
    // // Gets a neighbor in a specific direction
    // public Tile GetNeighbour(TileDirectionType direction)
    // {
    //     return neighbours.ContainsKey(direction) ? neighbours[direction] : null;
    // }
    //
    // // Get all directions that have neighbors
    // public List<TileDirectionType> GetDirections()
    // {
    //     return new List<TileDirectionType>(neighbours.Keys);
    // }
    //
    // // Retrieves the current list of possibilities for this tile
    // public List<TileConstraint> GetPossibilities()
    // {
    //     return Possibilities;
    // }
    //
    // // Collapse the tile to a single tile constraint
    // public void Collapse()
    // {
    //     // Select a random TileConstraint based on weights (you can customize this to account for weighted randomness)
    //     int selectedIndex = Random.Range(0, Possibilities.Count);
    //     TileConstraint selectedTile = Possibilities[selectedIndex];
    //
    //     // Set the possibilities to only the selected constraint
    //     Possibilities = new List<TileConstraint> { selectedTile };
    //     Entropy = 0; // Mark this tile as collapsed
    // }
    //
    // // Constrains this tile based on neighboring tile possibilities
    // public bool Constrain(List<TileConstraint> neighbourPossibilities, TileDirectionType direction)
    // {
    //     bool reduced = false;
    //
    //     if (Entropy > 0) // Only constrain tiles that haven't collapsed yet
    //     {
    //         // Collect valid connections based on neighboring possibilities
    //         List<TileConstraint> validConnections = new List<TileConstraint>();
    //
    //         foreach (TileConstraint neighborTile in neighbourPossibilities)
    //         {
    //             // Find the adjacency rule that applies for the specified direction
    //             var rule = neighborTile.adjacencyRules.Find(r => r.direction == direction);
    //
    //             if (rule != null)
    //             {
    //                 // Retrieve valid connections based on allowedTiles in the adjacency rules
    //                 validConnections.AddRange(
    //                     rule.allowedTiles.Select(pair => WFCGenerator.Instance.GetTileConstraintByType(pair.tileType))
    //                 );
    //             }
    //         }
    //
    //         // Remove invalid possibilities that do not match the valid connections
    //         for (int i = Possibilities.Count - 1; i >= 0; i--)
    //         {
    //             TileConstraint possibility = Possibilities[i];
    //             if (!validConnections.Contains(possibility))
    //             {
    //                 Possibilities.RemoveAt(i);
    //                 reduced = true;
    //             }
    //         }
    //
    //         Entropy = Possibilities.Count;
    //     }
    //
    //     return reduced;
    // }
    //
    // // Get the opposite direction (useful for bi-directional adjacency)
    // private TileDirectionType GetOppositeDirection(TileDirectionType direction)
    // {
    //     switch (direction)
    //     {
    //         case TileDirectionType.NORTH: return TileDirectionType.SOUTH;
    //         case TileDirectionType.EAST: return TileDirectionType.WEST;
    //         case TileDirectionType.SOUTH: return TileDirectionType.NORTH;
    //         case TileDirectionType.WEST: return TileDirectionType.EAST;
    //         default: return TileDirectionType.NORTH; // Default case, though this shouldn't happen
    //     }
    // }
}

