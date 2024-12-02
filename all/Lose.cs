using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour
{
    private GameObject main;

    private void Start()
    {
        main = GameObject.FindWithTag("MainCamera");
        Debug.Log(main);
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);

        main.GetComponent<FruitSpawner>().gameLost = true;


    }
}
