using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonCallbacks : MonoBehaviour
{
    public bool isPaused = false;
    public bool isOver = false;

    public GameObject mainmenuui;
    public GameObject creditsui;

    public GameObject gameoverui;
    public GameObject gameui;
    public GameObject pauseui;
    public GameObject gamelostui;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Barnyard");
    }

    public void ShowCredits()
    {
        mainmenuui.SetActive(false);
        creditsui.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainmenuui.SetActive(true);
        creditsui.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameoverui.SetActive(true);
        gameui.SetActive(false);
        Time.timeScale = 0.0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        isOver = true;
        Cursor.visible = true;
    }

    public void ShowGameLost()
    {
        gamelostui.SetActive(true);
        gameui.SetActive(false);
        Time.timeScale = 0.0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        isOver = true;
        Cursor.visible = true;
    }

    public void Pause()
    {
        gameui.SetActive(false);
        pauseui.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPause()
    {
        gameui.SetActive(true);
        pauseui.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Title");
    }
}
