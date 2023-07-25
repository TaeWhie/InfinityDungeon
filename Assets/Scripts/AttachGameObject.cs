using UnityEngine;

public class AttachGameObject : MonoBehaviour
{
   public Transform target;
   private void FixedUpdate()
   {
      transform.position = target.position;
   }
}
