using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// This system enables / disables spawned entities.
/// <see cref="EntitiesSpawnerSystem"/> creates max number of renderable entities when the application starts.
/// All of these entities are initially created as disabled (this is done by adding the built in Disabled tag)
/// Then this sytem removes or adds the Disabled tag depending on how many entities the user wants to render.
/// </summary>
public partial struct EntitiesEnableDisableSystem : ISystem
{
    public void OnCreate(ref SystemState state) {
        // Disable on creation so that update does not run
        state.Enabled = false;
    }

    public void OnUpdate(ref SystemState state) {
        // Disable on first update. Essentially achieving a single time use system 
        state.Enabled = false;

        // Get the options (number of entities to render) the user has selected
        var options = SystemAPI.ManagedAPI.GetSingleton<OptionsManaged>();
        int.TryParse(options.NumItems.text, out var numItems);

        // Loop through entities and enable only required number (numItems) that the user entered
        var commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        int counter = 0;
        
        foreach (var (entityData, entity) in 
            SystemAPI.Query<EntityData>()
                .WithOptions(EntityQueryOptions.IncludeDisabledEntities)
                .WithEntityAccess()) {

            if (counter < numItems) {
                commandBuffer.RemoveComponent<Disabled>(entity);
                
                // HACK: for some reason sometimes the built-in system (LocalToWorldSystem)
                // that calculates world coordinates does not seem to run or runs in incorrect order???
                // as a result the position of the entity gets set to (0,0,0)
                // this hack basically recalculates the world position of the entity from its local transform
                var lt = SystemAPI.GetComponentLookup<LocalTransform>();
                var p = SystemAPI.GetComponentLookup<Parent>();
                var ptm = SystemAPI.GetComponentLookup<PostTransformMatrix>();
                TransformHelpers.ComputeWorldTransformMatrix(entity, out var outputMatrix, ref lt, ref p, ref ptm);
                commandBuffer.SetComponent<LocalToWorld>(entity, new LocalToWorld { Value = outputMatrix });
                // END HACK
            }
            else {
                commandBuffer.AddComponent<Disabled>(entity);
            }
            counter++;
        }
        commandBuffer.Playback(World.DefaultGameObjectInjectionWorld.EntityManager);
    }
}
