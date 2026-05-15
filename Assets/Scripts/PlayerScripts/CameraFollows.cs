using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    [SerializeField] Camera Camera;
    [SerializeField] GameObject Player;
    private void Awake()
    {
            
    }
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
            Camera.transform.position = new Vector3(Player.transform.position.x, Camera.transform.position.y, Player.transform.position.z);
            
        
    }


    
    public void resetplayer(GameObject player)
    {
            Player = player;
    }
}
