using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
public class MeshRenderer: ComponentSystem{

    protected override void OnUpdate(){
        Entities.ForEach((ref Translation transform) => {
            Manager manager = Manager.GetManager();

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