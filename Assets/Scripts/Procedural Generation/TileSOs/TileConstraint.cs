using System.Collections.Generic;
using UnityEngine;


// Used for base terrain tiles in WFCGenerator
[CreateAssetMenu(menuName = "Tile/Constraint")]
public class TileConstraint : BaseTile
{ 
    public TransitionType transitionType; // Add back this property to indicate what this tile can transition into
    public TileDirection tileDirection; // Direction of the tile (e.g., for edges)
    public bool canRotate; // Can this tile rotate?
    public List<int> allowedRotations; // Allowed rotations
    public List<AdjacencyRule> adjacencyRules; // Adjacency rules
       
}
