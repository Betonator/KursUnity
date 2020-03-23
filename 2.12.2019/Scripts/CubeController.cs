using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Animator))]  podczas dodania skryptu do obiektu automatycznie zostanie dodany komponent o konkretnym typie
public class CubeController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetInteger("State",0); //usawianie parametru w animatorze (nazwa, wartosc)
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            animator.SetInteger("State", 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetInteger("State", 2);
        }
    }
}
