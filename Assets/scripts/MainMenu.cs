using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //skrypty aktywowane poprzez guziki w menu glownym:

    //nowa gra
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //zaladowanie gry z zapisu, jezeli zapis nie istnieje, to zaczyna nowa gre
    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    //wyjscie z gry
    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
