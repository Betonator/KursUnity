using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine.UI;
    
public struct PlayerData : IComponentData //define the component of the player
{
    public int health;
    public int money;
}


public class Player : MonoBehaviour {

    [SerializeField]
    private Image healthImage;

    private int health = 10;

    private static Player instance;
    public static Player GetPlayer(){
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype playerType = entityManager.CreateArchetype(
            typeof(PlayerData)
            ); //define enemy type


        healthImage.fillAmount = 1f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthImage.fillAmount = health/10f;
    }
}

