using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    void Awake()
    {
        if (Instance)                   //checks if another RoomManager exist
        {
            Destroy(gameObject);        //be gone! Another me
            return;
        }
        DontDestroyOnLoad(gameObject);  //only one RoomManager left
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /* PhotonPrefabs must be in the resources folder
     * Because Unity automatically excludes any files not referenced in the editor from the final build,
     * so here reference by string instead of the PhotonPrefabs.
     */
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1)// We're in the game scene (Different spawning object in Photon to normal Unity )
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    }
}
