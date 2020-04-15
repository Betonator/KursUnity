using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
public class Manager : MonoBehaviour
{
    public Mesh[] meshes;
    public Material[] materials;

    public Entity player;

    public static Manager instance;
    public static Manager GetManager(){
        return instance;
    }

    private void Awake() {
        instance = this;
    }
}