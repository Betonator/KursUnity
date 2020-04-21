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
public class TurretBuilder : MonoBehaviour {
    public Camera camera;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)){
                if(hit.transform.gameObject.CompareTag("Ground")){
                    Vector3 position = new Vector3(
                                                    Mathf.Round(hit.point.x),
                                                    Mathf.Round(hit.point.y),
                                                    Mathf.Round(hit.point.z));
                    TurretComponents.GetTurret().createTurret(position);
                }
            }
        }
    }
}