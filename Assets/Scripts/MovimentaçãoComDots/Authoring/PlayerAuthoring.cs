using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed = 5f;
}

public class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<PlayerTag>(entity);
        AddComponent(entity, new MoveSpeed { Value = authoring.moveSpeed });
        AddComponent<InputData>(entity);
    }
}