using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BusTouchHud : MonoBehaviour
{
    public BusDrivingController controller;
    Font font;

    void Start()
    {
        font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        ButtonHold("LEFT", new Vector2(0.09f, 0.18f), new Vector2(150, 100), b => controller.SetSteering(b ? -1 : 0));
        ButtonHold("RIGHT", new Vector2(0.22f, 0.18f), new Vector2(150, 100), b => controller.SetSteering(b ? 1 : 0));
        ButtonHold("BRAKE", new Vector2(0.78f, 0.18f), new Vector2(150, 100), b => controller.SetBrake(b ? 1 : 0));
        ButtonHold("ACCEL", new Vector2(0.91f, 0.18f), new Vector2(150, 100), b => controller.SetThrottle(b ? 1 : 0));
        ButtonTap("REVERSE", new Vector2(0.78f, 0.34f), new Vector2(150, 70), () => controller.SetReverse(!controller.reverseSelected));
        ButtonTap("DOOR", new Vector2(0.91f, 0.34f), new Vector2(130, 70), controller.ToggleDoor);
        ButtonTap("LIGHTS", new Vector2(0.09f, 0.34f), new Vector2(130, 70), controller.ToggleLights);
        ButtonTap("LEFT SIGNAL", new Vector2(0.39f, 0.34f), new Vector2(150, 70), () => controller.SetIndicator(1));
        ButtonTap("RIGHT SIGNAL", new Vector2(0.51f, 0.34f), new Vector2(150, 70), () => controller.SetIndicator(2));
        ButtonTap("HAZARD", new Vector2(0.63f, 0.34f), new Vector2(130, 70), () => controller.SetIndicator(3));
        ButtonTap("CAMERA", new Vector2(0.22f, 0.34f), new Vector2(150, 70), () => FindObjectOfType<BusCameraRig>()?.SwitchCamera());
    }

    void ButtonHold(string label, Vector2 anchor, Vector2 size, System.Action<bool> callback)
    {
        var go = MakeButton(label, anchor, size);
        var trigger = go.AddComponent<EventTrigger>();
        var down = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        down.callback.AddListener(_ => callback(true));
        trigger.triggers.Add(down);
        var up = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        up.callback.AddListener(_ => callback(false));
        trigger.triggers.Add(up);
        var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener(_ => callback(false));
        trigger.triggers.Add(exit);
    }

    void ButtonTap(string label, Vector2 anchor, Vector2 size, UnityEngine.Events.UnityAction action)
    {
        MakeButton(label, anchor, size).GetComponent<Button>().onClick.AddListener(action);
    }

    GameObject MakeButton(string label, Vector2 anchor, Vector2 size)
    {
        var go = new GameObject(label);
        go.transform.SetParent(transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = anchor;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = size;
        var image = go.AddComponent<Image>();
        image.color = new Color(0.03f, 0.08f, 0.12f, 0.78f);
        var button = go.AddComponent<Button>();
        button.targetGraphic = image;
        var textGo = new GameObject("Label");
        textGo.transform.SetParent(go.transform, false);
        var txt = textGo.AddComponent<Text>();
        txt.font = font;
        txt.text = label;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.white;
        txt.fontSize = 25;
        txt.fontStyle = FontStyle.Bold;
        var tr = txt.rectTransform;
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = tr.offsetMax = Vector2.zero;
        return go;
    }
}
