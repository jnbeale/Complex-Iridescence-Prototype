using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    //pause button activates menu
    public GameObject _pauseMenu;
    public bool isPaused;

    float selection;

    //[Header ("Continue")]
    //public GameObject ContinueSprite;
    //public GameObject ContinueSelected;

    //[Header ("Quit")]
    //public GameObject QuitSprite;
    //public GameObject QuitSelected;


    void Start()
    {
        _pauseMenu.SetActive(false);
        selection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void PauseGame()
    {
        if (!_pauseMenu.activeSelf){
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
         if (_pauseMenu.activeSelf){
            _pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
         }
         else{
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
         }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
