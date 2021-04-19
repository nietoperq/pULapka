using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private object[] obj = { };

    public void SaveGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("WasGameSaved", 1);
        if (obj.Length > 0)
        {
            System.Array.Clear(obj, 0, obj.Length);
        }
        obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.activeInHierarchy)
            {
                if (g.tag == "collectable" || g.tag == "enemy" || g.tag == "healthpotion")
                {
                    //zapisywanie obiektow 
                    PlayerPrefs.SetInt(g.name, 1);
                }
                if (g.tag == "player")
                {
                    //zapis pozycji gracza
                    PlayerPrefs.SetFloat("PlayerPositionX", g.transform.position.x);
                    PlayerPrefs.SetFloat("PlayerPositionY", g.transform.position.y);
                }
            }
        }
        //zapis zycia i punktow
        PlayerPrefs.SetInt("health", this.GetComponent<PlayerController>().getHealth());
        PlayerPrefs.SetInt("ects", this.GetComponent<PlayerController>().getECTS());
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("WasGameSaved"))
        {
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;

                if (g.tag == "collectable" || g.tag == "enemy" || g.tag == "healthpotion")
                {
                    if (!PlayerPrefs.HasKey(g.name))
                    {
                        //usuwanie obiektow ktorych nie ma na liscie zapisanych obiektow
                        Destroy(g);
                    }
                }
                if (g.tag == "player")
                {
                    float posX = PlayerPrefs.GetFloat("PlayerPositionX");
                    float posY = PlayerPrefs.GetFloat("PlayerPositionY");
                    g.transform.position = new Vector2(posX, posY + 1);
                }
            }
        }
    }
}
