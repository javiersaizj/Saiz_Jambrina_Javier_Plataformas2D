using System.Collections.Generic;
using UnityEngine;

public class EntityResetManager : MonoBehaviour
{
    public static EntityResetManager Instance;

    private List<GameObject> entities = new List<GameObject>();
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEntity(GameObject entity)
    {
        if (!entities.Contains(entity))
        {
            entities.Add(entity);
            initialPositions[entity] = entity.transform.position;
        }
    }

    public void ResetEntities()
    {
        foreach (var entity in entities)
        {
            if (entity != null)
            {
                entity.transform.position = initialPositions[entity];
                entity.SetActive(true); 
            }
        }
    }
}
