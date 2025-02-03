using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

class AudioDetector
{
    static void Main()
    {
        using (var capture = new WasapiLoopbackCapture())
        {
            bool isAudioPlaying = false;
            capture.DataAvailable += (s, a) =>
            {
                float max = 0;
                var buffer = new WaveBuffer(a.Buffer);
                for (int i = 0; i < a.BytesRecorded / 4; i++)
                {
                    var sample = Math.Abs(buffer.FloatBuffer[i]);
                    if (sample > max) max = sample;
                }
                isAudioPlaying = max > 0.01f;
            };

            capture.StartRecording();
            System.Threading.Thread.Sleep(500); // Kurze Wartezeit für die Erkennung
            capture.StopRecording();

            Console.Write(isAudioPlaying.ToString().ToLower());
        }
    }
}
