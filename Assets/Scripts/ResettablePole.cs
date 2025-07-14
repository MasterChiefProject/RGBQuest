using System.Linq;
using UnityEngine;
[DisallowMultipleComponent]
public class ResettablePole : MonoBehaviour
{
    private struct TransformData
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public bool active;
    }

    private Transform[] _transforms;
    private TransformData[] _original;

    void Awake()
    {
        _transforms = GetComponentsInChildren<Transform>(true);
        _original = _transforms.Select(t => new TransformData
        {
            localPosition = t.localPosition,
            localRotation = t.localRotation,
            localScale = t.localScale,
            active = t.gameObject.activeSelf
        }).ToArray();
    }

    public void ResetPole()
    {
        for (int i = 0; i < _transforms.Length; i++)
        {
            var t = _transforms[i];
            var data = _original[i];
            t.localPosition = data.localPosition;
            t.localRotation = data.localRotation;
            t.localScale = data.localScale;
            t.gameObject.SetActive(data.active);
            var rb = t.GetComponent<Rigidbody>();
            if (rb) { rb.velocity = rb.angularVelocity = Vector3.zero; rb.Sleep(); }
        }
    }
}
