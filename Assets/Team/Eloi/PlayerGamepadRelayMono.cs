using UnityEngine;
using UnityEngine.Events;
public  class PlayerGamepadRelayMono : MonoBehaviour {

    public int m_userIntegerId;

    [Header("Gamepad")]
    public Vector3 m_joystickLeftPercent;
    public Vector3 m_joystickRightPercent;
    public UnityEvent< Vector2, Vector2> m_onGamepadReceived;
    public UnityEvent<int, Vector2, Vector2> m_onGamepadWithIdReceived;

    [Header("Action")]
    public int m_lastActionReceived;
    public UnityEvent<int> m_onActionEventReceived;
    public UnityEvent<int, int> m_onActionEventWithIdReceived;

    public void SetUserIntegerId(int userIntegerId) {
        m_userIntegerId = userIntegerId;
    }

    public void PushInGamepadValue( Vector2 joystickLeftPercent, Vector2 joystickRightPercent)
    {
        PushInGamepadValue(m_userIntegerId, joystickLeftPercent, joystickRightPercent);
    }

    public void PushInGamepadValue(int userIntegerId, Vector2 joystickLeftPercent, Vector2 joystickRightPercent)
    {
        m_userIntegerId = userIntegerId;
        m_joystickLeftPercent = joystickLeftPercent;
        m_joystickRightPercent = joystickRightPercent;
        m_onGamepadReceived.Invoke( joystickLeftPercent, joystickRightPercent);
        m_onGamepadWithIdReceived.Invoke(userIntegerId, joystickLeftPercent, joystickRightPercent);
    }

    public void PushInIntegerAction(int userIntegerId, int integerEvent) {
        m_userIntegerId = userIntegerId;
        m_lastActionReceived= integerEvent;
        m_onActionEventReceived.Invoke(integerEvent);
        m_onActionEventWithIdReceived.Invoke(userIntegerId, integerEvent);
    }
}