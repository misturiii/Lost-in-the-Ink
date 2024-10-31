public class TrashCanBehaviour : ToolBehaviour
{
    public override string StartBehaviour(ItemSticker sticker)
    {
        if (sticker.TrashCan()) {
            return $"Successfully deleted {sticker.item.itemName} sticker";
        } else {
            return FunctionLibrary.HighlightString($"{sticker.item.itemName} sticker is unique, cannot delete it");
        }
    }
}
