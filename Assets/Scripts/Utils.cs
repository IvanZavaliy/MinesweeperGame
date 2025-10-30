using UnityEngine;

public static class Utils
{
    public static RaycastHit2D GetRaycastHit2DFromMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics2D.Raycast(ray.origin, ray.direction);
    }
}
