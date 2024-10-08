using System.Collections.Generic;
using UnityEngine;


// Used for base terrain tiles in WFCGenerator
[CreateAssetMenu(menuName = "Tile/Constraint")]
public class TileConstraint : BaseTile
{
    public TransitionType transitionType; // Tile's transition type (e.g., Grass to Sand)
    public TileDirection tileDirection; // Tile's direction
    public bool canRotate; // Can this tile rotate
    public List<int> allowedRotations; // Allowed rotations
    public List<AdjacencyRule> adjacencyRules; // Adjacency rules referencing specific TileConstraints
}

