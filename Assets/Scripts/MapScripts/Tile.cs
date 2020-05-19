using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Map tileMap;
    void OnMouseUp()
    {
        tileMap.GeneratePathTo(tileX, tileY);
    }
}
