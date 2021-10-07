using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T m_Instance = null;
    public static T Instance {
        get {
            if (m_Instance == null) {
                 m_Instance = FindObjectOfType<T>();
                 if (m_Instance == null) {
                     m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
                 }
             }
             return m_Instance;
        }
    }
}
