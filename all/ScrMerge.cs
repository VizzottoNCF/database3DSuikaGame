using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrMerge : MonoBehaviour
{
    public GameObject nextFruit;     // Prefab of the fruit to spawn after merging
    public float mergeOffset = 0.5f; // Offset for spawning the new fruit
    public int value; // set in prefabs
    private Score scr;

    private AudioSource pop;

    private bool isMerging = false;  // Prevents multiple merges at once

    private void Start()
    {
        pop = GetComponent<AudioSource>();
        scr = GameObject.FindWithTag("Event").GetComponent<Score>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if collision is with another fruit of the same type
        if (collision.gameObject.CompareTag(gameObject.tag) && !isMerging)
        {
            var otherFruit = collision.gameObject.GetComponent<ScrMerge>();
            if (otherFruit != null && !otherFruit.isMerging)
            {

                pop.Play();
                // Mark both fruits as merging to avoid duplicate merges
                isMerging = true;
                otherFruit.isMerging = true;

                // Calculate position for new fruit to spawn (center between colliding fruits)
                Vector3 spawnPosition = (transform.position + collision.transform.position) / 2 + Vector3.up * mergeOffset;

                // Spawn the next level fruit
                var fruit = Instantiate(nextFruit, spawnPosition, Quaternion.identity);

                fruit.GetComponent<Fruit>().IsHeld = false;

                scr.rf_AddToScore(value);
                
                // Destroy both merging fruits
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}
