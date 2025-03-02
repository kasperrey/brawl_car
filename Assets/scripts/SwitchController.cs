using Cinemachine;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    public GameObject character;

    public GameObject car;
    public CinemachineVirtualCamera vcam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (character.activeSelf)
            {
                character.SetActive(false);
                car.SetActive(true);
                vcam.Follow = car.transform.GetChild(2).transform;
                car.transform.position = character.transform.position;
                car.transform.rotation = character.transform.rotation;
            }
            else
            {
                character.SetActive(true);
                car.SetActive(false);
                vcam.Follow = character.transform.GetChild(0).transform;
                character.transform.position = car.transform.position;
                character.transform.rotation = car.transform.rotation;
            }
        }
    }
}