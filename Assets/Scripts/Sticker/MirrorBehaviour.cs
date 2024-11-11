using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBehaviour : ToolBehaviour
{

    public override string StartBehaviour(ItemSticker sticker)
    {
        sticker.Mirror(1);
        return "";
    }
}

