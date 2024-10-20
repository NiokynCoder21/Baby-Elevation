using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RedoGame : MonoBehaviour
{

    private const string PlayerDeathSceneKey = "PlayerDeathScene"; //this is used to store data when the player dies 

    public void OnRetry(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RespawnPlayer();
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown("r")) //when the player presses A
        {
            RespawnPlayer();
        }*/
    }

    public void PlayerDied(string sceneName) //when player dies this , this is called in the set scence script to say where the player died
    {
        PlayerPrefs.SetString(PlayerDeathSceneKey, sceneName); //when the player dies it saves the scence they died in
    }

    public void RespawnPlayer() //respawns player based on set scence
    {
        string lastDeathScene = PlayerPrefs.GetString(PlayerDeathSceneKey, "Tutorial Level 1"); //defult scence

        SceneManager.LoadScene(lastDeathScene, LoadSceneMode.Single); //load from the last scence the player died
    }
}
