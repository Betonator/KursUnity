using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
public class MeshRenderer: ComponentSystem{

    protected override void OnUpdate(){
        Entities.ForEach((ref Translation transform) => {
            ECSManager manager = ECSManager.GetECSManager();

            Graphics.DrawMesh(
                manager.mesh, 
                transform.Value, 
                Quaternion.identity, 
                manager.material,
                0
                );
        });
    }
}