using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public WFCGenerator wfcGenerator; // Reference to the WFCGenerator in the scene
    private DrawWorld drawWorld;

    private bool done = false;
    public bool interactive = true;
    public bool interactiveKeyPress = false;

    void Start()
    {
        // Find the DrawWorld component in the scene
        drawWorld = FindObjectOfType<DrawWorld>();

        if (drawWorld == null)
        {
            Debug.LogError("DrawWorld component not found in the scene!");
            return;
        }

        if (wfcGenerator == null)
        {
            Debug.LogError("WFCGenerator component not assigned in the inspector!");
            return;
        }

        // Initialize the world visuals using WFCGenerator after a short delay
        StartCoroutine(InitializeAndRunWFC());
    }

    private IEnumerator InitializeAndRunWFC()
    {
        // Delay to ensure all components are ready (important in complex scenes)
        yield return new WaitForEndOfFrame();

        // Initialize the world visuals using WFCGenerator
        drawWorld.InitializeWorld(wfcGenerator);

        // Run initial wave function collapse
        wfcGenerator.RunWaveFunctionCollapse();
        drawWorld.UpdateWorld();
    }

    void Update()
    {
        // Handle User Input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (interactive && interactiveKeyPress && !done)
            {
                wfcGenerator.RunWaveFunctionCollapse();
                drawWorld.UpdateWorld();
            }
        }

        // Continuous Collapse if not in interactive key press mode
        if (interactive && !interactiveKeyPress && !done)
        {
            wfcGenerator.RunWaveFunctionCollapse();
            drawWorld.UpdateWorld();
            done = true; // Prevent re-running collapse unless needed
        }

        drawWorld.Draw();
    }
}

