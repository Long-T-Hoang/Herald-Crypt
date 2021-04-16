using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField]
    private GameObject instructionScreen;
    public GameObject mainScreen;

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
        SceneManager.LoadScene("TestScene");
    }

    public void OpenInstruction()
    {
        instructionScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void CloseInstruction()
    {
        instructionScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void ReturnToStartScreen()
    {
        SceneManager.LoadScene("StartScene");
    }
}
