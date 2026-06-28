using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Token: 0x0200000E RID: 14
public class Scene3JoinController : MonoBehaviourPunCallbacks
{
	// Token: 0x0600003A RID: 58 RVA: 0x00003078 File Offset: 0x00001278
	private void Start()
	{
		this.canvasGroup = this.notificationPanel.GetComponent<CanvasGroup>();
		this.panelRect = this.notificationPanel.GetComponent<RectTransform>();
		this.shownPos = this.panelRect.anchoredPosition;
		this.hiddenPos = this.shownPos + new Vector2(0f, 20f);
		this.panelRect.anchoredPosition = this.hiddenPos;
		this.canvasGroup.alpha = 0f;
		this.notificationPanel.SetActive(false);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003105 File Offset: 0x00001305
	private void Update()
	{
		if ((this.sessionCodeInput.isFocused || this.nameInput.isFocused) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
		{
			this.ValidateAndJoin();
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003140 File Offset: 0x00001340
	public void ValidateAndJoin()
	{
		string text = this.sessionCodeInput.text.Trim().ToUpper();
		string text2 = this.nameInput.text.Trim();
		bool flag = !string.IsNullOrEmpty(text);
		bool flag2 = !string.IsNullOrEmpty(text2);
		if (flag2 && !flag)
		{
			this.ShowNotification("ENTER SESSION CODE", false);
			return;
		}
		if (flag && !flag2)
		{
			this.ShowNotification("ENTER YOUR NAME", false);
			return;
		}
		if (!flag && !flag2)
		{
			this.ShowNotification("ENTER SESSION CODE AND YOUR NAME", false);
			return;
		}
		this.ShowNotification("Joining session...", true);
		PhotonLauncher.Instance.JoinRoom(text, text2);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000031D9 File Offset: 0x000013D9
	public override void OnJoinedRoom()
{
    this.ShowNotification("You entered the session. Waiting for your instructor to start.", true);
    Debug.Log("Student joined room successfully.");

    if (RoomTermsManager.Instance != null)
    {
        RoomTermsManager.Instance.LoadTermsFromRoom();
    }

    SceneManager.LoadScene("Scene_4 Hessa Character Select");
}

	// Token: 0x0600003E RID: 62 RVA: 0x000031F1 File Offset: 0x000013F1
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		this.ShowNotification("INVALID SESSION CODE", false);
		Debug.LogWarning("Join room failed: " + message);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000320F File Offset: 0x0000140F
	private void ShowNotification(string message, bool success)
	{
		if (this.notificationRoutine != null)
		{
			base.StopCoroutine(this.notificationRoutine);
		}
		this.notificationRoutine = base.StartCoroutine(this.AnimateNotification(message, success));
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00003239 File Offset: 0x00001439
	private IEnumerator AnimateNotification(string message, bool success)
	{
		this.notificationPanel.SetActive(true);
		this.notificationText.text = message;
		this.notificationText.color = (success ? this.successTextColor : this.warningTextColor);
		if (this.notificationBackground != null)
		{
			this.notificationBackground.color = (success ? this.successBgColor : this.warningBgColor);
		}
		if (this.notificationOutline != null)
		{
			this.notificationOutline.effectColor = (success ? this.successBorderColor : this.warningBorderColor);
		}
		if (this.notificationIcon != null)
		{
			this.notificationIcon.color = (success ? this.successTextColor : this.warningTextColor);
		}
		this.canvasGroup.alpha = 0f;
		this.panelRect.anchoredPosition = this.hiddenPos;
		float showDuration = 0.2f;
		float elapsed = 0f;
		while (elapsed < showDuration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / showDuration;
			this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
			this.panelRect.anchoredPosition = Vector2.Lerp(this.hiddenPos, this.shownPos, t);
			yield return null;
		}
		this.canvasGroup.alpha = 1f;
		this.panelRect.anchoredPosition = this.shownPos;
		yield break;
	}

	// Token: 0x04000033 RID: 51
	[Header("Inputs")]
	public TMP_InputField sessionCodeInput;

	// Token: 0x04000034 RID: 52
	public TMP_InputField nameInput;

	// Token: 0x04000035 RID: 53
	[Header("Notification UI")]
	public GameObject notificationPanel;

	// Token: 0x04000036 RID: 54
	public TextMeshProUGUI notificationText;

	// Token: 0x04000037 RID: 55
	public Image notificationIcon;

	// Token: 0x04000038 RID: 56
	public Image notificationBackground;

	// Token: 0x04000039 RID: 57
	public Outline notificationOutline;

	// Token: 0x0400003A RID: 58
	[Header("Notification Colors")]
	public Color warningTextColor = new Color32(byte.MaxValue, 138, 138, byte.MaxValue);

	// Token: 0x0400003B RID: 59
	public Color successTextColor = new Color32(170, byte.MaxValue, 190, byte.MaxValue);

	// Token: 0x0400003C RID: 60
	public Color warningBgColor = new Color32(42, 15, 22, 235);

	// Token: 0x0400003D RID: 61
	public Color successBgColor = new Color32(16, 42, 28, 235);

	// Token: 0x0400003E RID: 62
	public Color warningBorderColor = new Color32(byte.MaxValue, 122, 122, byte.MaxValue);

	// Token: 0x0400003F RID: 63
	public Color successBorderColor = new Color32(120, byte.MaxValue, 170, byte.MaxValue);

	// Token: 0x04000040 RID: 64
	private CanvasGroup canvasGroup;

	// Token: 0x04000041 RID: 65
	private RectTransform panelRect;

	// Token: 0x04000042 RID: 66
	private Coroutine notificationRoutine;

	// Token: 0x04000043 RID: 67
	private Vector2 shownPos;

	// Token: 0x04000044 RID: 68
	private Vector2 hiddenPos;
}
