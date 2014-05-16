using UnityEngine;

public class SelectorScript : MonoBehaviour {

    public UnitGroup group;
    public Bounds SellectionBounds
    {
        get { return gameObject.collider.bounds; }
    }

	void Start () 
    {
        group = ScriptableObject.CreateInstance<UnitGroup>();
        group.ResetGroup();
        gameObject.collider.enabled = false;
    }

    public UnitGroup SnapSelection()
    {
        gameObject.collider.enabled = true;
        group.ResetGroup();

        // Check Selected
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Clickable"))
        {
            if (gameObject.collider.bounds.Contains(unit.transform.position) && unit.GetComponent<UnitScript>())
            {
                group.BeginGroupFill(unit);
            }
        }

        group.EndGroupFill();
        gameObject.collider.enabled = false;
        return group;
    }
}
