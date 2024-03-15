using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public Button StartButton;
    //private Button button; 
    public Image CountDown_Image;
    public Sprite[] CountDown_Array;
    void Start()
    {
        //button = GameObject.FindAnyObjectByType<Button>();
        StartButton.onClick.AddListener(StartCountdown);
    }

    void Update()
    {
        
    }

    void StartCountdown()
    {

        CountDown_Image.gameObject.SetActive(true);
        StartButton.gameObject.SetActive(false);
        
        StartCoroutine(CountDown());
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
