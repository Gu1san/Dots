using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct EnemyProjectileSystem : ISystem{
    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        float deltatime = SystemAPI.Time.DeltaTime;
        foreach (var (Transform, moveData, lifeTime, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<ProjectileMoveData>, RefRW<ProjectileLifetime>>().WithAll<ProjectileTag>().WithEntityAccess()){
            lifeTime.ValueRW.timeRemaining -= deltatime;
            if (lifeTime.ValueRO.timeRemaining <= 0f){
                ecb.DestroyEntity(entity);
                continue;
            }
            Transform.ValueRW.Position += moveData.ValueRO.Direction * moveData.ValueRO.Speed * deltatime;
        }
    }
}
