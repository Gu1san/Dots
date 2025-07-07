// using Unity.Entities;

// public partial struct EnemyDeathSystem : ISystem
// {
//     public void OnCreate(ref SystemState state) {
//         state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
//     }

//     public void OnUpdate(ref SystemState state){
//         var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
//         foreach (var (life, entity) in SystemAPI.Query<RefRO<EnemyLife>>().WithEntityAccess()){
//             if (life.ValueRO.lifeValue <= 0){
//                 ecb.DestroyEntity(entity);
//             }
//         }
//     }
// }