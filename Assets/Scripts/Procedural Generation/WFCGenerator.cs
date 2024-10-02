using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class WFCGenerator : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public TileConstraint[] tileConstraints;
    private NativeArray<int> grid; // Grid cells will store tile indices

    void Start()
    {
        InitializeGrid();
        RunWaveFunctionCollapse();
    }

    void InitializeGrid()
    {
        grid = new NativeArray<int>(gridWidth * gridHeight, Allocator.TempJob);

        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = -1; // Initialize all cells as "uncollapsed"
        }
    }

    void RunWaveFunctionCollapse()
    {
        var wfcJob = new WaveFunctionCollapseJob
        {
            grid = grid,
            gridWidth = gridWidth,
            gridHeight = gridHeight,
            tileCount = tileConstraints.Length
        };

        JobHandle handle = wfcJob.Schedule();
        handle.Complete();
        ApplyGrid();
        grid.Dispose();
    }

    void ApplyGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int tileIndex = grid[x + y * gridWidth];
                if (tileIndex >= 0)
                {
                    Vector3 position = new Vector3(x, y, 0);
                    Instantiate(tileConstraints[tileIndex].tilePrefab, position, Quaternion.identity);
                }
            }
        }
    }

    [BurstCompile]
    private struct WaveFunctionCollapseJob : IJob
    {
        public NativeArray<int> grid;
        public NativeArray<bool> possibleTiles; // Tracks possible tiles for each grid cell
        public int gridWidth;
        public int gridHeight;
        public int tileCount;

        public void Execute()
        {
            // Example: Implement the WFC logic here, using constraints to collapse cells
            for (int i = 0; i < grid.Length; i++)
            {
                // Find the cell with the lowest entropy
                int cellIndex = FindLowestEntropyCell();
                if (cellIndex == -1) break; // If all cells are collapsed

                // Collapse the cell to a tile
                int selectedTile = RandomTile(); // Placeholder: Select based on possibleTiles constraints
                grid[cellIndex] = selectedTile;

                // Propagate constraints to neighbors
                PropagateConstraints(cellIndex);
            }
        }

        private int FindLowestEntropyCell()
        {
            // Logic to find the cell with the fewest possible tiles
            return 0; // Placeholder
        }

        private void PropagateConstraints(int cellIndex)
        {
            // Logic to update neighboring cells based on constraints
        }

        private int RandomTile()
        {
            return UnityEngine.Random.Range(0, tileCount);
        }
    }
}

