using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    //pause button activates menu
    public GameObject _pauseMenu;
    public bool isPaused;

    [Header ("Continue")]
    public GameObject ContinueSprite;
    public GameObject ContinueSelected;

    [Header ("Quit")]
    public GameObject QuitSprite;
    public GameObject QuitSelected;


    void Start()
    {
        _pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
