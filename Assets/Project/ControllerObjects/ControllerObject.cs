using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObject : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();
    [SerializeField]
    private Transform resetPoint;
    [SerializeField]
    private Transform controllerObject;
    [SerializeField]
    [Range(1f, 30f)] private float jumpForce = 2f;
    private Rigidbody _rigidbody;
    [SerializeField]
    private Material OnMaterial;
    [SerializeField]
    private Material OffMaterial;
    private bool _turnOnOff = false;

    private GameObject SearchObject(string Name)
    {
        foreach (Transform obj in controllerObject)
        {
            if(obj.name == Name)
            {
                //Debug.Log(obj);
                return obj.gameObject;              
            }
        }
        return null;
    }

    private void jump(GameObject obj, float direction)
    {
        if (obj.GetComponent<Rigidbody>() != null && obj.GetComponent<Status>() != null)
        {
            if (obj.GetComponent<Status>().onGround == true)
            {
                StartCoroutine(ExecuteAfterTime(obj,1.4f));
                obj.GetComponent<Status>().onGround = false;
                _rigidbody = obj.GetComponent<Rigidbody>();
                _rigidbody.isKinematic = false;
                _rigidbody.AddForce(new Vector3(direction,1,0) * jumpForce, ForceMode.Impulse);
            }
        } 
    }

    IEnumerator ExecuteAfterTime(GameObject obj, float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        if (obj != null)
        {
            obj.GetComponent<Status>().onGround = true;
        }
    }

    public void rightArrow(string Name)
    {
        jump(SearchObject(Name), 1);
    }

    public void leftArrow(string Name)
    {
        jump(SearchObject(Name), -1);
    }

    public void TurnOnOff(string Name)
    {
        _turnOnOff = !_turnOnOff;
        if (SearchObject(Name) != null)
        {
            var obj = SearchObject(Name);
            if (_turnOnOff)
            {
                if (obj.GetComponent<Renderer>() != null && obj.transform.GetChild(0).GetComponent<Light>() != null)
                {
                    obj.GetComponent<Renderer>().material = OnMaterial;
                    obj.transform.GetChild(0).GetComponent<Light>().enabled = true;
                }   
            }
            else
            { 
                if (obj.GetComponent<Renderer>() != null && obj.transform.GetChild(0).GetComponent<Light>() != null)
                {
                    obj.GetComponent<Renderer>().material = OffMaterial;
                    obj.transform.GetChild(0).GetComponent<Light>().enabled = false;
                }
            }
        }
    }

    public void Spawn(string Name)
    {
        foreach (var obj in objects)
        {
            if(obj != null!)
            {
                if (obj.transform.name == "Cube")
                {
                    var clone = Instantiate(obj, resetPoint.position, Quaternion.identity);
                    clone.transform.SetParent(controllerObject);
                } 
            }

        }
    }

    public void ResetObject(string Name)
    {
        _turnOnOff = false;
        Destroy();
        Spawn(Name);
    }

    public void Destroy()
    {
        foreach(Transform obj in controllerObject)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
           
        }
    }
    
    private void DestroyWithoutOne()
    {
        if (controllerObject.childCount > 1)
        {
            for (int i = 0; i < controllerObject.childCount - 1; i++)
            {
                GameObject.Destroy(controllerObject.GetChild(i).gameObject);
            }
        }
    }
}
