using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonPool : MonoBehaviour
{
    public GameObject balloonPrefab; // Reference to the balloon prefab
    public int initialPoolSize = 15; // Number of balloons in the pool
    private List<GameObject> balloonPool; // List to hold pooled balloons

    void Start()
    {
        // Initialize the balloon pool
        balloonPool = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject balloon = Instantiate(balloonPrefab);
            balloon.SetActive(false); // Deactivate the balloon by default
            balloonPool.Add(balloon);
        }
    }

    // Get a balloon from the pool
    public GameObject GetBalloon(Vector3 position)
    {
        foreach (GameObject balloon in balloonPool)
        {
            if (!balloon.activeInHierarchy) // Find an inactive balloon
            {
                balloon.transform.position = position;
                balloon.SetActive(true); // Activate the balloon
                return balloon;
            }
        }

        // If all balloons are used, optionally instantiate more
        GameObject newBalloon = Instantiate(balloonPrefab);
        newBalloon.transform.position = position;
        balloonPool.Add(newBalloon);
        return newBalloon;
    }

    // Return the balloon back to the pool
    public void ReturnBalloon(GameObject balloon)
    {
        balloon.SetActive(false);
    }
}
