using UnityEngine;

namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(GameObject callingController);
    }
}
