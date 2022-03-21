using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NAudio.Wave;

namespace CSPL
{
    public static class AudioImport
    {
        public static AudioClip Import(string path)
        {
            float[] audioData = null;
            AudioFileReader fReader = new AudioFileReader(path);
            WaveFormat format = fReader.WaveFormat;

            var wholeFile = new List<float>((int)(fReader.Length / 4));
            var readBuffer = new float[fReader.WaveFormat.SampleRate * fReader.WaveFormat.Channels];
            int samplesRead;
            while ((samplesRead = fReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }
            audioData = wholeFile.ToArray();

            int lengthSamples = (int)fReader.Length / (format.BitsPerSample / 8);
            string name = Path.GetFileNameWithoutExtension(path);
            var audioClip = AudioClip.Create(name, lengthSamples / format.Channels, format.Channels, format.SampleRate, false);

            audioClip.SetData(audioData, format.Channels);
            audioClip.name = name;

            return audioClip;
        }
    }
}
