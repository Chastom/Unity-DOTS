using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using UnityEngine;

public struct SpawnTime : IComponentData
{
    public float Value;
}

// This system updates all entities in the scene with both a RotationSpeed_SpawnAndRemove and Rotation component.
//^^^^^^ it has no components <additional ones> for now
public class SpawnTimeSystem : JobComponentSystem
{
    EntityCommandBufferSystem m_Barrier;

    protected override void OnCreate()
    {
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    // Use the [BurstCompile] attribute to compile a job with Burst.
    // You may see significant speed ups, so try it!
    [BurstCompile]
    struct SpawnTimeJob : IJobForEachWithEntity<SpawnTime, PhysicsGravityFactor>
    {
        public float DeltaTime;

        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;

        public void Execute(Entity entity, int jobIndex, ref SpawnTime lifeTime, ref PhysicsGravityFactor gravity)
        {
            lifeTime.Value -= DeltaTime;

            if (lifeTime.Value < 0.0f)
            {
                //Debug.Log(jobIndex + "  " + entity.Index);
                //CommandBuffer.DestroyEntity(jobIndex, entity);
                gravity.Value = 0.01f;
                //Debug.Log("will it stop...");
                ComponentType toRemove = lifeTime.GetType();
                CommandBuffer.RemoveComponent(jobIndex, entity, toRemove);
            }
        }
    }

    // OnUpdate runs on the main thread.
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();

        var job = new SpawnTimeJob
        {
            DeltaTime = Time.DeltaTime,
            CommandBuffer = commandBuffer,

        }.Schedule(this, inputDependencies);

        m_Barrier.AddJobHandleForProducer(job);

        return job;
    }
}
