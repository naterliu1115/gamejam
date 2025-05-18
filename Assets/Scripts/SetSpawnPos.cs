using Platformer.Mechanics;
using UnityEngine;
using System.Collections.Generic;

public class SetSpawnPos : MonoBehaviour
{
    static public SetSpawnPos _;

    public PlayerController Player;
    public List<Collider2D> Colliders;

    public int Index = -1;

    private void Awake()
    {
        _ = this;
    }
    private void Update()
    {
        foreach(var i in Colliders)
        {
            if (Player.collider2d.IsTouching(i))
            {
                int Newindex = Colliders.IndexOf(i);
                if (Newindex > Index)
                {
                    Index = Newindex;
                    //Player.model.spawnPoint.position = i.transform.position;
                }
                break;
            }
        }
    }
    public Vector3 GetPos()
    {
        return Colliders[Index].transform.position;
    }
}
