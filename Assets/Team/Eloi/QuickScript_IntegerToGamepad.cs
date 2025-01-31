using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuickScript_IntegerToGamepad : MonoBehaviour
{
    public UnityEvent<int,Vector2> m_onRelayIndexIntegerJoystick;
    public UnityEvent<int,int> m_onRelayIndexInteger;
    public List<IntegerToArrayJoystick> m_playerInput = new List<IntegerToArrayJoystick>();

    [System.Serializable]
    public class IntegerToArrayJoystick
    {
        public int m_index;
        public Vector3 m_arrowDirection;
        public bool m_isAttaquing;

    }
//https://github.com/EloiStree/2024_08_29_ScratchToWarcraft/tree/main
    public int m_upPress=1038;
    public int m_downPress=1040;
    public int m_leftPress=1037;
    public int m_rightPress=1039;
    public int m_attackSpacePress=1032;


    public void PushInteger(int index, int value){

        for(int i =0 ; i < m_playerInput.Count; i++)
        {
            if (m_playerInput[i].m_index == index)
            {
                m_playerInput[i].m_arrowDirection = new Vector3();

                if (value == m_upPress)
                    m_playerInput[i].m_arrowDirection.y = 1;
                else if (value== m_downPress)
                    m_playerInput[i].m_arrowDirection.y = -1;
                else if (value == m_leftPress)
                    m_playerInput[i].m_arrowDirection.x = -1;
                else if (value == m_rightPress)
                    m_playerInput[i].m_arrowDirection.x = 1;
                else if (value == m_attackSpacePress)
                    m_playerInput[i].m_isAttaquing = true;

                else if (value == m_upPress+1000)
                    m_playerInput[i].m_arrowDirection.y = 0;
                else if (value== m_downPress+1000)
                    m_playerInput[i].m_arrowDirection.y = 0;
                else if (value == m_leftPress+1000)
                    m_playerInput[i].m_arrowDirection.x = 0;
                else if (value == m_rightPress+1000)
                    m_playerInput[i].m_arrowDirection.x = 0;
                else if (value == m_attackSpacePress+1000)
                    m_playerInput[i].m_isAttaquing = false;

                m_onRelayIndexIntegerJoystick.Invoke(index, m_playerInput[i].m_arrowDirection);
                m_onRelayIndexInteger.Invoke(index, value);
                return;
            }
        }

        IntegerToArrayJoystick newPlayer = new IntegerToArrayJoystick(){
                m_index = index,
        };
        m_playerInput.Add(newPlayer);

    }
}
