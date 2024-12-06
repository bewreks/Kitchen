using UnityEngine;

namespace Kitchen.Scripts.Player
{
    public class PlayerStabMovement : MonoBehaviour
    {
        public float speed = 1f;
        public Vector2 wrap = new Vector2(-19, 19);
        
        private Vector3 direction = Vector3.right;
        
        void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);
            if (transform.position.x < wrap.x || transform.position.x > wrap.y)
            {
                direction = -direction;
            }
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, wrap.x, wrap.y), transform.position.y, transform.position.z);
        }
    }
}