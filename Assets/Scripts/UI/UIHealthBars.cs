using UnityEngine;
using UnityEngine.UI;

public class UIHealthBars : MaskableGraphic, ILayoutElement {

	[SerializeField] int Height = 8;
	[SerializeField] int Border = 2;
	[Space]
	[SerializeField] Color Background = default;
	[SerializeField] new Camera camera = default;

	#region ILayoutElement implementation
	int ILayoutElement.layoutPriority => 0;
	float ILayoutElement.flexibleHeight => -1;
	float ILayoutElement.flexibleWidth => -1;
	float ILayoutElement.minHeight => 0;
	float ILayoutElement.minWidth => 0;
	float ILayoutElement.preferredHeight => Screen.height;
	float ILayoutElement.preferredWidth => Screen.width;

	void ILayoutElement.CalculateLayoutInputHorizontal() { }
	void ILayoutElement.CalculateLayoutInputVertical() { }
	#endregion

	UIVertex[][] quads_bg;
	UIVertex[][] quads_fg;

	protected override void Awake() {
		base.Awake();
		InitArrays();
		raycastTarget = false;
	}

	void Update() => SetVerticesDirty();

	protected override void OnPopulateMesh(VertexHelper vh) {
		vh.Clear();
		InitArrays();


		var canvasRect = canvas.GetComponent<RectTransform>().rect;
		var healthbarsCount = HealthBar.Active.Count;

		for (int i = 0; i < healthbarsCount; i++) {
			var hb = HealthBar.Active[i];
			var pos = hb.transform.position + new Vector3(0, hb.yOffset, 0);

			DrawHealthBarWorld(vh, camera, canvasRect, i, pos, hb.DrawPercentage, hb.width, hb.color, hb.opacity);
		}
	}

	void DrawHealthBarWorld(VertexHelper vh, Camera camera, Rect r, int index, Vector3 worldPos, float fill, float width, Color friendly, float alpha) {
		DrawHealthBarScreen(vh, r, index, camera.WorldToViewportPoint(worldPos), fill, width, friendly, alpha);
	}

	void DrawHealthBarScreen(VertexHelper vh, Rect r, int index, Vector2 screen, float fill, float width, Color color, float alpha) {
		Color bg;
		bg = Background;
		bg.a = alpha;

		Color fg;
		fg = color;
		fg.a = alpha;

		const int BOT_LEFT = 0;
		const int BOT_RIGHT = 1;
		const int TOP_RIGHT = 2;
		const int TOP_LEFT = 3;

		screen.x = r.width * (screen.x);
		screen.y = r.height * (screen.y);

		quads_bg[index][BOT_LEFT].color = bg;
		quads_bg[index][BOT_RIGHT].color = bg;
		quads_bg[index][TOP_RIGHT].color = bg;
		quads_bg[index][TOP_LEFT].color = bg;

		quads_fg[index][BOT_LEFT].color = fg;
		quads_fg[index][BOT_RIGHT].color = fg;
		quads_fg[index][TOP_RIGHT].color = fg;
		quads_fg[index][TOP_LEFT].color = fg;

		var bot_left = screen + new Vector2(-width / 2, -Height / 2);
		var bot_right = screen + new Vector2(+width / 2, -Height / 2);
		var top_right = screen + new Vector2(+width / 2, +Height / 2);
		var top_left = screen + new Vector2(-width / 2, +Height / 2);

		quads_bg[index][BOT_LEFT].position = bot_left;
		quads_bg[index][BOT_RIGHT].position = bot_right;
		quads_bg[index][TOP_RIGHT].position = top_right;
		quads_bg[index][TOP_LEFT].position = top_left;

		vh.AddUIVertexQuad(quads_bg[index]);

		// only add foreground if we have any fill value
		// otherwise we might get 1px errors when healthbar is empty
		if (fill > 0) {
			bot_left += new Vector2(Border, Border);
			bot_right += new Vector2(-Border, Border);
			top_right += new Vector2(-Border, -Border);
			top_left += new Vector2(Border, -Border);

			bot_right.x = bot_left.x + ((bot_right.x - bot_left.x) * fill);
			top_right.x = top_left.x + ((top_right.x - top_left.x) * fill);

			quads_fg[index][BOT_LEFT].position = bot_left;
			quads_fg[index][BOT_RIGHT].position = bot_right;
			quads_fg[index][TOP_RIGHT].position = top_right;
			quads_fg[index][TOP_LEFT].position = top_left;

			vh.AddUIVertexQuad(quads_fg[index]);
		}
	}

	void InitArrays() {
		if (quads_bg == null || quads_fg == null || quads_bg.Length < HealthBar.Active.Count) {
			quads_bg = new UIVertex[System.Math.Max(64, HealthBar.Active.Count)][];
			quads_fg = new UIVertex[quads_bg.Length][];

			for (int i = 0; i < quads_bg.Length; ++i) {
				quads_bg[i] = new UIVertex[4];
				quads_fg[i] = new UIVertex[4];
			}
		}
	}
}