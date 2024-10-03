using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawWorld : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap component
    private WFCGenerator wfcGenerator;

    public void InitializeWorld(WFCGenerator generator)
    {
        wfcGenerator = generator;
        tilemap.ClearAllTiles(); // Clear any existing tiles on the Tilemap
    }

    public void UpdateWorld()
    {
        for (int y = 0; y < wfcGenerator.gridHeight; y++)
        {
            for (int x = 0; x < wfcGenerator.gridWidth; x++)
            {
                int tileIndex = wfcGenerator.GetTileIndex(x, y);
                if (tileIndex < 0 || tileIndex >= wfcGenerator.tileConstraints.Length) continue;

                // Get the TileBase from the TileConstraint
                TileBase tile = wfcGenerator.tileConstraints[tileIndex].tile;

                // Set the tile in the Tilemap at the grid coordinates
                tilemap.SetTile(new Vector3Int(x, -y, 0), tile);
            }
        }
    }

    public void Draw()
    {
        // The tilemap automatically handles drawing, so nothing is needed here
    }
}




