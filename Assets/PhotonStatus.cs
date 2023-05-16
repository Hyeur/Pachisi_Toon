using Photon.Pun;
using TMPro;
using UnityEngine;

public class PhotonStatus : MonoBehaviour
{
    public string photonStatus;
    public TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        this.photonStatus = PhotonNetwork.NetworkClientState.ToString();
        this.textMeshProUGUI.text = this.photonStatus;
    }
}
