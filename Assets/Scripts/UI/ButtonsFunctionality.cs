using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsFunctionality : MonoBehaviour
{
    public void PressedStartGame()
    {
        GameEngine.Instance.StartGame();
    }

    public void PressedRestart()
    {
        GameEngine.Instance.StartGame();
    }
}
