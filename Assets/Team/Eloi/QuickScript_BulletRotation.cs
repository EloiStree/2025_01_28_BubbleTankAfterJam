using UnityEngine;

public class QuickScript_BulletRotation : MonoBehaviour
{
    void Reset()
    {
        m_whatToRotate = transform;
    }
    public Transform m_whatToRotate;
    public Vector3 m_rotationSpeedMin = new Vector3(180, 0, 0);
    public Vector3 m_rotationSpeedMax = new Vector3(180, 0, 0);
    public Vector3 m_rotationSpeed = new Vector3(180, 0, 0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (m_whatToRotate == null)
        {
            m_whatToRotate = transform;
        }
        m_rotationSpeed = new Vector3(Random.Range(m_rotationSpeedMin.x, m_rotationSpeedMax.x), Random.Range(m_rotationSpeedMin.y, m_rotationSpeedMax.y), Random.Range(m_rotationSpeedMin.z, m_rotationSpeedMax.z));
        m_whatToRotate.transform.Rotate(UnityEngine.Random.value*360, UnityEngine.Random.value*360, UnityEngine.Random.value*360);

    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        m_whatToRotate.Rotate(m_rotationSpeed, deltaTime);
    }
}
