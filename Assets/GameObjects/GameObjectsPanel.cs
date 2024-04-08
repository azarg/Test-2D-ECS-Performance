using UnityEngine;
using UnityEngine.UI;

public class GameObjectsPanel : MonoBehaviour
{
    public InputField numberOfItems;
    public Button renderButton;
    public Toggle enableEmptyBehavior;
    public Toggle enableComplexBehavior;
    public Toggle addMovement;
    public Toggle addBoundsCheck;

    GameObjectsSpawner spawner;

    private void Start() {
        spawner = FindObjectOfType<GameObjectsSpawner>();
        renderButton.onClick.AddListener(RenderButtonHandler);
        enableEmptyBehavior.onValueChanged.AddListener(EnableEmptyBehaviorHandler);
        enableComplexBehavior.onValueChanged.AddListener(EnableComplexBehaviorHandler);
        addMovement.onValueChanged.AddListener((bool e) => spawner.EnableMovement(e));
        addBoundsCheck.onValueChanged.AddListener((bool e) => spawner.EnableBoundsCheck(e));
        addMovement.interactable = enableComplexBehavior.isOn;
        addBoundsCheck.interactable = enableComplexBehavior.isOn;
    }

    private void EnableEmptyBehaviorHandler(bool isOn) {
        spawner.SetBehaviorActive<GameObjectEmptyBehavior>(isOn);
    }

    private void EnableComplexBehaviorHandler(bool isOn) {
        spawner.SetBehaviorActive<GameObjectComplexBehavior>(isOn);
        addMovement.interactable = isOn;
        addBoundsCheck.interactable = isOn;
    }

    private void RenderButtonHandler() {
        int.TryParse(numberOfItems.text, out int count);
        spawner.RenderItems(count);
    }
}
