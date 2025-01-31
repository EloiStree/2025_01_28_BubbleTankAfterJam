using UnityEngine;

public class PlayerRadiusSizeFetchMono: MonoBehaviour{

    public Transform m_center;
    public Transform m_radius;

    public float m_currentSize;
    

    void Reset(){
        m_center = transform;
        m_radius = transform;
    }   

    public void Update(){
        m_currentSize = Vector3.Distance(m_center.position, m_radius.position);
    }

    public void GetRadiusSize(out float size){

        size = m_currentSize;

    }
}
