using UnityEngine;

public class FunctionLibrary
{
    static float sketchbookWidth = 1780;
    static float sketchbookHeight = 1080;
    static float worldWidth = 40;
    static float worldHeight = 30;
    static float initialHeight = 0;

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

    static public string styleStart = "<b><color=#a10103>";
    static public string styleEnd = "</color></b>";

    static public string HighlightString (string s) {
        return styleStart + s + styleEnd;
    }

    public static Color LineColor1 = new Color(0.7f, 0.9f, 1f);
    public static Color LineColor2 = new Color(1, 0.86f, 0.26f);
}
