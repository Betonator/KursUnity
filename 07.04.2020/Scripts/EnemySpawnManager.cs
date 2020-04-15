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
        public Mesh enemyMesh; //what enemy would u want
        public Material enemyMaterial; //in what color
        public EntityManager spawnManager; //the thing handling all entities
        public Entity player; //we need a reference to the player at all times
        EntityArchetype enemyType = new EntityArchetype(); //two archetypes
        EntityArchetype playerType = new EntityArchetype();

        [SerializeField]
        private Slider healthSlider; //for handling the slider

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
                typeof(EnemyMoveData)
                ); //define enemy type

            playerType = spawnManager.CreateArchetype(
                typeof(PlayerData)
                ); //define enemy type

            player = spawnManager.CreateEntity(playerType); //create player from player type
            spawnManager.SetComponentData(player, new PlayerData
            {
                health = 100,
                money = 0
            }); //set player values and slider values
            healthSlider.maxValue = 100;
            healthSlider.value = 100;
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
                enemyMoveMinDistance = 0.3f,
                enemyDamage = 5
                //deltaTime = Time.deltaTime
            });
        }

        public void UpdateHealthSlider()
        {
            healthSlider.value = spawnManager.GetComponentData<PlayerData>(player).health;
            //check if dead
        }
    }
}
