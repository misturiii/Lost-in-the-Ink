using UnityEngine;

public class CheckGameObjects : MonoBehaviour
{
    public GameObject leo;     
    public string[] requiredObjects = new string[] { "Object1", "Object2", "Object3", "Object4", "Object5" };  // List of GameObject names to check

    void Start()
    {
        if (leo != null)
        {
            leo.SetActive(false);
        }
        CheckAllObjects();
    }

    public void CheckAllObjects()
    {
        bool allObjectsExist = true;
        foreach (string objName in requiredObjects)
        {
            GameObject foundObject = GameObject.Find(objName);

            if (foundObject == null)
            {
                Debug.Log($"GameObject '{objName}' not found in the world.");
                allObjectsExist = false;
                break;
            }
        }
        if (allObjectsExist && leo != null)
        {
            Debug.Log("All required GameObjects are present. Showing Leo.");
            leo.SetActive(true);
        }
    }
}
