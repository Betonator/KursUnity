using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using UnityEngine.Jobs;

public class JobManager : MonoBehaviour
{
    [SerializeField] private Transform[] enemy;

    void Update()
    {
        float time = Time.realtimeSinceStartup;

#region IJobParallelFor
        NativeArray<float> posYarray = new NativeArray<float>(enemy.Length, ALlocator.TempJob);

        for(int i =0; i < enemy.Length; i++){
            posYarray[i] = enemy[i].position.y;
        }

        ArrayJob arrayJob = new ArrayJob{
            deltaTime = Time.deltaTime,
            posY = posYarray
        };

        JobHandle arrayJobHandle = arrayJob.Schedule(enemy.Length, 10);
        arrayJobHandle.Complete();

        for(int i =0; i < enemy.Length; i++){
            enemy[i].position += new Vector3(0, posYarray[i], 0);
        }

        posYarray.Dispose();
#endregion

#region IJobParallelForTransform
        TransformAccessArray transformAccess = new TransformAccessArray(enemy.Length);

        for(int i = 0; i < enemy.Length; i++){
            transformAccess.Add(enemy[i]);
        }

        transformJob tJob = new transformJob{
            deltaTime = Time.deltaTime,
        };

        JobHandle jobHandle = tJob.Schedule(transformAccess);
        jobHandle.Complete();

        transformAccess.Dispose();

#endregion
        Debug.Log((Time.realtimeSinceStartup - time)*1000f + " ms");
    }

    private JobHandle GetJob(){
        ToughJob job = new ToughJob();
        return job.Schedule();
    }
}

//[BurstCompile]
public struct ToughJob:IJob{
    public void Execute(){
        float x = 0.0f;
        for(int i = 0; i < 10000; i++){
            x  = math.exp10(math.sqrt(x));
        }
    }
}

public struct ArrayJob:IJobParallelFor{

    public NativeArray<float> posY;
    [ReadOnly] public float deltaTime;

    public void Execute(int index){
        posY[index] += 2.0f * deltaTime;
        if(posY[index] > 5.0f)
            posY[index] = -5.0f;
    }
}

public struct transformJob:IJobParallelForTransform{
    [ReadOnly] public float deltaTime;
    public void Execute(int index, TransformAccess transform){
        float deltaPos = 2.0f * deltaTime;
        float y = transform.position.y + deltaPos;
        if(y > 5.0f)
        {
            y = -5.0f;
        }
        transform.position = new float3(transform.position.x,y,transform.position.z);
    }
}