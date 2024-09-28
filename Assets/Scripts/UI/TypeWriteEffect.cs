using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TypeWriteEffect : MonoBehaviour
{
    [SerializeField] private float delay = 0.1f;
    [TextArea]
    [SerializeField] private string fullText;
    private string currentText = "";
   // [SerializeField] private Text text;
    
    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("BaseScene");
    }
}
