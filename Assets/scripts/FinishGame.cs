using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public GameObject finishGameUI;
    public PlayerController player;
    public Text textECTS;
    [SerializeField] private AudioSource winSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            StartCoroutine(IFinishGame());
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    IEnumerator IFinishGame()
    {
        winSound.Play();
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 0f;
        finishGameUI.SetActive(true);
        Time.timeScale = 0f;
        textECTS.text = player.getECTS() + " ECTS";
    }

}


