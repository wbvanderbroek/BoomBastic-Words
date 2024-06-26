using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : NetworkBehaviour {
    [SerializeField] private string Username;
    [SerializeField] private RoundManager RoundManager;
    [SerializeField] private GameplayManager GameplayManager;
    private Dictionary<string, PlayerClass> playerClasses = new Dictionary<string, PlayerClass>();

    // Start is called before the first frame update
    void Start() {
        if (!IsOwner) return;
        RoundManager = FindObjectOfType<RoundManager>();
        GameplayManager = FindObjectOfType<GameplayManager>();
        Username = $"User_{Mathf.Floor(Random.Range(1000, 9999))}";

        PlayerClass newPlayerClass = new PlayerClass(Username, RoundManager);
        playerClasses.Add(Username, newPlayerClass);

        RoundManager.AddUserToListServerRPC(Username);
        newPlayerClass.ResetLives();

        GameObject SendButton = GameObject.FindGameObjectWithTag("SendButton");
        if (SendButton) {
            SendButton.GetComponent<Button>().onClick.AddListener(SendMessage);
        }
    }

    public string GetUserName() {
        if (!IsOwner) return "TWENTYONECHARACTERS";
        return Username;
    }

    public void SendMessage() {
        if (!IsOwner) return;
        string Field = GameplayManager.ReturnMessageField();
        RoundManager.SendMessageServerRPC(Field, GetUserName());
    }
}
