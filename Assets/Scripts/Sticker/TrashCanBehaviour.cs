public class TrashCanBehaviour : ToolBehaviour
{

    public override void StartBehaviour(ItemSticker sticker)
    {
        sticker.Delete();
    }
}
