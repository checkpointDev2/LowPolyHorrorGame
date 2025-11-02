using UnityEngine;
using UnityEditor;

public class FixScale : MonoBehaviour
{
    [MenuItem("Tools/Make All Object Scales Positive")]
    static void MakeAllScalesPositive()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        int fixedCount = 0;
        foreach (GameObject obj in allObjects)
        {
            Vector3 scale = obj.transform.localScale;
            bool changed = false;

            float newX = Mathf.Abs(scale.x);
            float newY = Mathf.Abs(scale.y);
            float newZ = Mathf.Abs(scale.z);

            if (newX != scale.x || newY != scale.y || newZ != scale.z)
            {
                obj.transform.localScale = new Vector3(newX, newY, newZ);
                changed = true;
            }

            if (changed)
                fixedCount++;
        }

        Debug.Log($"Fixed scale for {fixedCount} objects in the scene!");
    }
}
