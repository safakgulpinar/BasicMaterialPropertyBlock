using UnityEngine;

namespace _BasicMPB.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private MPBController _mpbController;

        private void Awake()
        {
            _mpbController = GetComponent<MPBController>();
        }

        //Example of how to use the MPBController
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _mpbController.SetColor(Color.magenta);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _mpbController.AnimateColor(Color.green, 2f);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _mpbController.StartColorPulse(Color.red, Color.green, 2f, 1);
            }
        }
    }
}
