using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class JobManager : MonoBehaviour
{
    [SerializeField] private bool useJobs;

    void Update()
    {
        float time = Time.realtimeSinceStartup;

        if(useJobs == true){
            NativeList<JobHandle> joblist = new NativeList<JobHandle>(Allocator.Temp);
            for(int j =0; j < 100; j++){
                JobHandle job = GetJob();
                joblist.Add(job);
            }
            JobHandle.CompleteAll(joblist);
            joblist.Dispose();
        }
        else{
            for(int j=0; j < 100;j ++){
                float x = 0.0f;
                for(int i = 0; i < 10000; i++){
                    x = math.exp10(math.sqrt(x));
                }
            }
        }

        Debug.Log((Time.realtimeSinceStartup - time)*1000f + " ms");
    }

    private JobHandle GetJob(){
        ToughJob job = new ToughJob();
        return job.Schedule();
    }
}

[BurstCompile]
public struct ToughJob:IJob{
    public void Execute(){
        float x = 0.0f;
        for(int i = 0; i < 10000; i++){
            x  = math.exp10(math.sqrt(x));
        }
    }
}
