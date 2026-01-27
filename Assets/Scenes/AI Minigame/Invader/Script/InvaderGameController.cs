using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InvaderTimeController))]
public class InvaderGameController : MonoBehaviour
{
    public static InvaderGameController instance;

    [HideInInspector] public bool isPlayable = false;
    [HideInInspector] public bool isInProgress = false;

    // Enemyの移動範囲
    [SerializeField] Transform enemyLeftLimit;
    [SerializeField] Transform enemyRightLimit;
    [HideInInspector] public float leftLimitX;
    [HideInInspector] public float rightLimitX;

    [Header("制限時間")]
    public float timeLimit = 60f;

    [Header("UI上弾を並べるところ")]
    [SerializeField] GameObject AmmoIconContainer;

    [Header("残弾、もしくは次に撃つ弾")]
    public List<GameObject> AmmoList;

    [SerializeField] GameObject startCanvas;

    InvaderTimeController invaderTimeController;

    private void Awake()
    {
        // Singletonパターン
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        // Enemyの移動範囲の設定
        leftLimitX = enemyLeftLimit.position.x;
        rightLimitX = enemyRightLimit.position.x;
        if(leftLimitX > rightLimitX)
            Debug.LogAssertion("leftLimitとrightLimitが逆になっています");
    }


    private void Start()
    {
        // コンポーネントの取得
        invaderTimeController = GetComponent<InvaderTimeController>();
        if (invaderTimeController == null)
            Debug.LogError("PuzzleTimeControllerコンポーネントが見つかりません。");

        isPlayable = false;
        isInProgress = false;

        startCanvas.SetActive(true);
    }

    public void GameStart()
    {
        Debug.Log("ゲームスタート");
        isInProgress = true;
        isPlayable = true;
        invaderTimeController.StartTimer();
    }


    public void GameOver()
    {
        isInProgress = false;
        isPlayable = false;

        // 成功処理
        Debug.Log("成功");
    }


    /// <summary>
    /// 残弾に弾を追加する
    /// </summary>
    /// <param name="newBullet">追加する弾のprefab</param>
    public void AddAmmoIcon(GameObject newBulletIconPrefab)
    {
        GameObject newBulletIcon = Instantiate(newBulletIconPrefab, AmmoIconContainer.transform);
        newBulletIcon.transform.parent = AmmoIconContainer.transform;
        AmmoList.Add(newBulletIcon);
    }

    /// <summary>
    /// 残弾の次の弾を取得する
    /// </summary>
    /// <returns>次の弾</returns>
    public GameObject GetNextAmmo()
    {
        GameObject nextAmmoIcon = null;
        GameObject nextAmmo = null;

        if (AmmoList.Count == 0)
            return null;

        nextAmmoIcon = AmmoList[0];
        
        InvaderAmmoIcon iconScript = nextAmmoIcon.GetComponent<InvaderAmmoIcon>();
        if (iconScript == null)
            Debug.LogAssertion("InvaderAmmoIconの取得に失敗しました");
        else
            nextAmmo = iconScript.GetThisAmmo();


        AmmoList.RemoveAt(0);
        Destroy(nextAmmoIcon);

        return nextAmmo;
    }
}
