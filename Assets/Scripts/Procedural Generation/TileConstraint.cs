using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile Constraint")]
public class TileConstraint : ScriptableObject
{
    public TileBase tile;
    public TileType tileType;    
    public TransitionType transitionType;  // Secondary terrain type (e.g., Sand, Snow)// Primary biome type (e.g., Grass)
    public TileDirection tileDirection;    // The direction or position (e.g., Bottom)
    

    // Adjacency rules now include both TileType, TileDirection, and TransitionType combinations
    public List<AdjacencyRule> adjacencyRules;
    private void OnEnable()
    {
        // Ensure the adjacency rules list is initialized
        if (adjacencyRules == null)
        {
            adjacencyRules = new List<AdjacencyRule>();
        }

        // Initialize each adjacency rule's allowedTiles list
        foreach (var rule in adjacencyRules)
        {
            if (rule.allowedTiles == null)
            {
                rule.allowedTiles = new List<TileTypePair>();
            }
        }
    }
}

[System.Serializable]
public class AdjacencyRule
{
    public Direction direction; // Use "Above", "Below", "ToTheLeft", "ToTheRight"
    public List<TileTypePair> allowedTiles; // List of allowed TileType, TileDirection, and TransitionType combinations
}

[System.Serializable]
public class TileTypePair
{
    public TileType tileType;               // The primary type (e.g., Grass, Sand)
    public TileDirection tileDirection;     // Direction or position (e.g., Top, BottomLeft)
    public TransitionType transitionType;   // Secondary terrain type (e.g., Sand, Snow)
    
}

public enum TileType
{
    None,
    Grass,
    Sand,
    Water,
    Snow,
    Swamp
}
public enum TileDirection
{
    Middle,       // Center tile with no specific edge
    Top,
    Bottom,
    Left,
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}
public enum TransitionType
{
    None, // No transition (e.g., plain grass, sand)
    Grass,
    Sand,
    Water,
    Snow,
    Swamp
}






