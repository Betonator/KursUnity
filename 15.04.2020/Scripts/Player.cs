using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine.UI;

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


        healthImage.fillAmount = 1f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthImage.fillAmount = health/10f;
    }
}

