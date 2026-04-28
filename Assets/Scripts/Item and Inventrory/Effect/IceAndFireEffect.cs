using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/Item Effect/Ice And Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xveiocity;

    public override void ExecuteEffect(Transform _RespondTransform)
    {
        Player player = PlayerManager.Instance.player;

        bool thirdAttack = player.PrimaryAttack.comboCounter == 2;

        if(thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _RespondTransform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xveiocity * player.facingDir, 0);
            Destroy(newIceAndFire, 2);
        }

    }
}
