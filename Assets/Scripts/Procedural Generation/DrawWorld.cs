using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawWorld : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap component
    private WFCGenerator wfcGenerator;

    public void InitializeWorld(WFCGenerator generator)
    {
        // Set the reference to WFCGenerator
        wfcGenerator = generator;
        if (generator == null)
        {
            Debug.LogError("WFCGenerator passed to InitializeWorld is null!");
            return;
        }

        wfcGenerator = generator;

        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned in the DrawWorld script!");
            return;
        }

        tilemap.ClearAllTiles(); // Clear any existing tiles on the Tilemap
    }

    public void UpdateWorld()
    {
        if (wfcGenerator == null)
        {
            Debug.Log("Can't find wfcGenerator while updating world, returning");
            return;
        }

        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned in DrawWorld!");
            return;
        }

        for (int y = 0; y < wfcGenerator.gridHeight; y++)
        {
            for (int x = 0; x < wfcGenerator.gridWidth; x++)
            {
                int tileIndex = wfcGenerator.GetTileIndex(x, y);
                if (tileIndex < 0 || tileIndex >= wfcGenerator.tileConstraints.Length) continue;

                TileConstraint tileConstraint = wfcGenerator.tileConstraints[tileIndex];
                TileBase tile = tileConstraint?.tile;

                if (tile == null) continue;

                // Set the rotation on the Tilemap
                Quaternion rotation = Quaternion.Euler(0, 0, tileConstraint.allowedRotations[0]); // Assuming the first rotation in allowedRotations is used
                Matrix4x4 tileRotation = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);

                // Set the tile with rotation
                tilemap.SetTransformMatrix(new Vector3Int(x, -y, 0), tileRotation);
                tilemap.SetTile(new Vector3Int(x, -y, 0), tile);
            }
        }
    }


    public void Draw()
    {
        // The tilemap automatically handles drawing, so nothing is needed here
    }
}



