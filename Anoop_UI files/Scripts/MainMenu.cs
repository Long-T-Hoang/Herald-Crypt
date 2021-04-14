using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button startBtn;
    public Text text;

    private Color textColor;

    // Start is called before the first frame update
    public void Start()
    {
        startBtn = gameObject.GetComponent<Button>();
        startBtn.onClick.AddListener(startOnClick);

        textColor = new Color(210f, 210f, 210f);
        text.color = textColor;
    }

    public void startOnClick(){
        SceneManager.LoadScene("SampleScene");
    }

    public void OnPointerEnter(PointerEventData eventData){
        text.color = Color.white;
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData){
        text.color = textColor;
        Debug.Log("Exit");
    }
}
