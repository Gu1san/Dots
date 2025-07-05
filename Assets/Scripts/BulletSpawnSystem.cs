using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine; // Para Input e Camera
using Unity.Collections; // Para EntityCommandBuffer

// Define the PlayerTag component as a tag component
public struct PlayerTag : IComponentData { }

// Define the BulletData component
public struct BulletData : IComponentData
{
    public float Speed;
    public float Lifetime;
    public float TimeAlive;
}

// Define the InputData component
public struct InputData : IComponentData
{
    public float2 AimDirection;
    public float2 MoveDirection;
}

public partial struct BulletSpawnSystem : ISystem
{
    private Entity bulletPrefab;
    private float shootCooldown;
    private float timeSinceLastShot;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
        // Pegue o prefab de algum singleton ou authoring component
        //bulletPrefab = ...;
        shootCooldown = 0.1f;
        timeSinceLastShot = 0;
    }

    public void OnUpdate(ref SystemState state)
    {
        timeSinceLastShot += SystemAPI.Time.DeltaTime;
        if (!Input.GetMouseButton(0) || timeSinceLastShot < shootCooldown) return;

        timeSinceLastShot = 0;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, input) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<InputData>>().WithAll<PlayerTag>())
        {
            Entity bullet = ecb.Instantiate(bulletPrefab);
            var pos = transform.ValueRO.Position;
            var dir = input.ValueRO.AimDirection;

            ecb.SetComponent(bullet, new LocalTransform
            {
                Position = pos + new float3(dir.x, dir.y, 0),
                Rotation = quaternion.identity,
                Scale = 1
            });

            ecb.SetComponent(bullet, new BulletData
            {
                Speed = 10f,
                Lifetime = 3f,
                TimeAlive = 0f
            });
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
