using UnityEngine;

public class Rotate : MonoBehaviour
{
    private void FixedUpdate()
    {
        gameObject.GetComponent<RectTransform>().Rotate(-Vector3.forward * 5);
    }
}

