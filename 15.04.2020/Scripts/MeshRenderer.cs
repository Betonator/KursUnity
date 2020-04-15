using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
 
  
public struct MeshRendererData : IComponentData{
    public int id;
}

public class MeshRendererSystem : ComponentSystem //define the system which handles the rendering
{ 
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation transform, ref MeshRendererData mesh) => { //look for all entities which have the translate and show the cubes on their position
            Manager manager = Manager.GetManager();

            Graphics.DrawMesh(
                manager.meshes[mesh.id],
                transform.Value,
                Quaternion.identity,
                manager.materials[mesh.id],
                0
                );
        });
    }
}