using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Forwards, Backwards
}

[System.Serializable]
public struct Path {
    [SerializeField]
    Vector2[] points;

    public Path(Vector2[] _points) {
        points = _points;
    }

    private float GetLength() {
        Vector2[] points = this.points;
        return points.Skip(1).Select((x, i) => x-points[i]).Select(x => x.magnitude).Aggregate(0f, (a, b) => a+b);
    }

    /// <summary>
    /// Gets the point `progress` along the path
    /// Can crashes if `this.points` has less than two points or is null
    /// </summary>
    /// <param name="progress">Normalised in the range [0-1]</param>
    public Vector2 GetPosition(float progress, Direction direction) {
        float progressMade = 0f;
        float length = GetLength();
        for (int i=0; i<points.Length-1; i++) {

            /// considering `direction`
            Vector2 start = points[direction == Direction.Forwards ? i : points.Length-i-1];
            Vector2 end = points[direction == Direction.Forwards ? i+1 : points.Length-i-2];
            
            float step = (start-end).magnitude / length;
            if (progressMade + step >= progress) {
                // return a point between these two points
                return Vector2.Lerp(start, end, (progress-progressMade)/step);
            }
            progressMade += step;
        }
        return direction == Direction.Forwards ? this.points[this.points.Length-1] : this.points[0];
    }
}

public class FollowPath : MonoBehaviour
{
    public Path path;
    public Direction direction;
    public float progress;
    public float progressSpeed;

    // Start is called before the first frame update
    void Start()
    {
        progress = 0f;
        direction = Direction.Forwards;
    }

    // Update is called once per frame
    void Update()
    {
        progress += progressSpeed * Time.deltaTime;
        if (progress > 1f) {
            progress = 0f;
            direction = direction == Direction.Forwards ? Direction.Backwards : Direction.Forwards;
        }

        Vector2 position = path.GetPosition(progress, direction);
        transform.position = new Vector3(position.x, position.y, 0f);
    }
}
