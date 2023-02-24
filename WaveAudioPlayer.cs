using System.Runtime.InteropServices;

namespace NetAudio
{
    public class WaveaudioPlayer
    {
        private readonly string _prefix = "wv_";

        [DllImport("Winmm.dll", SetLastError = true)]
        static extern uint mciSendString(string lpszCommand,
            IntPtr lpszReturnString,
            uint cchReturn,
            IntPtr hwndCallback);
        
        [DllImport("Winmm.dll", SetLastError = true)]
        static extern bool mciGetErrorString(uint fdwError,
            IntPtr lpszErrorText,
            uint cchErrorText);

        /// <summary>
        /// Opens and play a .wav audio file
        /// </summary>
        /// <param name="path">Path to find the .wav file</param>
        /// <param name="name">Name to reference this particular audio</param>
        public void Play(string path, string name)
        {
            string fullname = _prefix + name;
            OperationResult result = OpenDevice(path, fullname);
            if(!result.Succeeded)
                Console.WriteLine("Failed to load audio: " + result.Message);
            else
            {
                var code = mciSendString($"play {fullname}", IntPtr.Zero, 0, IntPtr.Zero);
                if (code != 0)
                    Console.WriteLine(GetErrorMessage(code));
            }
        }

        /// <summary>
        /// Closes an audio file
        /// </summary>
        /// <param name="name">Reference name of the audio file created</param>
        public void Close(string name)
        {
            string fullname = _prefix + name;

            var code = mciSendString($"close {fullname}", IntPtr.Zero, 0, IntPtr.Zero);
            if (code != 0)
                Console.WriteLine(GetErrorMessage(code));
        }

        private OperationResult OpenDevice(string path, string name)
        {
            var code = mciSendString($"open {path} type waveaudio alias {name}", IntPtr.Zero, 0, IntPtr.Zero);

            if (code != 0)
                return new OperationResult(false, GetErrorMessage(code));
            else
                return new OperationResult(true, "");
        }

        private string GetErrorMessage(uint code)
        {
            IntPtr p = Marshal.AllocHGlobal(BufferSize.LARGE);
            mciGetErrorString(code, p, BufferSize.LARGE);

            string errorMessage = Marshal.PtrToStringAnsi(p);
            Marshal.FreeHGlobal(p);

            return errorMessage;
        }
    }
}