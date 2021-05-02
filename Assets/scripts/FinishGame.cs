using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public GameObject finishGameUI;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            finishGameUI.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("finish game");
        }
    }
}


