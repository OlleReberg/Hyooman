using System.Collections.Generic;

[System.Serializable]
public class AdjacencyRule
{
    public TileDirectionType direction; // Direction of adjacency (North, South, East, West)
    public List<TileConstraint> allowedTiles; // List of specific TileConstraints that are allowed

    public AdjacencyRule(TileDirectionType direction)
    {
        this.direction = direction;
        this.allowedTiles = new List<TileConstraint>();
    }
}

[System.Serializable]
public class TileTypePair
{
    public TileType tileType;               // The primary type (e.g., Grass, Sand)
    public TileDirection tileDirection;     // Direction or position (e.g., Top, BottomLeft)
    public TransitionType transitionType;   // Secondary terrain type (e.g., Sand, Snow)
}

public enum TileType { None, Grass, Sand, Water, Snow, Swamp }

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

// Using TileDirectionType for adjacency rules
public enum TileDirectionType { NORTH, SOUTH, EAST, WEST }