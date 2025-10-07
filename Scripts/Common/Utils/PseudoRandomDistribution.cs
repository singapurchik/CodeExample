using System.Collections.Generic;
using UnityEngine;
using System;

namespace FAS
{
	public static class PseudoRandomDistribution
	{
		static readonly (float Probability, double Coefficient)[] ValvePrecomputed =
		{
			(0.05f, 0.003801658303553139101756466),
			(0.10f, 0.014745844781072675877050816),
			(0.15f, 0.032220914373087674975117359),
			(0.20f, 0.055704042949781851858398652),
			(0.25f, 0.084744091852316990275274806),
			(0.30f, 0.118949192725403987583755553),
			(0.35f, 0.157983098125747077557540462),
			(0.40f, 0.201547413607754017070679639),
			(0.45f, 0.249306998440163189714677100),
			(0.50f, 0.302103025348741965169160432),
			(0.55f, 0.360397850933168697104686803),
			(0.60f, 0.422649730810374235490851220),
			(0.65f, 0.481125478337229174401911323),
			(0.70f, 0.571428571428571428571428572),
			(0.75f, 0.666666666666666666666666667),
			(0.80f, 0.750000000000000000000000000),
			(0.85f, 0.823529411764705882352941177),
			(0.90f, 0.888888888888888888888888889),
			(0.95f, 0.947368421052631578947368421),
		};

		static readonly Dictionary<int, double> ValveCoefficientCache = new(64);
		static readonly Dictionary<int, double> LinearStepCache = new(64);
		static readonly System.Random GlobalRandom = new();

		public static double GetValveCoefficientCached(float baseProbability)
		{
			baseProbability = Mathf.Clamp01(baseProbability);
			for (int i = 0; i < ValvePrecomputed.Length; i++)
				if (Mathf.Abs(ValvePrecomputed[i].Probability - baseProbability) < 1e-6f)
					return ValvePrecomputed[i].Coefficient;

			int key = Mathf.RoundToInt(baseProbability * 10000f);
			if (ValveCoefficientCache.TryGetValue(key, out double cached))
				return cached;

			double left = 1e-9, right = 0.99, target = baseProbability;
			for (int i = 0; i < 80; i++)
			{
				double middle = 0.5 * (left + right);
				double average = averagePerAttemptValve(middle);
				if (average > target) right = middle; else left = middle;
			}
			double result = 0.5 * (left + right);
			ValveCoefficientCache[key] = result;
			return result;

			static double averagePerAttemptValve(double coefficient)
			{
				if (coefficient <= 0) return 0;
				int maxStep = (int)Math.Ceiling(1.0 / coefficient);
				double survival = 1.0;
				double expectedLength = 0.0;
				for (int step = 1; step <= maxStep; step++)
				{
					double chance = step * coefficient;
					if (chance > 1.0) chance = 1.0;
					double probability = survival * chance;
					expectedLength += step * probability;
					survival *= (1.0 - chance);
					if (survival <= 1e-16) break;
				}
				return expectedLength > 0 ? 1.0 / expectedLength : 1.0;
			}
		}

		public static double GetLinearStepCached(float baseProbability)
		{
			baseProbability = Mathf.Clamp01(baseProbability);
			int key = Mathf.RoundToInt(baseProbability * 10000f);
			if (LinearStepCache.TryGetValue(key, out double cached))
				return cached;

			double left = 0.0, right = 1.0 - baseProbability, target = baseProbability;
			for (int i = 0; i < 80; i++)
			{
				double middle = 0.5 * (left + right);
				double average = AveragePerAttemptLinear(baseProbability, middle);
				if (average > target) right = middle; else left = middle;
			}
			double result = 0.5 * (left + right);
			LinearStepCache[key] = result;
			return result;

			static double AveragePerAttemptLinear(double baseProbability, double step)
			{
				if (baseProbability <= 0) return 0;
				double survival = 1.0;
				double expectedLength = 0.0;
				for (int n = 1; n < 100000; n++)
				{
					double p = baseProbability + (n - 1) * step;
					if (p > 1.0) p = 1.0;
					double probability = survival * p;
					expectedLength += n * probability;
					survival *= (1.0 - p);
					if (survival <= 1e-16 || p >= 1.0) break;
				}
				return expectedLength > 0 ? 1.0 / expectedLength : 1.0;
			}
		}

		public static float ChanceValve(double coefficient, int failedAttempts)
		{
			double chance = coefficient * (failedAttempts + 1);
			if (chance > 1.0) chance = 1.0;
			return (float)chance;
		}

		public static float ChanceLinear(float baseProbability, double step, int failedAttempts)
		{
			double chance = baseProbability + failedAttempts * step;
			if (chance > 1.0) chance = 1.0;
			return (float)chance;
		}

		public static bool TryValve(double coefficient, ref int failedAttempts, System.Random random = null)
		{
			float chance = ChanceValve(coefficient, failedAttempts);
			random ??= GlobalRandom;
			bool success = random.NextDouble() < chance;
			if (success) failedAttempts = 0; else failedAttempts++;
			return success;
		}

		public static bool TryLinear(float baseProbability, double step, ref int failedAttempts, System.Random random = null)
		{
			float chance = ChanceLinear(baseProbability, step, failedAttempts);
			random ??= GlobalRandom;
			bool success = random.NextDouble() < chance;
			if (success) failedAttempts = 0; else failedAttempts++;
			return success;
		}
	}
}