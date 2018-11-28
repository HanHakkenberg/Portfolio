using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{

    public static CursorManager instance;

    private Vector3 mousePos;
    private Camera mainCam;

    public bool cursorVisible;
    public bool CursorVisibilityStandard { get; private set; }

    public GameObject cursorObject;
    private Transform cursorObjectPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        mainCam = Camera.main;

        if (cursorObject != null)
        {
            cursorObjectPos = Instantiate(cursorObject).transform;
        }

        Cursor.visible = cursorVisible;
        CursorVisibilityStandard = cursorVisible;
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - mainCam.transform.position.z;
        mousePos = mainCam.ScreenToWorldPoint(mousePos);

        if (cursorObject != null)
        {
            cursorObjectPos.position = mousePos;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastWorldUI();
        }
    }

    public void ToggleCursorObject()
    {
        cursorObjectPos.gameObject.SetActive(!cursorObjectPos.gameObject.activeSelf);
    }

    private void RaycastWorldUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count == 0)
        {
            CastleUpgradeManager.instance.CloseAllUI(null);
        }
        else
        {
            results.Clear();
        }
    }
}
