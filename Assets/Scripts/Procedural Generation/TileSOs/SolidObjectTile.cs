using UnityEngine;

[CreateAssetMenu(menuName = "Tile/SolidObject")]
// Used for solid objects (trees, rocks, etc.)
public class SolidObjectTile : BaseTile
{
    public bool isCollidable; // Does this object block player movement?
}