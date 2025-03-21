using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class EntityCameraTargetSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var targets = Object.FindObjectsOfType<EntityCameraTarget>();
        
        foreach (var target in targets)
        {
            if (target.TargetEntity == Entity.Null || target.TargetWorld == null)
                continue;

            // Get the entity's position
            if (target.TargetWorld.EntityManager.HasComponent<LocalTransform>(target.TargetEntity))
            {
                var transform = target.TargetWorld.EntityManager.GetComponentData<LocalTransform>(target.TargetEntity);
                target.transform.position = transform.Position;
                target.transform.rotation = transform.Rotation;
            }
        }
    }
} 