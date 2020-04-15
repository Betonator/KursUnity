using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;


public struct Turret : IComponentData{}

public struct TurretAttack : IComponentData{
    public float range;
    public int damage;
}

public struct HasTarget : IComponentData{
    public Entity target;
}