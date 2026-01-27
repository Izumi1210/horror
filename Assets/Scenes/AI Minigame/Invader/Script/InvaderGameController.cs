using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderGameController : MonoBehaviour
{
    public static InvaderGameController instance;

    [SerializeField] Transform enemyLeftLimit;
    [SerializeField] Transform enemyRightLimit;
    [HideInInspector] public float leftLimitX;
    [HideInInspector] public float rightLimitX;

    [Header("UIã’e‚ğ•À‚×‚é‚Æ‚±‚ë")]
    [SerializeField] GameObject AmmoIconContainer;

    [Header("c’eA‚à‚µ‚­‚ÍŸ‚ÉŒ‚‚Â’e")]
    public List<GameObject> AmmoList;

    private void Awake()
    {
        // Singletonƒpƒ^[ƒ“
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        // Enemy‚ÌˆÚ“®”ÍˆÍ‚Ìİ’è
        leftLimitX = enemyLeftLimit.position.x;
        rightLimitX = enemyRightLimit.position.x;
        if(leftLimitX > rightLimitX)
            Debug.LogAssertion("leftLimit‚ÆrightLimit‚ª‹t‚É‚È‚Á‚Ä‚¢‚Ü‚·");
    }

    /// <summary>
    /// c’e‚É’e‚ğ’Ç‰Á‚·‚é
    /// </summary>
    /// <param name="newBullet">’Ç‰Á‚·‚é’e‚Ìprefab</param>
    public void AddAmmoIcon(GameObject newBulletIconPrefab)
    {
        GameObject newBulletIcon = Instantiate(newBulletIconPrefab, AmmoIconContainer.transform);
        newBulletIcon.transform.parent = AmmoIconContainer.transform;
        AmmoList.Add(newBulletIcon);
    }

    /// <summary>
    /// c’e‚ÌŸ‚Ì’e‚ğæ“¾‚·‚é
    /// </summary>
    /// <returns>Ÿ‚Ì’e</returns>
    public GameObject GetNextAmmo()
    {
        GameObject nextAmmoIcon = null;
        GameObject nextAmmo = null;

        if (AmmoList.Count == 0)
            return null;

        nextAmmoIcon = AmmoList[0];
        
        InvaderAmmoIcon iconScript = nextAmmoIcon.GetComponent<InvaderAmmoIcon>();
        if (iconScript == null)
            Debug.LogAssertion("InvaderAmmoIcon‚Ìæ“¾‚É¸”s‚µ‚Ü‚µ‚½");
        else
            nextAmmo = iconScript.GetThisAmmo();


        AmmoList.RemoveAt(0);
        Destroy(nextAmmoIcon);

        return nextAmmo;
    }
}
