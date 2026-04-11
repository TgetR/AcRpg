using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public List<Transform> teleporterPointsTransform = new List<Transform>();
    void Start()
    {
        teleporterPointsTransform = GameObject.FindGameObjectsWithTag("Teleport").Select(obj => obj.transform).ToList();
    }
    public void Teleport()
    {
        int rand = Random.Range(0, teleporterPointsTransform.Count);
        transform.position = teleporterPointsTransform[rand].position;
    }
    
}
