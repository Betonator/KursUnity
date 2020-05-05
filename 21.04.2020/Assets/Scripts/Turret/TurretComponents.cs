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

public struct Angle : IComponentData{
    public float angle;
}

public class TurretFindTarget : ComponentSystem
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

            if(newTarget != Entity.Null){
                PostUpdateCommands.AddComponent(turretEntity, new HasTarget{ target = newTarget });
            }
        });
    }
}



public class TurretAim : ComponentSystem
{
    protected override void OnUpdate()
    { 
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entities.WithAll<HasTarget>().ForEach((Entity entity, ref Translation turretPos, ref Rotation turretRot, ref Angle angle, ref HasTarget target)=>{
            if(target.target != Entity.Null){
                
                Translation enemyPos = entityManager.GetComponentData<Translation>(target.target);

                float3 targetVec = enemyPos.Value - turretPos.Value;

                float len = targetVec.x*targetVec.x + targetVec.y*targetVec.y + targetVec.z*targetVec.z;
                len = Mathf.Sqrt(len);

                float a = targetVec.z / len;

                a = Mathf.Acos(a);

                float rad = angle.angle * Mathf.Deg2Rad;

                if(targetVec.x <  0) a = -a;

                float speedInDeg = 90.0f;
                float maxDelta = Mathf.Deg2Rad * speedInDeg * Time.DeltaTime;

                float newRot = Mathf.MoveTowards(rad, a, maxDelta);
                angle.angle = newRot * Mathf.Rad2Deg;
                turretRot.Value = quaternion.EulerXYZ(0, newRot, 0);
            }
            else{
                PostUpdateCommands.RemoveComponent(entity, typeof(HasTarget));
            }
        });
    }
}


public class TurretComponents : MonoBehaviour {
    
    [SerializeField]
    private float3[] position;

    private EntityManager entityManager;
    private EntityArchetype archetype;
    private static TurretComponents instance;
    public static TurretComponents GetTurret(){
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Manager manager = Manager.GetManager();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        archetype = entityManager.CreateArchetype(
            typeof(Turret),
            typeof(TurretAttack),
            typeof(Translation),
            typeof(Rotation),
            typeof(Angle),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> turrets = new NativeArray<Entity>(position.Length, Allocator.Temp);

        entityManager.CreateEntity(archetype, turrets);

        for(int i = 0; i < position.Length; i++){
            Entity entity = turrets[i];
            entityManager.SetComponentData(entity, new TurretAttack{ 
                range = 10,
                damage = 10
                });
            entityManager.SetComponentData(entity, new Translation{ 
                Value = position[i]
                });
            entityManager.SetComponentData(entity, new Rotation{
                Value = quaternion.Euler(0,0,0)
            });
            entityManager.SetComponentData(entity, new Angle{
                angle = 0.0f
            });
            entityManager.SetSharedComponentData(entity, new RenderMesh{
                mesh = manager.meshes[1],
                material = manager.materials[1]
                });
        }

        turrets.Dispose();
    }

    public void createTurret(float3 position){
        Manager manager = Manager.GetManager();
        Entity entity = entityManager.CreateEntity(archetype);
        entityManager.SetComponentData(entity, new TurretAttack{ 
            range = 10,
            damage = 10
            });
        entityManager.SetComponentData(entity, new Translation{ 
            Value = position
            });
        entityManager.SetComponentData(entity, new Rotation{
            Value = quaternion.Euler(0,0,0)
        });
        entityManager.SetComponentData(entity, new Angle{
            angle = 0.0f
        });
        entityManager.SetSharedComponentData(entity, new RenderMesh{
            mesh = manager.meshes[1],
            material = manager.materials[1]
            });
    }
}