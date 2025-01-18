using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemigos : MonoBehaviour
{
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private float tiempoEntreSpawns;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > tiempoEntreSpawns)
        {
            Instantiate(enemigoPrefab, new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0), Quaternion.identity);
            timer = 0f;
        }
        
    }
}
