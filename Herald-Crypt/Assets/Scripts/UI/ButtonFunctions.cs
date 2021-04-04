using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField]
    private GameObject instructionScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenInstruction()
    {
        instructionScreen.SetActive(true);
    }

    public void CloseInstruction()
    {
        instructionScreen.SetActive(false);
    }

    public void ReturnToStartScreen()
    {
        SceneManager.LoadScene(0);
    }
}
