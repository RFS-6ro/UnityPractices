using UnityEngine;

public class SendGameCommand : MonoBehaviour
{
    public TriggerCommandsType command;
    public GameCommandReciever[] recievers;
    public bool oneShot = false;
    public float cooldown = 0f;
    public float lastSendTime = 0f;
    public bool isTriggered = false;
    public SendGameCommand auth;

    protected Transform _sender;
    protected Transform[] _recievers;

    private void Awake()
    {
        if (command == TriggerCommandsType.Save)
        {
            recievers = new GameCommandReciever[1];
            recievers[0] = FindObjectOfType<SaveGameManager>().GetComponent<GameCommandReciever>();
        }
    }

    public void Send()
    {
        if (oneShot && isTriggered) return;
        if (Time.time - lastSendTime < cooldown) return;

        isTriggered = true;
        lastSendTime = Time.time;
        foreach (var reciever in recievers)
        {
            reciever.Recieve(command);
        }
    }
}
