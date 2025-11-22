using UnityEngine;

public static class Utils
{
    public static RaycastHit2D GetRaycastHit2DFromMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics2D.Raycast(ray.origin, ray.direction);
    }
    
    public static RaycastHit2D GetRaycastHit2DFromWorldObjectPosition(Vector2 worldObjectPosition)
    {
        return Physics2D.Raycast(worldObjectPosition, Vector2.zero); 
    }
}
