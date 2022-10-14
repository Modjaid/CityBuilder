using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    // [SerializeField] private int _mapZ;
    // [SerializeField] private int _mapX;
    //
    // [SerializeField] private float _offsetY;
    // [SerializeField] private float _offsetX;
    //
    // [SerializeField] private GameObject _tile;
    //
    //
    // [ContextMenu("Generation")]
    // private void Generator()
    // {
    //     var mapGO = new GameObject();
    //
    //     mapGO.name = $"[{_mapX}X{_mapZ}]";
    //     float offsetX = 0;
    //     float offsetZ = 0;
    //
    //     for (int z = 0; z < _mapZ; z++)
    //     {
    //         offsetX = 0;
    //         for (int x = 0; x < _mapX; x++)
    //         {
    //             var newSprite = Instance(x, z, mapGO.transform);
    //             newSprite.transform.position = new Vector3(offsetX, 0, offsetZ);
    //             offsetX += _offsetX;
    //         }
    //
    //         offsetZ += _offsetY;
    //     }
    // }
    
    // private Transform Instance(int x, int y, Transform parent)
    // {
    //     var newTile = Instantiate(_tile, parent);
    //     newTile.name = $"[X:{x}] [Y:{y}]";
    //     newTile.AddComponent<Tile>().Pos = new Vector2Int(x,y);
    //     return newTile.transform;
    // }

    // private Transform Instance(int x, int y, Transform parent)
    // {
    //     int randIndex = Random.Range(0, _tiles.Length);
    //     var newSprite = Instantiate(_tiles[randIndex], parent);
    //     newSprite.name = $"[X:{x}] [Y:{y}]";
    //     var tile = newSprite.gameObject.GetComponent<Tile>();
    //     tile.Pos = new Vector2Int(x, y);
    //     return newSprite.transform;
    // }


}
