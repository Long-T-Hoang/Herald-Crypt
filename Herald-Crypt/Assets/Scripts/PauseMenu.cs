using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;

    public Button resumeBtn;
    public Button quitBtn;

    private GameObject player;
    private PlayerAttack playerScript;

    void Start(){
        resumeBtn.onClick.AddListener(Resume);
        quitBtn.onClick.AddListener(quitClick);
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerAttack>();
    }

    public void quitClick(){
        Resume();
        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape)){
            if(gamePaused){
                Resume();
            }
            else {
                Pause();
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Space pressed");
        }
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        playerScript.paused = true;
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        playerScript.paused = false;
    }
}
