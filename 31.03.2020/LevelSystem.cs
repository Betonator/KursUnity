using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;

// public class LevelSystem : ComponentSystem
// {
//     protected override void OnUpdate(){
//         Entities.ForEach((ref LevelComponent levelComponent) => {
//             levelComponent.level += Time.DeltaTime;
//         });
//     }
// }

public class LevelSystem : JobComponentSystem{
    protected override JobHandle OnUpdate(JobHandle inputDeps){
        float deltaTime = Time.DeltaTime;

        JobHandle job = Entities.ForEach((ref LevelComponent levelComponent) => {
            levelComponent.level += deltaTime;
        }).Schedule(inputDeps);

        return job;
    }
}

public struct Enemy:IComponentData{}
public struct Turret:IComponentData{}
