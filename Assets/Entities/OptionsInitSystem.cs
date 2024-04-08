using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public partial struct OptionsInitSystem : ISystem
{
    public void OnCreate(ref SystemState systemState) {
        systemState.RequireForUpdate<EntitiesSpawner>();
    }

    public void OnUpdate(ref SystemState state) {
        state.Enabled = false;

        var optionsGO = GameObject.FindObjectOfType<OptionsManager>();
        var options = optionsGO.GetComponent<OptionsManager>();

        var optionsManaged = new OptionsManaged();
        optionsManaged.EnableBehaviorSystem = options.enableBehaviorSystem;
        optionsManaged.EnableMovement = options.enableMovement;
        optionsManaged.EnableBoundsCheck = options.enableBoundsCheck;
        optionsManaged.UseParallelJob = options.useParallelJob;
        optionsManaged.NumItems = options.numItems;
        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(entity, optionsManaged);
    }
}

/// <summary>
/// Managed component is needed so that we can read values of UI elements in systems
/// An alternative approach could be to create an unmanaged component, and update it each time UI elements change
/// </summary>
public class OptionsManaged : IComponentData
{
    public InputField NumItems;
    public Toggle EnableBehaviorSystem;
    public Toggle EnableMovement;
    public Toggle EnableBoundsCheck;
    public Toggle UseParallelJob;

    // Every IComponentData class must have a no-arg constructor.
    public OptionsManaged() {
    }
}
