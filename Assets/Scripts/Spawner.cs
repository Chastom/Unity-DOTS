using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    public GameObject Prefab;
    public int CountX = 100;
    public int CountY = 100;

    void Start()
    {
        Random random = new Random();

        // Create entity prefab from the game object hierarchy once
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        for (var x = 0; x < CountX; x++)
        {            
            for (var y = 0; y < CountY; y++)
            {
                // Efficiently instantiate a bunch of entities from the already converted entity prefab
                var instance = entityManager.Instantiate(prefab);

                // Place the instantiated entity in a grid with some noise
                var position = transform.TransformPoint(new float3(x * 2.5F, noise.cnoise(new float2(x, y) * 0.21F) * 2, y * 2.5F));
                entityManager.SetComponentData(instance, new Translation { Value = position });
                entityManager.AddComponentData(instance, new SpawnTime { Value = random.Next(1, 20) });
            }
        }
    }
}
