using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;

namespace TD
{
    public class PathFollowManager : MonoBehaviour
    {
        public List<Transform> followPoints = new List<Transform>(); //just for containing the points

        public static PathFollowManager instance;
        public static PathFollowManager GetManager()
        {
            return instance;
        }

        private void Awake()
        {
            instance = this;
        }
    }
}
