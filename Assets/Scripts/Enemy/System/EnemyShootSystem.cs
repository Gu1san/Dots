using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))] 
public partial struct EnemyShootSystem : ISystem{
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        Entity playerEntity;
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out playerEntity)) return;
        LocalToWorld playerTransform = SystemAPI.GetComponent<LocalToWorld>(playerEntity);
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (gun, gunTransform) in SystemAPI.Query<RefRW<EnemyGun>, RefRO<LocalToWorld>>().WithAll<EnemyTag>()){
            gun.ValueRW.shotCooldown -= deltaTime;
            if (gun.ValueRO.shotCooldown > 0) continue;
            gun.ValueRW.shotCooldown = gun.ValueRO.fireRate;
            float3 fireDirection = playerTransform.Position - gunTransform.ValueRO.Position;
            fireDirection.y = 0; 
            fireDirection = math.normalize(fireDirection); 
            Entity newBullet = ecb.Instantiate(gun.ValueRO.bulletPrefab);
            var bulletTransform = LocalTransform.FromPositionRotationScale(
                gunTransform.ValueRO.Position,
                quaternion.LookRotationSafe(fireDirection, math.up()),
                gun.ValueRO.bulletScale);
            ecb.SetComponent(newBullet, bulletTransform);
            ecb.AddComponent(newBullet, new ProjectileMoveData{
                Direction = fireDirection,
                Speed = gun.ValueRO.shotStrength
            });
        }
    }
}