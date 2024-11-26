using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AppearEffectManager : MonoBehaviour
{
    InputActions inputActions;
    Vector3 startPosition;
    float moveSpeed = 0.1f, offset = 1;
    [SerializeField] AnimationCurve factor;
    List<Vector3> targets;
    List<Transform> effects;
    [SerializeField] GameObject Prefab;
    Transform cam;
    public delegate void RemoveEffects ();
    public RemoveEffects Remove;
    void Start()
    {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += HandleAppearEffect;
        inputActions.Player.Trigger.performed += EndAllEffects;
        cam = Camera.main.transform;
        targets = new List<Vector3>();
        effects = new List<Transform>();
    }

    public int AddEffect (Vector3 position, int i) {
        if (i >= 0) {
            targets[i] = position;
        } else {
            i = effects.Count;
            targets?.Add(position);
        }
        return i;
    }

    void HandleAppearEffect (InputAction.CallbackContext context) {
        startPosition = cam.position + cam.forward * offset;
        foreach (var target in targets) {
            StartCoroutine(StartAppearEffect(target));
        }
    }

    void EndAllEffects (InputAction.CallbackContext context) {
        targets.Clear();
        foreach (var effect in effects) {
            Destroy(effect.gameObject);
        }
        effects?.Clear();
        Remove?.Invoke();
    }

    IEnumerator StartAppearEffect(Vector3 target) {
        Vector3 cur = startPosition;
        Vector3 direction = (target - cur).normalized * moveSpeed;
        Transform effect = Instantiate(Prefab, cur, Quaternion.identity).transform;
        effects.Add(effect);
        float progress = 0;
        while (Vector3.Dot(direction, target - cur) > 0) {
            if (effect) {
                effect.position = cur;
                cur += direction * factor.Evaluate(Mathf.Clamp(progress, 0, 1));
                progress += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            } else {
                break;
            } 
        }
        effects?.Remove(effect);
        if(effect){
            Destroy(effect?.gameObject);
        }
        Debug.Log("Effect end");
    }
}
