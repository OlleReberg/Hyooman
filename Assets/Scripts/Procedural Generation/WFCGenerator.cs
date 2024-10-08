using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class WFCGenerator : MonoBehaviour
{
    public static WFCGenerator Instance { get; private set; }

    public int gridWidth = 10;
    public int gridHeight = 10;
    public List<TileConstraint> tileConstraints; // List of possible tile constraints

    public GridManager gridManager; // Reference to GridManager, set in the inspector
    [SerializeField] private DrawWorld drawWorld; // Reference to DrawWorld script

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize GridManager and pass in tile constraints
        gridManager.gridWidth = gridWidth;
        gridManager.gridHeight = gridHeight;
        gridManager.Initialize(tileConstraints.ToArray()); // Convert List to Array here

        // Generate Biomes before running the wave function collapse
        GenerateBiomes();

        // Run the WFC algorithm
        RunWaveFunctionCollapse();

        // Draw the world using DrawWorld
        if (drawWorld != null)
        {
            drawWorld.InitializeWorld(); // Draw the world
        }
        else
        {
            Debug.LogError("DrawWorld script reference is not set in WFCGenerator!");
        }
    }

    public void RunWaveFunctionCollapse()
    {
        // Initialize the collapse handler
        TileCollapseHandler collapseHandler = new TileCollapseHandler(this, gridManager);

        // Loop over all grid positions
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // Collapse the tile at position (x, y)
                int collapsedTileIndex = collapseHandler.CollapseTileAt(x, y);

                // Update the grid manager with the collapsed tile index
                gridManager.SetTileIndex(x, y, collapsedTileIndex);
            }
        }
    }
    private void GenerateBiomes()
    {
        // Define biome sizes relative to grid size
        int biomePatchSize = Mathf.Max(gridWidth, gridHeight) / 4;

        // Split grid into sections, and assign each section a biome
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                // Assign a biome based on grid position
                if (x / biomePatchSize % 2 == 0 && y / biomePatchSize % 2 == 0)
                {
                    gridManager.SetTileType(x, y, TileType.Grass); // Grass biome
                }
                else
                {
                    gridManager.SetTileType(x, y, TileType.Sand); // Sand biome
                }

                Debug.Log($"Biome generated at ({x}, {y}): {gridManager.GetTileTypeAt(x, y)}");
            }
        }
    }

    public TileConstraint GetTileConstraintByType(TileType tileType)
    {
        // Find the first TileConstraint that matches the given tileType
        foreach (TileConstraint constraint in tileConstraints)
        {
            if (constraint.tileType == tileType)
            {
                return constraint;
            }
        }

        return null;
    }
}

[System.Serializable]
public class Biome
{
    public TileType biomeType;
    public float biomeWeight; // Determines how much of the map this biome should cover
}












