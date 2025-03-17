using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace UI.Extension
{
   public static class CanvasGroupExtension
	{
		public enum WindowAnimationType
		{
			None = 0,
			Random = 100,
			Scale = 200,
			MoveLeft = 300,
			MoveRight = 301,
			MoveUp = 302,
			MoveDown = 303,
		}

		private static readonly List<WindowAnimationType> animations = Enum.GetValues(typeof(WindowAnimationType))
		                                                                   .Cast<WindowAnimationType>()
		                                                                   .Where(x => x != WindowAnimationType.None
			                                                                   && x != WindowAnimationType.Random)
		                                                                   .ToList();
		private static readonly Random random = new();
		
		public static void Show(this CanvasGroup canvasGroup,
		                        float duration = 0,
		                        float delay = 0,
		                        Action callback = null,
		                        (Vector3 cashedPos, RectTransform rect) rectTuple = default,
		                        WindowAnimationType animationType = WindowAnimationType.None)
		{
			if (duration < 0)
				throw new ArgumentException("Value cannot be negative", nameof(duration));

			if (delay < 0)
				throw new ArgumentException("Value cannot be negative", nameof(delay));

			canvasGroup.DOKill();
			if (duration == 0 && delay == 0)
				canvasGroup.alpha = 1;
			else if (rectTuple != default)
				ApplyShowAnimation(canvasGroup, duration, rectTuple, animationType);

			canvasGroup.DOFade(1, duration)
			           .SetDelay(delay)
			           .SetLink(canvasGroup.gameObject)
			           .SetUpdate(true)
			           .OnComplete(() =>
			           {
				           canvasGroup.interactable = true;
				           canvasGroup.blocksRaycasts = true;
				           callback?.Invoke();
			           });
		}
		public static void Hide(this CanvasGroup canvasGroup,
		                        float duration = 0,
		                        float delay = 0,
		                        Action callback = null,
		                        (Vector3 cashedPos, RectTransform rect) rectTuple = default,
		                        WindowAnimationType animationType = WindowAnimationType.None)
		{
			if (duration < 0)
				throw new ArgumentException("Value cannot be negative", nameof(duration));

			if (delay < 0)
				throw new ArgumentException("Value cannot be negative", nameof(delay));

			canvasGroup.DOKill();
			if (duration == 0 && delay == 0)
				canvasGroup.alpha = 0;
			else if (animationType != default)
				ApplyHideAnimation(canvasGroup, duration, rectTuple, animationType);

			canvasGroup.DOFade(0, duration)
			           .SetDelay(delay)
			           .SetLink(canvasGroup.gameObject)
			           .SetUpdate(true)
			           .OnStart(() =>
			           {
				           canvasGroup.interactable = false;
				           canvasGroup.blocksRaycasts = false;
			           })
			           .OnComplete(() =>
			           {
				           callback?.Invoke();
			           });
		}

		private static void ApplyShowAnimation(this CanvasGroup canvasGroup,
		                                       float duration,
		                                       (Vector3 cashedPos, RectTransform rect) rectTuple,
		                                       WindowAnimationType animationType)
		{
			RectTransform canvasRect = rectTuple.rect;
			float screenWidth = Screen.width;
			float screenHeight = Screen.height;
			Vector3 from;
			Vector3 to = rectTuple.cashedPos;

			switch (animationType)
			{
				case WindowAnimationType.Random:
					ApplyShowAnimation(canvasGroup,
					                   duration,
					                   (rectTuple.cashedPos, rectTuple.rect),
					                   animations[random.Next(animations.Count)]);
					break;
				case WindowAnimationType.Scale:
					AnimationMove(canvasRect, default, rectTuple.cashedPos, rectTuple.cashedPos);
					AnimationScale(canvasRect, duration, Vector3.zero, Vector3.one);
					break;
				case WindowAnimationType.MoveLeft:
					from = new Vector3(-canvasRect.rect.width, screenHeight / 2);
					AnimationMove(canvasRect, duration, from, to);
					break;
				case WindowAnimationType.MoveRight:
					from = new Vector3(screenWidth + canvasRect.rect.width, screenHeight / 2);
					AnimationMove(canvasRect, duration, from, to);
					break;
				case WindowAnimationType.MoveUp:
					from = new Vector3(screenWidth / 2, screenHeight + canvasRect.rect.height);
					AnimationMove(canvasRect, duration, from, to);
					break;
				case WindowAnimationType.MoveDown:
					from = new Vector3(screenWidth / 2, -canvasRect.rect.height);
					AnimationMove(canvasRect, duration, from, to);
					break;
			}
		}

		private static void ApplyHideAnimation(this CanvasGroup canvasGroup,
		                                       float duration,
		                                       (Vector3 cashedPos, RectTransform rect) rectTuple,
		                                       WindowAnimationType animationType)
		{
			RectTransform canvasRect = rectTuple.rect;
			float screenWidth = Screen.width;
			float screenHeight = Screen.height;
			Vector3 from = rectTuple.cashedPos;
			Vector3 to;

			switch (animationType)
			{
				case WindowAnimationType.Random:
					ApplyHideAnimation(canvasGroup,
					                   duration,
					                   (rectTuple.cashedPos, rectTuple.rect),
					                   animations[random.Next(animations.Count)]);
					break;
				case WindowAnimationType.Scale:
					AnimationScale(canvasRect, duration, Vector3.one, Vector3.zero);
					break;
				case WindowAnimationType.MoveLeft:
					to = new Vector3(-canvasRect.rect.width, screenHeight / 2);
					AnimationMove(canvasRect, duration, from, to);
					break;
				case WindowAnimationType.MoveRight:
					to = new Vector3(screenWidth + canvasRect.rect.width, screenHeight / 2);
					AnimationMove(canvasRect, duration, from, to);
					break;
				case WindowAnimationType.MoveUp:
					to = new Vector3(screenWidth / 2, screenHeight + canvasRect.rect.height);
					AnimationMove(canvasRect, duration, from, to);
					break;
				case WindowAnimationType.MoveDown:
					to = new Vector3(screenWidth / 2, -canvasRect.rect.height);
					AnimationMove(canvasRect, duration, from, to);
					break;
			}
		}

		private static void AnimationScale(RectTransform rectTransform, float duration, Vector3 from, Vector3 to)
		{
			rectTransform.DOKill();
			rectTransform.DOScale(to, duration).From(from).SetLink(rectTransform.gameObject).SetUpdate(true);
		}

		private static void AnimationMove(RectTransform rectTransform, float duration, Vector3 from, Vector3 to)
		{
			rectTransform.transform.localScale = Vector3.one;
			rectTransform.DOKill();
			rectTransform.DOMove(to, duration).From(from).SetLink(rectTransform.gameObject).SetUpdate(true);
		}
	}
}