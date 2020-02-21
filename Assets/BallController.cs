using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Interpolator interp;
    // Start is called before the first frame update
    void Start()
    {
        interp = new Interpolator(1f);
    }

    // Update is called once per frame
    void Update()
    {
        interp.Update(Time.deltaTime);

        if (interp.IsMaxPrecise)
            interp.ToMin();
        else if (interp.IsMinPrecise)
            interp.ToMax();

        this.transform.position = Vector2.down * 2 + Vector2.up * 4 * interp.Value;
    }
}
