using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private string level1 = "Level 1";

    public void StartGame()
    {
        SceneManager.LoadScene("MoveStopMove");
    }
}
