using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    public Button StartButton;                  // Button to start Countdown
    public Button ControlsButton;               // Button to show Controls
    public Image Title_Image;                   // Image where the Title shows
    public Image CountDown_Image;               // Image where the Countdown shows
    public Image WASD_Image;                    // Image where the WASD Keys shows
    public Image LeftClick_Image;               // Image where the Arrow Keys shows
    public Text Controls_Text;                  // Textbox that show what the controls do
    public Button BackButton;                   // Button to go back to the Main Menu
    public Sprite[] CountDown_Array;            // Array of sprites for the Countdown
    void Start()
    {
        StartButton.onClick.AddListener(StartCountdown);
        ControlsButton.onClick.AddListener(ShowControls);
    }

    void StartCountdown()
    {
        CountDown_Image.gameObject.SetActive(true);
        StartButton.gameObject.SetActive(false);
        ControlsButton.gameObject.SetActive(false);
        Title_Image.gameObject.SetActive(false);
        
        StartCoroutine(CountDown());
    }

    void ShowControls()
    {
        StartButton.gameObject.SetActive(false);
        ControlsButton.gameObject.SetActive(false);
        Title_Image.gameObject.SetActive(false);
        WASD_Image.gameObject.SetActive(true);
        LeftClick_Image.gameObject.SetActive(true);
        Controls_Text.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
        BackButton.onClick.AddListener(BackToMenu);
    }

    void BackToMenu()
    {
        StartButton.gameObject.SetActive(true);
        ControlsButton.gameObject.SetActive(true);
        Title_Image.gameObject.SetActive(true);
        WASD_Image.gameObject.SetActive(false);
        LeftClick_Image.gameObject.SetActive(false);
        Controls_Text.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
    }

    IEnumerator CountDown()
    {
        for (int i = 0; i < CountDown_Array.Length; i++)
        {
            CountDown_Image.sprite = CountDown_Array[i];
            yield return new WaitForSeconds(1);
        }

        SceneManager.LoadScene("Level 1");
    }
}
