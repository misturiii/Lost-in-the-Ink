public class CameraBehaviour : ToolBehaviour
{

    public override string StartBehaviour(ItemSticker sticker)
    {
        sticker.Camera();
        return "";
    }
}
