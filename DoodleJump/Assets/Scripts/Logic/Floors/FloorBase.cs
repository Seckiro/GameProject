using UnityEngine;

public abstract class FloorBase : MonoBehaviour
{
    public abstract Vector2 Vector2Force { get; }

    public FloorObjectPool FloorObjectPool
    {
        private get
        {
            return _floorObjectPool;
        }
        set
        {
            _floorObjectPool = value;
        }
    }

    private FloorObjectPool _floorObjectPool;

    public virtual void OnDepress(GameObject gameObject, Rigidbody2D rigidbody2D) { }

    public virtual void OnJumpUp(GameObject gameObject, Rigidbody2D rigidbody2D) { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal == Vector2.down)
        {
            GameObject gameObject = collision.gameObject;
            if (gameObject.CompareTag("Player") && gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.velocity = Vector2Force;
                OnDepress(gameObject, rigidbody2D);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.CompareTag("Player") && gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            OnJumpUp(gameObject, rigidbody2D);
        }
    }

    public void Recovery()
    {
        _floorObjectPool.Recovery(this.gameObject);
    }

}
