using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

namespace TD
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private int startingEnemyAmount = 0; //for spawning enemies correctly
        public int enemyAmount; //desired enemy amount
        public float waitBetweenSpawnTime; //how long to wait between spawns
        public EntityManager spawnManager; //the thing handling all entities
        private EntityArchetype enemyType; //two archetypes

        public static EnemySpawnManager instance;
        public static EnemySpawnManager GetManager()
        {
            return instance;
        }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            spawnManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            enemyType = spawnManager.CreateArchetype(
                typeof(Translation),
                typeof(EnemyMoveData),
                typeof(EnemyAttack),
                typeof(EnemyHealth),
                typeof(MeshRendererData)
                ); //define enemy type


        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SpawnCoroutine()); //start the spawn coroutine, runs on separate thread
            }
        }

        IEnumerator SpawnCoroutine()
        {
            while (startingEnemyAmount < enemyAmount) //until we reach desired amount, spawn enemies
            {
                SpawnEnemy();
                yield return new WaitForSeconds(waitBetweenSpawnTime); //wait for the time
                startingEnemyAmount++;
            }
            yield return 0;
        }

        public void SpawnEnemy()
        {
            Entity spawnedEnemy = spawnManager.CreateEntity(enemyType); //spawn enemy from enemytype and set desired values
            spawnManager.SetComponentData(spawnedEnemy, new Translation
            {
                Value = new float3(
                    PathFollowManager.instance.followPoints[0].position.x,
                    PathFollowManager.instance.followPoints[0].position.y,
                    PathFollowManager.instance.followPoints[0].position.z)
            });
            spawnManager.SetComponentData(spawnedEnemy, new EnemyMoveData
            {
                targetIndex = 1,
                enemySpeed = 0.05f,
            });
            spawnManager.SetComponentData(spawnedEnemy, new EnemyAttack{
                damage = 1
            });
            spawnManager.SetComponentData(spawnedEnemy, new EnemyHealth{
                health = 10
            });
            spawnManager.SetComponentData(spawnedEnemy, new MeshRendererData{
                id = 0
            });
        }
    }
}
