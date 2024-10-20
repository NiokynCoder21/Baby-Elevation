using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartGame : MonoBehaviour
{
    public void onContinue(InputAction.CallbackContext context)
    {
        Continue();
    }

    public void Continue()
    {
        SceneManager.LoadScene("Tutorial Level 1"); // Load Tutorial Level 1
    }
}
