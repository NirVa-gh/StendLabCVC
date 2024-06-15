using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnerObject : MonoBehaviour
{
    [Header("Setup objects")]
    [SerializeField]
    private GameObject[] ObjectArray;
    private int ObjectIndex;
    [Header("Spawn setup")]
    private Vector3 ObjectPosition;
    [SerializeField]
    private float Delay = 1.0f;
    [SerializeField] 
    private Transform spawn_point;
    [SerializeField]
    [Tooltip("Square spawner size")] 
    private Vector3 volume;
    [SerializeField]
    private Transform trash;
    private bool _turnOnOff = false;
    [Header("Scale objects")]
    [Range(0.01f,0.2f)]public float minSize = 0.01f;
    [Range(0.01f,0.2f)]public float maxSize = 0.01f;
    public void TurnOnOff()
    {
        _turnOnOff = !_turnOnOff;
        if(_turnOnOff)
        {
            Debug.Log("InvokeRepeating");
            InvokeRepeating("SpawnObject", 0f, Delay);
        }
        else
        {
            Debug.Log("CancelInvoke");
            CancelInvoke("SpawnObject");
        }
    }
    public void StartInvoke()
    {
        InvokeRepeating("SpawnObject", 1f, Delay);
    }
    public void StopInvoke()
    {
        CancelInvoke("SpawnObject");
    }
    private void SpawnObject()
    {
        Vector3 ObjectPosition = new Vector3(
            Random.Range(spawn_point.position.x - volume.x, spawn_point.position.x + volume.x),
            spawn_point.position.y,
            Random.Range(spawn_point.position.z - volume.z, spawn_point.position.z + volume.z)
                                            );
        ObjectIndex = Random.Range(0, ObjectArray.Length);
        GameObject clone = ObjectArray[ObjectIndex];
        var randomValue = Random.Range(minSize,maxSize);
        clone.transform.localScale = new Vector3 (randomValue,randomValue,randomValue);                                    
        var cloneInstantiate = Instantiate(ObjectArray[ObjectIndex], ObjectPosition, Quaternion.identity);
        cloneInstantiate.transform.SetParent(trash);
    }
    public void DestroyTrash()
    {
        foreach(Transform obj in trash)
        {
            Destroy(obj.gameObject);
        }
    }
}
