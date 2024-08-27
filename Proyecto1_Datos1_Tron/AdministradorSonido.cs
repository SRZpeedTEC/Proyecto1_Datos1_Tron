using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Datos1_Tron
{
    public class AdministradorSonido
    {
        private List<WaveOutEvent> waveOutEvents = new List<WaveOutEvent>();

        public void ReproducirSonido(string rutaSonido)
        {
            var waveOut = new WaveOutEvent();
            var audioFile = new AudioFileReader(rutaSonido);
            waveOut.Init(audioFile);
            waveOut.Play();

            waveOutEvents.Add(waveOut);

            waveOut.PlaybackStopped += (sender, args) =>
            {
                waveOut.Dispose();
                audioFile.Dispose();
                waveOutEvents.Remove(waveOut);
            };
        }
    }
}
