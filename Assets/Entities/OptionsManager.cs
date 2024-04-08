using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages options exposed in the user interface
/// </summary>
public class OptionsManager : MonoBehaviour
{
    public InputField numItems;
    public Button renderButton;
    public Toggle enableBehaviorSystem;
    public Toggle enableMovement;
    public Toggle enableBoundsCheck;
    public Toggle useParallelJob;


    private void Start() {
        enableBehaviorSystem.interactable = true;
        enableMovement.interactable = false;
        enableBoundsCheck.interactable = false;
        useParallelJob.interactable = false;

        renderButton.onClick.AddListener(RenderButtonHandler);
        enableBehaviorSystem.onValueChanged.AddListener(EnableBehaviorHandler);
    }

    private void RenderButtonHandler() {
        EnableSystem<EntitiesEnableDisableSystem>(true);
    }

    private void EnableBehaviorHandler(bool isOn) {
        // these options should only be interactable when behavior system is enabled
        enableMovement.interactable = enableBehaviorSystem.isOn;
        enableBoundsCheck.interactable = enableBehaviorSystem.isOn;
        useParallelJob.interactable = enableBehaviorSystem.isOn;

        EnableSystem<EntitiesBehaviorSystem>(isOn);
    }

    private void EnableSystem<T>(bool enable) where T : ISystem {
        var world = World.DefaultGameObjectInjectionWorld;
        SystemHandle systemHandle = world.GetExistingSystem(typeof(T));
        ref SystemState state = ref world.Unmanaged.ResolveSystemStateRef(systemHandle);
        state.Enabled = enable;
    }
}
