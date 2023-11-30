using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainManager : MonoBehaviour
{
    public IEnumerator StartGame()
    {
        yield return new WaitForFixedUpdate();
        GameManager.startGame();
    }
}
