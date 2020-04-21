using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Rendering;

public struct Turret : IComponentData{}

public struct TurretAttack : IComponentData{
    public float range;
    public int damage;
}

public struct HasTarget : IComponentData{
    public Entity target;
}

public struct TurretRotation: IComponentData{
    public float angle;
}

public class TurretAim : ComponentSystem
{ 
    protected override void OnUpdate()
    { 
        Entities.WithNone<HasTarget>().ForEach((Entity turretEntity, ref Translation translation, ref TurretAttack turretAttack) => {
            float3 turretPosition = translation.Value;
            float range = turretAttack.range;
            Entity newTarget = Entity.Null;
            float minDistance = 0;

            Entities.WithAll<TD.Enemy>().ForEach((Entity enemyEntity, ref Translation enemyTranslation) => {
                if(newTarget == Entity.Null){
                    minDistance = math.distance(turretPosition, enemyTranslation.Value);
                    if(minDistance < range)
                    newTarget = enemyEntity;
                }
                else {
                    float distance = math.distance(turretPosition, enemyTranslation.Value);
                    if(distance < minDistance && distance < range){
                        minDistance = distance;
                        newTarget  = enemyEntity;
                    }
                }
            });

            if(newTarget == Entity.Null){
                PostUpdateCommands.AddComponent(turretEntity, new HasTarget{ target = newTarget });
            }
        });
    }
}


public class TurretComponents : MonoBehaviour {
    
    [SerializeField]
    private float3[] position;

    void Start()
    {
        Manager manager = Manager.GetManager();
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Turret),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> turrets = new NativeArray<Entity>(position.Length, Allocator.Temp);

        entityManager.CreateEntity(archetype, turrets);

        for(int i = 0; i < position.Length; i++){
            Entity entity = turrets[i];

            entityManager.SetComponentData(entity, new Translation{ 
                Value = position[i]
                });
            entityManager.SetSharedComponentData(entity, new RenderMesh{
                mesh = manager.meshes[1],
                material = manager.materials[1]
                });
        }

        turrets.Dispose();
    }
}