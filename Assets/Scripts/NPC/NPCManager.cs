using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private List<string> hintList = new List<string>();
    private Dictionary<string, string> stickerToHintMap;

    void Start()
    {
        // Initialize the mapping
        stickerToHintMap = new Dictionary<string, string>
        {
            { "Balloon pieces", "Hint-Balloon" },
            { "Balloon cart-incomplete", "Hint-Cart" },
            { "Bush", "Hint-Bush" },
            { "Coin", "Hint-Coin" },
            // { "Ferris wheel-cabin", "Hint2" },
            // { "Ferris wheel fence", "Hint3" },
            { "Incomplete ferris wheel", "Hint-Ferris" },
            { "Ice cream cart", "Hint-Ice Cream" },
            { "Tree", "Hint-Tree" },
            { "Circus", "Hint-Circus" },

            // Add more mappings as needed
        };
    }

    // Function to add a hint based on the sticker name
    public void AddHint(string stickerName)
    {
        if (stickerToHintMap.TryGetValue(stickerName, out string hintName))
        {
            if (!hintList.Contains(hintName))
            {
                hintList.Add(hintName);
                Debug.Log($"Hint '{hintName}' added to NPC '{name}'");
            }
        }
        else
        {
            Debug.LogWarning($"Sticker '{stickerName}' does not have a mapped hint for NPC '{name}'");
        }
    }
}
