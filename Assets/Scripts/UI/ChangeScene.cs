using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadBattleScene();
        }
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void ExitBattleScene()
    {
        SceneManager.LoadScene("BaseScene");
    }
}
