using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// Moves the entities by changing their position, can check bounds to make the positions stay within bounds
/// </summary>
public partial struct EntitiesBehaviorSystem : ISystem
{
    public void OnCreate(ref SystemState state) {
        // Start as disabled because it is enabled / disabled from UI
        state.Enabled = false;
    }

    // Cant burst compile this becuase of the use of "GetSingleton"
    // which is used to get the options selected by the user from the UI.
    // Although the jobified version is Burst Compiled
    public void OnUpdate(ref SystemState state) {

        var options = SystemAPI.ManagedAPI.GetSingleton<OptionsManaged>();
        float speed = 10f;
        if (options.EnableMovement.isOn) {
            float deltaTime = SystemAPI.Time.DeltaTime;

            if (options.UseParallelJob.isOn) {
                // Update using parallel job. Job is burst compiled and parallel
                new MoveJob {
                    speed = speed,
                    deltaTime = deltaTime,
                    checkBounds = options.EnableBoundsCheck.isOn
                }.ScheduleParallel();
            }
            else {
                // Update without job.  Not burst compiled and not parallel
                foreach (var (transform, entityData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<EntityData>>()) {
                    float2 direction = entityData.ValueRO.direction;

                    LocalTransform localTransform = transform.ValueRW;
                    localTransform.Position += new float3(direction.x, direction.y, 0) * speed * deltaTime;
                    transform.ValueRW = localTransform;

                    if (options.EnableBoundsCheck.isOn) {
                        var pos = transform.ValueRO.Position;
                        if (pos.x < 0) direction.x = math.abs(direction.x);
                        if (pos.x > 500) direction.x = -math.abs(direction.x);
                        if (pos.y < 0) direction.y = math.abs(direction.y);
                        if (pos.y > 500) direction.y = -math.abs(direction.y);
                        entityData.ValueRW.direction = direction;
                    }
                }
            }
        }
    }
}

[BurstCompile]
public partial struct MoveJob : IJobEntity
{
    [ReadOnly] public float speed;
    [ReadOnly] public float deltaTime;
    [ReadOnly] public bool checkBounds;

    public void Execute(ref LocalTransform transform, ref EntityData entityData) {
        float2 direction = entityData.direction;
        transform.Position += new float3(direction.x, direction.y, 0) * speed * deltaTime;


        if (checkBounds) {
            var pos = transform.Position;
            if (pos.x < 0) direction.x = math.abs(direction.x);
            if (pos.x > 500) direction.x = -math.abs(direction.x);
            if (pos.y < 0) direction.y = math.abs(direction.y);
            if (pos.y > 500) direction.y = -math.abs(direction.y);
            entityData.direction = direction;
        }
    }
}
