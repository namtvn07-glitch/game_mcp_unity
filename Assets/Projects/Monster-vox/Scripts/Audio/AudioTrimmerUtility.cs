using UnityEngine;

namespace MonsterVox.Audio
{
    public static class AudioTrimmerUtility
    {
        /// <summary>
        /// Reads an AudioClip and trims leading and trailing silence based on a dB threshold.
        /// Does NOT create garbage string allocations.
        /// </summary>
        public static AudioClip TrimSilence(AudioClip originalClip, float noiseGateDB)
        {
            if (originalClip == null) return null;

            int totalSamples = originalClip.samples * originalClip.channels;
            if (totalSamples == 0) return null;

            float[] samples = new float[totalSamples];
            originalClip.GetData(samples, 0);

            int startSample = 0;
            int endSample = totalSamples - 1;
            bool foundAudio = false;

            // Forward scan to find Start
            for (int i = 0; i < totalSamples; i++)
            {
                // Ensure no Log10(0) by adding Epsilon
                float db = 20f * Mathf.Log10(Mathf.Abs(samples[i]) + float.Epsilon);
                if (db >= noiseGateDB)
                {
                    startSample = i;
                    foundAudio = true;
                    break;
                }
            }

            // If completely silent
            if (!foundAudio)
            {
                return null;
            }

            // Backward scan to find End
            for (int i = totalSamples - 1; i >= startSample; i--)
            {
                float db = 20f * Mathf.Log10(Mathf.Abs(samples[i]) + float.Epsilon);
                if (db >= noiseGateDB)
                {
                    endSample = i;
                    break;
                }
            }

            int newSampleCount = endSample - startSample + 1;
            // Additional check
            if (newSampleCount <= 0) return null;

            float[] trimmedSamples = new float[newSampleCount];
            System.Array.Copy(samples, startSample, trimmedSamples, 0, newSampleCount);

            // Create GC clean string by ignoring dynamic concatenations
            AudioClip trimmedClip = AudioClip.Create("TrimmedVoiceClip", newSampleCount / originalClip.channels, originalClip.channels, originalClip.frequency, false);
            trimmedClip.SetData(trimmedSamples, 0);

            return trimmedClip;
        }
    }
}
