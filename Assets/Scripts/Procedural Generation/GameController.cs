using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GridManager gridManager;
    [SerializeField] private WFCGenerator wfcGenerator; // Reference to the WFCGenerator (set in the inspector)
    [SerializeField] private TilePostProcessor tilePostProcessor; // Reference to the TilePostProcessor (set in the inspector)
    [SerializeField] private ObjectPlacementHandler objectPlacementHandler; // Reference to ObjectPlacementHandler (set in the inspector)

    private void Start()
    {
        // Start the terrain generation process
        StartCoroutine(InitializeAndGenerateWorld());
    }

    private IEnumerator InitializeAndGenerateWorld()
    {
        // Wait for a frame to ensure all components are ready
        yield return new WaitForEndOfFrame();

        // Run the wave function collapse to generate the base terrain
        wfcGenerator.RunWaveFunctionCollapse();

        // Post-process tiles to add transitions and decorations
        tilePostProcessor.ProcessTiles();

        // Place decorative objects and tall grass
        objectPlacementHandler.ProcessAndPlaceObjects();
    }

    private void Update()
    {
        // Handle User Input (Optional: For debugging and additional interactions)
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reset the world and regenerate it
            ResetWorld();
        }
    }
    private void ResetWorld()
    {
        // Clear the existing world (clear all tilemaps, objects, etc.)
        //gridManager.ClearGrid(); 
        tilePostProcessor.ClearTiles(); 
        objectPlacementHandler.ClearObjects(); 

        // Regenerate the world
        StartCoroutine(InitializeAndGenerateWorld());
    }
}




