using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectPlacementHandler : MonoBehaviour
{
    [SerializeField] private WFCGenerator wfcGenerator; // Reference to WFCGenerator (set in inspector)
    [SerializeField] private Tilemap solidObjectsTilemap; // Reference to the Solid Objects Tilemap
    [SerializeField] private Tilemap tallGrassTilemap;   // Reference to the Tall Grass Tilemap

    // Example method to process and place objects
    public void ProcessAndPlaceObjects()
    {
        for (int y = 0; y < wfcGenerator.gridHeight; y++)
        {
            for (int x = 0; x < wfcGenerator.gridWidth; x++)
            {
                PlaceDecorativeObject(x, y);
                PlaceTallGrass(x, y);
            }
        }
    }

    private void PlaceDecorativeObject(int x, int y)
    {
        TileType currentTileType = wfcGenerator.GetNeighborTileType(x, y);

        // Example logic to place objects based on tile type
        if (currentTileType == TileType.Grass)
        {
            GameObject prefab = GetRandomDecorativeObject(); // Implement a method to get a decorative object prefab
            if (prefab != null)
            {
                Vector3 position = solidObjectsTilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }

    private void PlaceTallGrass(int x, int y)
    {
        TileType currentTileType = wfcGenerator.GetNeighborTileType(x, y);

        // Example logic to place tall grass on grass tiles
        if (currentTileType == TileType.Grass)
        {
            TileBase tallGrassTile = GetRandomTallGrassTile(); // Implement a method to get a tall grass tile
            if (tallGrassTile != null)
            {
                tallGrassTilemap.SetTile(new Vector3Int(x, y, 0), tallGrassTile);
            }
        }
    }

    private GameObject GetRandomDecorativeObject()
    {
        // Placeholder for logic to get a random decorative object prefab
        return null;
    }

    private TileBase GetRandomTallGrassTile()
    {
        // Placeholder for logic to get a random tall grass tile
        return null;
    }
    
    public void ClearObjects()
    {
        // Destroy all placed objects (e.g., trees, tall grass)
        foreach (Transform child in solidObjectsTilemap.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in tallGrassTilemap.transform)
        {
            Destroy(child.gameObject);
        }
    }

}

