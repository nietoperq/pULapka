using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;

    public GameObject pauseMenuUI;

    private void Start()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        //wlaczenie menu klawiszem ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //wylaczenie ekranu menu, ustawienie predkosci czasu na 100%
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        //wlaczenie ekranu menu, ustawienie predkosci czasu na 0% (wszystkie aktywnosci w grze sa zatrzymane)
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GoToMainMenu()
    {
        //przejscie do sceny z menu glownym
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        //wyjscie z gry
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
