using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public float offset;
    public float offsetSmoothing;
    private Vector3 _playerPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _playerPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

        if (player.transform.localScale.x > 0f)
        {
            _playerPosition = new Vector3(_playerPosition.x + offset, _playerPosition.y, _playerPosition.z);
        }
        else
        {
            _playerPosition = new Vector3(_playerPosition.x - offset, _playerPosition.y, _playerPosition.z);
        }

        transform.position = Vector3.Lerp(transform.position, _playerPosition, offsetSmoothing * Time.deltaTime);
    }
}
