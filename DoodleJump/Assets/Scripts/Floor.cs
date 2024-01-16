using UnityEngine;
using UnityEngine.Animations;

public class Floor : MonoBehaviour
{
    public FloorObjectPool floorObjectPool;
    public FloorType floorType = FloorType.Normal;
    public Vector2 vector2Force = new Vector2(0, 100);
    private Animator _animator;
    private EdgeCollider2D _edgeCollider2D;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        if (Camera.main.transform.position.y - 5 > this.gameObject.transform.position.y)
        {
            SetActiveFalse();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal == Vector2.down)
        {
            GameObject gameObject = collision.gameObject;
            if (gameObject.CompareTag("Player") && gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                switch (floorType)
                {
                    case FloorType.Normal:
                        rigidbody2D.velocity = vector2Force;
                        break;
                    case FloorType.Move:
                        break;
                    case FloorType.Bounce:
                        break;
                    case FloorType.White:
                        break;
                    case FloorType.Weak:
                        if (_animator != null)
                        {
                            _animator.SetTrigger("Stamp");
                            Invoke("SetActiveFalse", 0.5f);
                        }
                        break;
                    case FloorType.Floor1:
                        break;
                    case FloorType.Floor2:
                        break;
                    case FloorType.Floor3:
                        break;
                    case FloorType.Floor4:
                        break;
                    case FloorType.Floor5:
                        break;
                    case FloorType.Break:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.CompareTag("Player") && gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            switch (floorType)
            {
                case FloorType.Normal:
                    break;
                case FloorType.Move:
                    break;
                case FloorType.Bounce:
                    break;
                case FloorType.White:
                    break;
                case FloorType.Weak:
                    break;
                case FloorType.Floor1:
                    break;
                case FloorType.Floor2:
                    break;
                case FloorType.Floor3:
                    break;
                case FloorType.Floor4:
                    break;
                case FloorType.Floor5:
                    break;
                case FloorType.Break:
                    break;
                default:
                    break;
            }
        }

    }

    private void SetActiveFalse()
    {
        floorObjectPool.Recovery(this.gameObject);
    }

}


