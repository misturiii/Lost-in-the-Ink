using UnityEngine;

public class TransformationFunction
{
    static float sketchbookWidth = 1780;
    static float sketchbookHeight = 1080;
    static float worldWidth = 40;
    static float worldHeight = 30;
    static float initialHeight = 5;

    static public Vector2 WorldToBook (Vector3 position) {
        float x = position.x * sketchbookWidth / worldWidth;
        float y = position.z * sketchbookHeight / worldHeight;
        return new Vector2(-x, -y);
    }

    static public Vector3 BookToWorld (Vector3 position) {
        float x = -position.x * worldWidth / sketchbookWidth;
        float z = -position.y * worldHeight / sketchbookHeight;
        return new Vector3(x, initialHeight, z);
    }
}
