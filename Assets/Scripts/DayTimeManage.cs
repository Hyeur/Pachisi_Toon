using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeManage : MonoBehaviour
{
    [Header("Shadow Direction")] 
    [SerializeField] [Range(30,150)] private float x = 30f;
    [SerializeField] [Range(0,45)] private float y = 0f;
    [SerializeField] [Range(1, 10)] private float speed;

    [Space] 
    [SerializeField] [Range(0, 100)]
    private float percent = 0;

    [SerializeField] private bool increasing = true;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        dayLoop();
    }

    private void dayLoop()
    {
        if (this.percent > 99) this.increasing = false;
        if (this.percent < 1) this.increasing = true;

        if (increasing)
        {
            this.percent += 1 * Time.deltaTime * speed;
        }
        else
        {
            this.percent -= 1 * Time.deltaTime * speed;
        }

        this.x = percentX(this.percent);
        this.y = percentY(this.percent);
        Quaternion rotation = Quaternion.Euler(x,y,0);
        transform.rotation = rotation;
    }
    
    private float percentX(float percent)
    {
        float per = (150f - 30f)/100f;
        return (per * percent) + 30f;
    }
    private float percentY(float percent)
    {
        float per = 45f/100f;
        
        return (per * percent);
    }
}
