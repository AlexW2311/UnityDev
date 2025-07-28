using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public sealed class crosshairHud : MonoBehaviour
{
    VisualElement _cross;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _cross = root.Q("cross");

    }
    void Start()
    {
        Show(true);
        SetColor(Color.white);
    }
    public void Show(bool on) => _cross.style.display = on ? DisplayStyle.Flex : DisplayStyle.None;
    public void SetColor(Color c) => _cross.style.unityBackgroundImageTintColor = c;
}
