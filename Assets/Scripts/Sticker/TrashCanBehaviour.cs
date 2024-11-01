using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrashCanBehaviour : ToolBehaviour
{
    public AudioClip deleteStickerAudioClip;
    public AudioSource audioSourceTrashCan;
    public override string StartBehaviour(ItemSticker sticker)
    {   
        if (sticker.TrashCan()) {
            audioSourceTrashCan = GetComponent<AudioSource>();
            if (audioSourceTrashCan != null && !audioSourceTrashCan.isPlaying)
            {
                audioSourceTrashCan.PlayOneShot(deleteStickerAudioClip);
                Debug.Log("audio for delete is played");
            }
            return $"Successfully deleted {sticker.item.itemName} sticker";
        } else {
            return FunctionLibrary.HighlightString($"{sticker.item.itemName} sticker is unique, cannot delete it");
        }
    }
}
