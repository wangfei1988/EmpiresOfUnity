using UnityEngine;
using System.Collections.Generic;

/*
 * CursorObj
 * Used for Animated Cursor Component
 * Usage: Create an Asset of this ScriptableObject
 * @date 2014-04-26
 */
public class CursorObj : ScriptableObject
{
    public AnimatedCursor.CURSOR CursorType;
    public Vector2 ClickPoint = Vector2.zero;
    public float AnimationFps = 3;
    public List<Texture2D> TextureList;
}
