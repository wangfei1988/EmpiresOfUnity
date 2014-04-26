using UnityEngine;
using System.Collections;

/*
 * Event Handler for Update per frame.
 * Use this instead of Unity "void Update()"-Method
 * Usage:
 *  - UpdateHandler.OnUpdate += DoUpdate;
 *  - void DoUpdate() {}
 * @date 2014-04-26
 * @author Dario D. Müller
 * @contact <moin@game-coding.com>
 */
public class UpdateHandler : MonoBehaviour
{

    public delegate void UpdateEvent();
    public static event UpdateEvent OnUpdate;
	
	void Update()
    {
        DoEventStuff();
	}

    private void DoEventStuff()
    {
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    
}
