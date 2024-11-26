using UnityEngine;

public class LightManager : MonoBehaviour
{
    int count = 0, total = 6;
    [SerializeField] Material mat;
    float minIntensity = 2, maxIntensity = 12, initialIntensity = 8;

    void Start () {
        mat.SetFloat("_MinIntensity", initialIntensity);
        mat.SetFloat("MaxIntensity", initialIntensity);
    }

    public void TurnOnLight ()
    {
        if (count < total) {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).GetChild(count).gameObject.SetActive(true);
            }
        } else if (count == total) {
            mat.SetFloat("_MinIntensity", minIntensity);
            mat.SetFloat("MaxIntensity", maxIntensity);
        }
        count++;
    }
}
