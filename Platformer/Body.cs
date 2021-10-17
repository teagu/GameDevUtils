using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2 size;

    public Vector2 Center => (Vector2)transform.position + offset;
    public Vector2 Size => size;

    public void Move(Vector2 motion)
    {
        CheckCollision(ref motion, 0);
        CheckCollision(ref motion, 1);
        transform.position += (Vector3)motion;
    }

    public bool IsOverlaping(Vector2 offset = default)
    {
        var point = Center + offset;
        var size = Size - Vector2.one;
        return Physics2D.OverlapBox(point, size, 0);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Center, Size);
    }

    private void CheckCollision(ref Vector2 motion, int index)
    {
        var axis = new Vector2[] { Vector2.right, Vector2.up };
        var origin = Center + axis[index] * 0.5F * Mathf.Sign(motion[index]);
        var size = Size - Vector2.one;
        var direction = motion * axis[index];
        var distance = Mathf.Abs(motion[index]);
        var hit = Physics2D.BoxCast(origin, size, 0, direction, distance);

        if (hit)
        {
            motion[index] = hit.distance * Mathf.Sign(motion[index]);
        }
    }
}