using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Begin1 : MonoBehaviour
{
    public void onContinue(InputAction.CallbackContext context)
    {
        BeginGame();
    }


    void BeginGame()
    {
        SceneManager.LoadScene("Tutorial Level 3", LoadSceneMode.Single); //load Tutorial Level 3
    }
}
