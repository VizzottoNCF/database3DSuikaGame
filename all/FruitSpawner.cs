using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public bool gameLost = false;
    public GameObject[] fruitPrefabs;  // Array of fruit prefabs to spawn
    public Transform spawnPoint;       // Position where fruits spawn
    public float spawnDelay = 1.0f;    // Delay between spawns

    public bool CalledFruit = false;

    public GameObject currentFruit;

    private Score scr;

    void Start()
    {
        scr = GameObject.FindWithTag("Event").GetComponent<Score>();
        SpawnFruit();
    }

    void Update()
    {
        if (!gameLost)
        {
            // Check if current fruit has been thrown (i.e., it has fallen away)
            if (currentFruit == null && !CalledFruit)
            {
                // sets bool to true to not spawn various fruits
                CalledFruit = true;

                // Delay the spawn to give a moment before the next fruit appears
                Invoke(nameof(SpawnFruit), spawnDelay);
            }
            else if (currentFruit != null && currentFruit.GetComponent<Fruit>().IsHeld)
            {
                currentFruit.transform.position = spawnPoint.position;
            }
            else if (currentFruit != null && !currentFruit.GetComponent<Fruit>().IsHeld && !CalledFruit)
            {
                // sets bool to true to not spawn various fruits
                CalledFruit = true;

                // Delay the spawn to give a moment before the next fruit appears
                Invoke(nameof(SpawnFruit), spawnDelay);
            }
        }
        else
        {
            if (currentFruit != null)
            {
                scr.rf_OnGameEnd();
                Destroy(currentFruit);
            }
        }
    }

    void SpawnFruit()
    {
        // Pick a random fruit prefab from the array
        int randomIndex = Random.Range(0, fruitPrefabs.Length);

        // Spawn the fruit at the spawn point
        currentFruit = Instantiate(fruitPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        // sets bool back to false so it can call more fruits once it needs
        CalledFruit = false;
    }
}
