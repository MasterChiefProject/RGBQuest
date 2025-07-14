using System.Collections;
using UnityEngine;

public class PoleResetter : MonoBehaviour
{
    [SerializeField] private GameObject polePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float delaySeconds = 5f;

    private GameObject currentPole;
    private bool isResetting;      // ← guard flag

    // -------------------------------------------------
    private void Start() => SpawnNewPole();

    public void DespawnAndRespawn()
    {
        if (!isResetting)                // ignore extra calls
            StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isResetting = true;

        // 1️⃣  Wait first, let the pole keep falling / exploding
        yield return new WaitForSeconds(delaySeconds);

        // 2️⃣  Now destroy whatever’s left of the old pole
        if (currentPole != null)
            Destroy(currentPole);

        //   …give Destroy() one frame to finish
        yield return null;

        // 3️⃣  Spawn a fresh copy
        SpawnNewPole();
        isResetting = false;
    }

    private void SpawnNewPole()
    {
        currentPole = Instantiate(polePrefab,
                                  spawnPoint.position,
                                  spawnPoint.rotation);
    }
}
