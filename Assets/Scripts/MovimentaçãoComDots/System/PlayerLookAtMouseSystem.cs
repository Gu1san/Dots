using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Game.Components;

namespace Game.Systems
{
    public partial class PlayerLookAtMouseSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var camera = Camera.main;
            if (camera == null) return;

            Vector3 mousePosition = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePosition);

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out float distance)) return;

            Vector3 worldMousePosition = ray.GetPoint(distance);
            float3 targetPos = (float3)worldMousePosition;

            Entities
                .WithAll<PlayerTag>() 
                .ForEach((ref LocalTransform transform) =>
                {
                    float3 direction = targetPos - transform.Position;
                    direction.y = 0;

                    if (math.lengthsq(direction) > 0.0001f)
                    {
                        quaternion targetRotation = quaternion.LookRotationSafe(direction, math.up());
                        transform.Rotation = math.slerp(transform.Rotation, targetRotation, SystemAPI.Time.DeltaTime * 15f);
                    }
                }).Schedule();
        }
    }
}