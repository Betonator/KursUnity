using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;

namespace TD
{
    public struct EnemyMoveData : IComponentData //define the component that our enemies will have
    { 
        public int targetIndex;
        public float enemySpeed;
    }

    public struct EnemyHealth : IComponentData
    {
        public int health;
    }

    public struct EnemyAttack : IComponentData
    {
        public int damage;
    }

    #region EnemyMover
    public class EnemyMover : ComponentSystem
    { //getposition works only in main thread ???

        protected override void OnUpdate()
        { //look for all entities that have the enemymovement and translation so we know they are enemies
            EnemySpawnManager manager = EnemySpawnManager.GetManager();

            Entities.ForEach((Entity e, ref Translation transform, ref EnemyMoveData moveData, ref EnemyAttack attack) =>
            {
                Entity entityToDestroy = e;
                int damage = attack.damage;
                Vector3 targetPos = PathFollowManager.instance.followPoints[moveData.targetIndex].position;
                Vector3 currentPos = transform.Value; //move them accordingly through the followpoints
                Vector3 posDifference = (targetPos - currentPos);
                if (posDifference.magnitude < 0.3f) //did the enemy reach the point?
                {
                    moveData.targetIndex++;
                    moveData.targetIndex %= PathFollowManager.instance.followPoints.Count;
                    if (moveData.targetIndex == 0) //if the enemy wants to go back to the start, it reached the end
                    {

                        EnemySpawnManager.instance.spawnManager.DestroyEntity(entityToDestroy);
                        Player.GetPlayer().TakeDamage(damage); //update player hp
                    }
                }
                posDifference = posDifference.normalized * moveData.enemySpeed; //move enemy accordingly
                transform.Value.x += posDifference.x;
                transform.Value.y += posDifference.y;
                transform.Value.z += posDifference.z;
            });
        }
    }
    #endregion
}