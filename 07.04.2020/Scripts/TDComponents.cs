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
        public int enemyDamage;
        public float enemySpeed;
        public float enemyMoveMinDistance;
        //public float deltaTime;
    }

    public struct PlayerData : IComponentData //define the component of the player
    {
        public int health;
        public int money;
    }

    public class MeshRenderer : ComponentSystem //define the system which handles the rendering
    { //drawmesh works only in main thread ???

        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation transform) => { //look for all entities which have the translate and show the cubes on their position
                EnemySpawnManager manager = EnemySpawnManager.GetManager();

                Graphics.DrawMesh(
                    manager.enemyMesh,
                    transform.Value,
                    Quaternion.identity,
                    manager.enemyMaterial,
                    0
                    );
            });
        }
    }

    #region EnemyMover
    public class EnemyMover : ComponentSystem
    { //getposition works only in main thread ???

        protected override void OnUpdate()
        { //look for all entities that have the enemymovement and translation so we know they are enemies
            Entities.ForEach((Entity e, ref Translation transform, ref EnemyMoveData moveData) =>
            {
                Entity entityToDestroy = e;
                int damage = moveData.enemyDamage;
                Vector3 targetPos = PathFollowManager.instance.followPoints[moveData.targetIndex].position;
                Vector3 currentPos = transform.Value; //move them accordingly through the followpoints
                Vector3 posDifference = (targetPos - currentPos);
                if (posDifference.magnitude < moveData.enemyMoveMinDistance) //did the enemy reach the point?
                {
                    moveData.targetIndex++;
                    moveData.targetIndex %= PathFollowManager.instance.followPoints.Count;
                    if (moveData.targetIndex == 0) //if the enemy wants to go back to the start, it reached the end
                    {
                        //deal dmg to Player, destory entity
                        Entities.ForEach((ref PlayerData player) =>
                        {
                            player.health -= damage;
                            //destroy entity
                            EnemySpawnManager.instance.spawnManager.DestroyEntity(entityToDestroy);
                            EnemySpawnManager.instance.UpdateHealthSlider(); //update player hp
                        });
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