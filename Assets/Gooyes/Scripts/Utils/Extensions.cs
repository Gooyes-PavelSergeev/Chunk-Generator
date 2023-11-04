using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Threading;

namespace GooyesPlugin
{
    public static class Extensions
    {
        public static T Random<T>(this List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                return list[index];
            }
            return default(T);
        }

        public static void Shuffle<T>(this List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<int> GetRandomNumbers(int min, int max, int amount)
        {
            List<int> randomNumbers = new List<int>();

            if (min != max && min < max)
            {
                int possibleValuesCount = max - min;
                if (amount == possibleValuesCount)
                {
                    for (int i = min; i < max; ++i)
                    {
                        randomNumbers.Add(i);
                    }
                }
                else if (amount < possibleValuesCount)
                {
                    int count = 0;
                    int maxCount = 100;
                    do
                    {
                        --maxCount;
                        int number = UnityEngine.Random.Range(min, max);
                        if (!randomNumbers.Contains(number))
                        {
                            randomNumbers.Add(number);
                        }
                    }
                    while (maxCount > 0 && count < amount);
                }

            }
            return randomNumbers;
        }

        public static int HashIntVector(Vector2Int v)
        {
            int a = v.x;
            int b = v.y;
            var A = (uint)(a >= 0 ? 2 * a : -2 * a - 1);
            var B = (uint)(b >= 0 ? 2 * b : -2 * b - 1);
            var C = (int)((A >= B ? A * A + A + B : A + B * B) / 2);
            return a < 0 && b < 0 || a >= 0 && b >= 0 ? C : -C - 1;
        }

        public static bool IsOnScreen(this Camera camera, Vector3 worldPos, float widthOffset = 0, float heightOffset = 0)
        {

            float minX = -widthOffset;
            float maxX = Screen.width + widthOffset;
            float minY = -heightOffset;
            float maxY = Screen.height + heightOffset;

            Vector3 pos = camera.WorldToScreenPoint(worldPos);

            return pos.x > minX && pos.x < maxX && pos.y > minY && pos.y < maxY;
        }

        public static int GetRandomNear(this int value, int delta)
        {
            int min = Mathf.Max(1, value - delta);
            int max = Mathf.Max(2, value + delta);
            return UnityEngine.Random.Range(min, max);
        }

        public static Vector3 GetPosNearInCircleRadius(this Vector3 initialPos, float radius)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * radius;
            return new Vector3(initialPos.x + offset.x, initialPos.y + offset.y, initialPos.z);
        }

        public static bool Valid(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static T GetSafe<T>(this List<T> list, int index)
        {
            if (list != null && list.Count > index && index >= 0)
            {
                return list[index];
            }
            return default(T);
        }

        public static T GetSafe<T>(this T[] array, int index)
        {
            if (array != null && array.Length > index && index >= 0)
            {
                return array[index];
            }
            return default(T);
        }

        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this Transform target, Vector3 endValue, float duration, Ease ease, float magnitude = 1, float period = 1)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate (Vector3 x)
            {
                target.position = x;
            }, endValue, duration);
            tweenerCore.SetOptions(false).SetTarget(target).SetEase(ease, magnitude, period);
            return tweenerCore;
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static Transform FindTransform(this Transform transform, string name)
        {

            { // NOTE: Termination cases
                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                if (transform.name.Equals(name))
                {
                    return transform;
                }

                if (transform == null || transform.childCount == 0)
                {
                    return null;
                }
            }

            foreach (Transform child in transform)
            {
                Transform t = FindTransform(child, name);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }

        public static string ToPercent(this float value, bool addSymbol = true)
        {
            int v = Mathf.FloorToInt(value * 100f);
            if (addSymbol)
            {
                return v + "%";
            }

            return v.ToString();
        }

        public static Vector2 WorldToCanvasCoords(this Vector3 pos, Vector2 canvasSize)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(pos);
            Vector2 result = new Vector2(((viewportPosition.x - 0.5f) * canvasSize.x), ((viewportPosition.y - 0.5f) * canvasSize.y));

            return result;
        }

        public static string RemoveNewLinesAndSpaces(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
            }
            return string.Empty;
        }

        public static bool IsInRange(this int value, int minInclusive, int maxInclusive)
        {
            return value >= minInclusive && value <= maxInclusive;
        }

        public static void SleepAndLogDisable()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Debug.unityLogger.logEnabled = Debug.isDebugBuild;
        }

        public static string ToStringForLog<T>(this List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder builed = new StringBuilder();
                builed.Append("( ");
                foreach (T value in list)
                {
                    if (value != null)
                    {
                        builed.Append(value.ToString());
                        builed.Append("\t");
                    }
                }
                builed.Append(" )");
                return builed.ToString();
            }
            return "";
        }

        public static T GetRandomValue<T>(List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                int RndIndex = UnityEngine.Random.Range(0, list.Count);
                return list[RndIndex];
            }
            return default;
        }

        public static string RemoveAllWhitespacesAndNewLines(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
            }
            return str;
        }

        public static int Distance(this Vector2Int point, Vector2Int to)
        {
            return Mathf.Abs(point.x - to.x) + Mathf.Abs(point.y - to.y);
        }

        public static float GetRandomInRange(this Vector2 range)
        {
            float first = range.x;
            float second = range.y;
            if (first > second)
            {
                float buffer = first;
                first = second;
                second = buffer;
            }
            return UnityEngine.Random.Range(first, second);
        }
        public static int GetRandomInRange(this Vector2Int range)
        {
            return UnityEngine.Random.Range(range.x, range.y);
        }

        public static float AngleYBetween(this Vector3 position, Vector3 target)
        {
            position.y = target.y = 0;
            Vector3 direction = target - position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 euler = rotation.eulerAngles;
            float angle = euler.y;
            return angle;
        }


        public static CancellationTokenSource Toggle(this CancellationTokenSource token, bool active)
        {
            if (token != null)
            {
                token.Cancel();
            }

            if (active)
            {
                if (token != null)
                {
                    token.Dispose();
                }
                token = new CancellationTokenSource();
            }
            return token;
        }
    }
}

