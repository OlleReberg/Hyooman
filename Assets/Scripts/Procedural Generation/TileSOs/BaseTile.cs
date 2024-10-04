using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile/BaseTile")]
// Base class for all tile types
public class BaseTile : ScriptableObject
{
    public TileBase tile; // Reference to the Unity Tile asset
    public TileType tileType;
    public float weight = 1.0f; // Default weight for tile placement
}












