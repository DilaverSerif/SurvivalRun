using UnityEngine;

public class Finish : MonoBehaviour
{
    private Vector3 playerStartPos;

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Health>(out var player)) {
            Base.FinisGame(GameStat.Win,1f);
        }
    }
}
