using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    //pause button activates menu
    public GameObject _pauseMenu;

    public GameObject _healthBar;
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
        _healthBar = GameObject.Find("HealthBar");
        _pauseMenu.SetActive(false);
        _healthBar.SetActive(true);
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
            _healthBar.SetActive(false);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        //_healthBar.SetActive(true);
         if (_pauseMenu.activeSelf){
            _healthBar.SetActive(true);
            _pauseMenu.SetActive(false);
            //_healthBar.SetActive(true);
            Time.timeScale = 1f;
            isPaused = false;
         }
         else{
            _pauseMenu.SetActive(true);
            _healthBar.SetActive(false);
            Time.timeScale = 0f;
            isPaused = true;
         }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
