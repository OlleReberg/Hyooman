using System.Collections.Generic;

public class AdjacencyRule
{
    public TileDirectionType direction; // Use the new TileDirectionType enum instead of the ambiguous Direction
    public List<TileTypePair> allowedTiles; // List of allowed TileType, TileDirection, and TransitionType combinations
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

// New enum to avoid ambiguity
public enum TileDirectionType { NORTH, SOUTH, EAST, WEST }