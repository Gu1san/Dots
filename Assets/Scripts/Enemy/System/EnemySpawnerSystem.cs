
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public partial struct EnemySpawnerSystem : ISystem{
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state){
        Entity playerEntity;
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out playerEntity)) return;
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);

        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (spawner, spawnerTransform) in SystemAPI.Query<RefRW<ProximitySpawner>, RefRO<LocalTransform>>()){
            float distance = math.distance(spawnerTransform.ValueRO.Position, playerTransform.Position);
            if (distance < spawner.ValueRO.ActivationDistance){
                spawner.ValueRW.SpawnTimer -= deltaTime;
                if (spawner.ValueRO.SpawnTimer <= 0){
                    spawner.ValueRW.SpawnTimer = spawner.ValueRO.SpawnRate;
                    Entity newEnemy = ecb.Instantiate(spawner.ValueRO.EnemyPrefab);
                    ecb.SetComponent(newEnemy, LocalTransform.FromPosition(spawnerTransform.ValueRO.Position));
                }
            }
        }
    }
}
