using UnityEngine;

public class FlagManager : MonoBehaviour
{
    [SerializeField] private FlagList flagList;

    public static FlagManager Instance { get; private set; }

    public FlagList FlagList => flagList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // ƒV[ƒ“‚ğ‚Ü‚½‚¢‚Å‚à•Û
    }
}
