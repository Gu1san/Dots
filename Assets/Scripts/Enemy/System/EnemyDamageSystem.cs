// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Jobs;
// using Unity.Physics;
// using Unity.Physics.Systems;

// [UpdateInGroup(typeof(PhysicsSystemGroup))]
// [UpdateAfter(typeof(PhysicsSimulationGroup))]
// public partial struct EnemyDamageSystem : ISystem
// {
//     public void OnCreate(ref SystemState state) {
//         state.RequireForUpdate<SimulationSingleton>();
//     }

//     [BurstCompile]
//     public void OnUpdate(ref SystemState state){
//         var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
//         var ecb = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

//         state.Dependency = new DamageJob{
//             EnemyLifeLookup = SystemAPI.GetComponentLookup<EnemyLife>(false),
//             ProjectileDataLookup = SystemAPI.GetComponentLookup<ProjectileMoveData>(true),
//             PlayerProjectileLookup = SystemAPI.GetComponentLookup<PlayerProjectileTag>(true),
//             EnemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true),
//             Ecb = ecb
//         }.Schedule(simulationSingleton, state.Dependency);
//     }
// }

// [BurstCompile]
// public struct DamageJob : ITriggerEventsJob{
//     public ComponentLookup<EnemyLife> EnemyLifeLookup;
//     [ReadOnly] public ComponentLookup<ProjectileMoveData> ProjectileDataLookup;
//     [ReadOnly] public ComponentLookup<PlayerProjectileTag> PlayerProjectileLookup;
//     [ReadOnly] public ComponentLookup<EnemyTag> EnemyLookup;
//     public EntityCommandBuffer Ecb;

//     public void Execute(TriggerEvent triggerEvent){
//         Entity entityA = triggerEvent.EntityA;
//         Entity entityB = triggerEvent.EntityB;

//         bool isBodyAProjectile = PlayerProjectileLookup.HasComponent(entityA);
//         bool isBodyBProjectile = PlayerProjectileLookup.HasComponent(entityB);
//         bool isBodyAEnemy = EnemyLookup.HasComponent(entityA);
//         bool isBodyBEnemy = EnemyLookup.HasComponent(entityB);

//         if (isBodyAProjectile && isBodyBEnemy){
//             ApplyDamage(entityB, entityA);
//         }
//         else if (isBodyBProjectile && isBodyAEnemy){
//             ApplyDamage(entityA, entityB);
//         }
//     }

//     private void ApplyDamage(Entity enemyEntity, Entity projectileEntity){
//         Ecb.DestroyEntity(projectileEntity);
//         EnemyLife health = EnemyLifeLookup[enemyEntity];
//         health.lifeValue -= ProjectileDataLookup[projectileEntity].damage;
//         EnemyLifeLookup[enemyEntity] = health;
//     }
// }