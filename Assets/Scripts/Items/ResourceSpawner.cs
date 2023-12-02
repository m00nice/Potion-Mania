using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : InteractableObjectSuper
{

    [SerializeField] private ResourceObject[] resourceArray;
    [SerializeField] private GameObject spawnpoint;
    
    private ResourceObject resource;
    private GameObject resourcePrefab;
    private bool isSpawning;
    
    void Start()
    {
        StartCoroutine(TimedSpawn());
    }

    
    void Update()
    {
        
    }

    private void SpawnNewResource()
    {
        resource = resourceArray[Random.Range(0, resourceArray.Length)];
        resourcePrefab = Instantiate(resource.Prefab, spawnpoint.transform);
    }
    

    public override void Interact(Player player)
    {
        if (resource != null)
        {
            resource.Amount++;
            Destroy(resourcePrefab);
            resourcePrefab = null;
            resource = null;
        }
    }

    private IEnumerator TimedSpawn()
    {
        while (Application.isPlaying)
        {
            
            if (resource == null)
            {
                SpawnNewResource();
            }
            yield return new WaitForSeconds(5);
        }
    }
    
    
    
    
}
