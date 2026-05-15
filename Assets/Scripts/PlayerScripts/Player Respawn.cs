using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject respawnPoint;

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        Instantiate(playerPrefab, respawnPoint.transform.position, Quaternion.identity);

    }
}
