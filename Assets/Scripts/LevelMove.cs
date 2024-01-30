using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            SceneManager.LoadScene("Maze");
        }
        else if (col.gameObject.tag == "c")
        {
            SceneManager.LoadScene("End");
        }
        else if (col.gameObject.tag == "w")
        {
            SceneManager.LoadScene("End");
        }
        else if (col.gameObject.tag == "r")
        {
            SceneManager.LoadScene("End");
        }
    }
}
