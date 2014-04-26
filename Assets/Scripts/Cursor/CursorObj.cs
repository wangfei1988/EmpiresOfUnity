using UnityEngine;
using System.Collections.Generic;

public class CursorObj : ScriptableObject
{
    public AnimatedCursor.CURSOR CursorType;
    public Vector2 ClickPoint = Vector2.zero;
    public List<Texture2D> TextureList;
}
