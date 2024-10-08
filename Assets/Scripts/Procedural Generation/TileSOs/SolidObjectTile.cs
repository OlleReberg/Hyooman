using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile/SolidObject")]
public class SolidObjectTile : BaseTile
{
    [System.Serializable]
    public struct TileData
    {
        public TileBase tile;       // The tile for a specific part of the object
        public Vector2Int position; // The relative position of this tile
    }

    public List<TileData> tiles; // List of tiles that form the entire object

    private void OnEnable()
    {
        // Ensure the list is initialized
        if (tiles == null)
        {
            tiles = new List<TileData>();
        }
    }
}