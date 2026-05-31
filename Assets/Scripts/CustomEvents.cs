using UnityEngine;

public class CustomEvents : MonoBehaviour
{
    private static CustomEvents _instance;
    private void Start()
    {
        if (_instance != null) _instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        return;
    }
}
