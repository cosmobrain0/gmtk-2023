using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagement : MonoBehaviour
{
    public GameObject levelStuffsParent;
    public GameObject archerPrefab;
    public GameObject wallPrefab;
    public GameObject targetPrefab;

    string[][] levels;
    public int currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        levels = new string[][] {
            new string[] {
                "archers:",
                "  -2, -2, 2, -2, 4, 3, 0",
                "",
                "targets:",
                "  2, 3",
                "  0, 0",
                "",
                "walls:",
                "  -1, -1, 1, -1",
                "  2, 0, 3, -1",
                ""
            }
        };
        levelStuffsParent.transform.position = new Vector3(0, 0, 0);

        SetCurrentLevel(0);
    }

    public void SetCurrentLevel(int level) {
        if (level >= 0 & level < levels.Length) {
            foreach (Transform child in levelStuffsParent.transform) {
                Destroy(child.gameObject);
            }
            currentLevel = level;
            string[] levelData = levels[currentLevel].Select(x => x.TrimEnd()).Where(x => x.Length != 0).ToArray();
            BlockType currentBlockType = BlockType.Unknown;

            IEnumerable<float[]> archers = (IEnumerable<float[]>)new List<float[]>();
            IEnumerable<float[]> walls = (IEnumerable<float[]>)new List<float[]>();
            IEnumerable<float[]> targets = (IEnumerable<float[]>)new List<float[]>();

            for (int i=0; i<levelData.Length; i++) {
                bool knownBlockType = currentBlockType != BlockType.Unknown;
                bool listOfNumbers = levelData[i].StartsWith("  ");
                bool blockName = !listOfNumbers && levelData[i][levelData[i].Length-1] == ':';
                if (knownBlockType && listOfNumbers) {
                    float[] numbers = levelData[i].Split(",").Select(x => float.Parse(x)).ToArray();
                    if (currentBlockType == BlockType.Archers && numbers.Length > 3 && numbers.Length%2 == 1 && (numbers[numbers.Length-1] == 0f || numbers[numbers.Length-1] == 1f)) {
                        // these numbers are for an archer
                        archers = archers.Append(numbers);
                    } else if (currentBlockType == BlockType.Targets && numbers.Length == 2) {
                        // these numbers are for a target
                        targets = targets.Append(numbers);
                    } else if (currentBlockType == BlockType.Walls && numbers.Length == 4 && !(numbers[0] == numbers[2] && numbers[1] == numbers[3])) {
                        // these numbers are for a wall
                        walls = walls.Append(numbers);
                    } else {
                        // these numbers are invalid
                        // so ignore them
                    }
                } else if (blockName) {
                    switch (levelData[i].Split(":")[0]) {
                        case "archers":
                            currentBlockType = BlockType.Archers;
                            break;
                        case "walls":
                            currentBlockType = BlockType.Walls;
                            break;
                        case "targets":
                            currentBlockType = BlockType.Targets;
                            break;
                        default:
                            // ignore invalid block names
                            break;
                    }
                } else {
                    // idk what this is,
                    // so ignore it
                }
            }


            GameObject[] archerObjects = archers.Select(x => SpawnArcher(x)).ToArray();
            GameObject[] wallObjects = walls.Select(x => SpawnWall(x)).ToArray();
            GameObject[] targetObjects = targets.Select(x => SpawnTarget(x)).ToArray();
        }
    }

    GameObject SpawnArcher(float[] values) {
        float[] ordinates = values.Take(values.Length-1).ToArray();
        Vector2[] coordinates = new Vector2[ordinates.Length/2];
        for (int i=0; i<coordinates.Length; i++) {
            coordinates[i] = new Vector2(ordinates[i*2], ordinates[i*2+1]);
        }
        if (values.Last() == 1f) coordinates = coordinates.Append(coordinates[0]).ToArray();
        
        GameObject archer = Instantiate(archerPrefab);
        FollowPath script = archer.GetComponent<FollowPath>();
        script.path = new Path((Vector2[])coordinates.Clone());
        archer.transform.parent = levelStuffsParent.transform;
        return archer;
    }

    GameObject SpawnWall(float[] values) {
        Vector2 a = new Vector2(values[0], values[1]);
        Vector2 b = new Vector2(values[2], values[3]);
        Vector2 centre = (a+b)/2f;
        float width = (a-b).magnitude;
        float angle = Mathf.Atan2(a.y-b.y, a.x-b.x)*180f/Mathf.PI;

        GameObject wall = Instantiate(wallPrefab);
        wall.transform.position = new Vector3(centre.x, centre.y, 0f);
        wall.transform.rotation = Quaternion.Euler(0, 0, angle);
        wall.transform.parent = levelStuffsParent.transform;
        return wall;
    }

    GameObject SpawnTarget(float[] values) {
        Vector2 position = new Vector2(values[0], values[1]);

        GameObject target = Instantiate(targetPrefab);
        target.transform.position = new Vector3(position.x, position.y, 0f);
        target.transform.parent = levelStuffsParent.transform;
        return target;
    }

}
