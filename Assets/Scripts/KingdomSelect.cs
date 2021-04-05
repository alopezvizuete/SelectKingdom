using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class KingdomSelect : MonoBehaviour
{
    public List<Kingdom> kingdoms = new List<Kingdom>();

    [Header("Public References")]
    public GameObject kingdomPointPrefab;
    public GameObject kingdomButtonPrefab;
    public Transform modelTransform;
    public Transform kingdomButtonsContainer;

    [Header("Tween Settings")]
    public float lookDuration;
    public Ease lookEase;

    [Space]
    public Vector2 visualOffset;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Kingdom k in kingdoms)
        {
            SpawnKingdomPoint(k);
        }

        if (kingdoms.Count > 0)
        {
            LookAtKingdom(kingdoms[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookAtKingdom(Kingdom k)
    {
        Transform cameraParent = Camera.main.transform.parent;
        Transform cameraPivot = cameraParent.parent;

        cameraParent.DOLocalRotate(new Vector3(k.y, 0, 0), lookDuration, RotateMode.Fast).SetEase(lookEase);
        cameraPivot.DOLocalRotate(new Vector3(0, -k.x, 0), lookDuration, RotateMode.Fast).SetEase(lookEase);

    }

    [System.Serializable]
    public class Kingdom
    {
        public string name;

        [Range(-180, 180)]
        public float x;
        [Range(-89, 89)]
        public float y;

        [HideInInspector]
        public Transform visualPoint;
    }


    private void SpawnKingdomPoint(Kingdom k)
    {
        GameObject kingdom = Instantiate(kingdomPointPrefab, modelTransform);
        kingdom.transform.localEulerAngles = new Vector3(k.y + visualOffset.y, -k.x - visualOffset.x, 0);
        k.visualPoint = kingdom.transform.GetChild(0);

        SpawnKingdomButton(k);
    }

    private void SpawnKingdomButton(Kingdom k)
    {
        Button kingdomButton = Instantiate(kingdomButtonPrefab, kingdomButtonsContainer).GetComponent<Button>();
        kingdomButton.onClick.AddListener(() => LookAtKingdom(k));
        kingdomButton.transform.GetChild(0).GetComponentInChildren<Text>().text = k.name;
    }
}
