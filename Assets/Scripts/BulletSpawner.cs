using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject Prefab;

    //private EntityManager entityManager;
    //private Entity prefab;

    void Start()
    {
        //var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        //prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);
        //entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            Shoot();
        }
    }


    void Shoot()
    {
        //var instance = entityManager.Instantiate(prefab);

        //var position = transform.position;
        //entityManager.SetComponentData(instance, new Translation { Value = position });
        //entityManager.SetComponentData(instance, new PhysicsVelocity { Linear = new Vector3(0, 0, 100) });

        var test = Instantiate(Prefab, transform.position, transform.rotation);
        test.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
    }
}
