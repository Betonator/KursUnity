using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    public Mesh mesh;
    public Material material;

    public static ECSManager instance;
    public static ECSManager GetECSManager(){
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(LevelComponent),
            typeof(Translation)
        );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(1, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);

        for(int i = 0; i < entityArray.Length; i++){
            Entity entity = entityArray[i];
            entityManager.SetComponentData(entity, new LevelComponent{level = UnityEngine.Random.Range(0f,50f)});
            entityManager.SetComponentData(entity, new Translation{
                Value = new float3(UnityEngine.Random.Range(-5,5),
                UnityEngine.Random.Range(-5,5),
                UnityEngine.Random.Range(-5,5))
                });
        }

        entityArray.Dispose();
    }

    void Update()
    {
        
    }
}
